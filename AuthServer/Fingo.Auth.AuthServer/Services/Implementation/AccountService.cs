using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Policies.CheckingFunctions;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Enums;
using Fingo.Auth.Domain.Policies.ExtensionMethods;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.JsonWrapper;

namespace Fingo.Auth.AuthServer.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IAddUserFactory _addUserFactory;
        private readonly IChangePasswordToUserFactory _changePasswordToUserFactory;
        private readonly IGetAllProjectFactory _getAllProjectFactory;
        private readonly IGetPoliciesFromProjectFactory _getPoliciesFromProjectFactory;
        private readonly IHashingService _hashingService;
        private readonly IUserRepository _userRepository;

        public AccountService(IAddUserFactory addUserFactory , ILogger<AccountService> logger ,
            IChangePasswordToUserFactory changePasswordToUserFactory ,
            IGetPoliciesFromProjectFactory getPoliciesFromProjectFactory ,
            IGetAllProjectFactory getAllProjectFactory ,
            IUserRepository userRepository , IHashingService hashingService)
        {
            _getAllProjectFactory = getAllProjectFactory;
            _getPoliciesFromProjectFactory = getPoliciesFromProjectFactory;
            _addUserFactory = addUserFactory;
            _changePasswordToUserFactory = changePasswordToUserFactory;
            _userRepository = userRepository;
            _hashingService = hashingService;
        }

        public string CreateNewUserInProject(UserModel user , Guid projectGuid)
        {
            int projectId;
            try
            {
                projectId = _getAllProjectFactory.Create().Invoke().FirstOrDefault(p => p.ProjectGuid == projectGuid).Id;
            }
            catch (Exception)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find id of project with guid: {projectGuid}"
                }.ToJson();
            }

            List<Tuple<Policy , PolicyConfiguration>> list;
            try
            {
                list =
                    _getPoliciesFromProjectFactory.Create()
                        .Invoke(projectId)
                        .WithTypes(PolicyType.AccountCreation)
                        .ToList();
            }
            catch (Exception)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find policies of project with id: {projectId}"
                }.ToJson();
            }

            try
            {
                foreach (var policyTuple in list)
                    switch (policyTuple.Item1)
                    {
                        case Policy.MinimumPasswordLength:
                            if (
                                !Check.MinimumPasswordLength((MinimumPasswordLengthConfiguration) policyTuple.Item2 ,
                                    user.Password))
                                return new JsonObject {Result = JsonValues.PasswordLengthIncorrect}.ToJson();
                            break;
                        case Policy.RequiredPasswordCharacters:
                            if (
                                !Check.RequiredPasswordCharacters(
                                    (RequiredPasswordCharactersConfiguration) policyTuple.Item2 , user.Password))
                                return new JsonObject {Result = JsonValues.RequiredCharactersViolation}.ToJson();
                            break;
                        case Policy.ExcludeCommonPasswords:
                            if (
                                !Check.ExcludeCommonPasswords((ExcludeCommonPasswordsConfiguration) policyTuple.Item2 ,
                                    user.Password))
                                return new JsonObject {Result = JsonValues.PasswordTooCommon}.ToJson();
                            break;
                        default:
                            throw new Exception("Invalid policy.");
                    }
            }
            catch (Exception e)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Unable to check policies ({e.Message})."
                }.ToJson();
            }

            try
            {
                user.PasswordSalt = _hashingService.GenerateNewSalt();
                user.Password = _hashingService.HashWithGivenSalt(user.Password , user.PasswordSalt);
                _addUserFactory.Create().Invoke(user , projectGuid);
            }
            catch (Exception e)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"User was not created in project because of some internal error ({e.Message})."
                }.ToJson();
            }

            return new JsonObject {Result = JsonValues.UserCreatedInProject}.ToJson();
        }

        public string PasswordChangeForUser(ChangingPasswordUser user)
        {
            if (user.NewPassword != user.ConfirmNewPassword)
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = "Confirmed password must match new password."
                }.ToJson();

            IEnumerable<Project> projects;
            try
            {
                var id = _userRepository.GetAll().FirstOrDefault(x => x.Login == user.Email).Id;
                projects = _userRepository.GetAllProjectsFromUser(id);
            }
            catch
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find user's (login: {user.Email}) id and all projects that they are in."
                }.ToJson();
            }

            try
            {
                foreach (var project in projects)
                {
                    var policies = _getPoliciesFromProjectFactory.Create().Invoke(project.Id).ToList();

                    foreach (var policy in policies)
                        switch (policy.Item1)
                        {
                            case Policy.MinimumPasswordLength:
                                if (
                                    !Check.MinimumPasswordLength((MinimumPasswordLengthConfiguration) policy.Item2 ,
                                        user.NewPassword))
                                    return new JsonObject {Result = JsonValues.PasswordLengthIncorrect}.ToJson();
                                break;
                            case Policy.RequiredPasswordCharacters:
                                if (
                                    !Check.RequiredPasswordCharacters(
                                        (RequiredPasswordCharactersConfiguration) policy.Item2 , user.NewPassword))
                                    return new JsonObject {Result = JsonValues.RequiredCharactersViolation}.ToJson();
                                break;
                            case Policy.ExcludeCommonPasswords:
                                if (
                                    !Check.ExcludeCommonPasswords((ExcludeCommonPasswordsConfiguration) policy.Item2 ,
                                        user.NewPassword))
                                    return new JsonObject {Result = JsonValues.PasswordTooCommon}.ToJson();
                                break;
                        }
                }
            }
            catch
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find and check all policies for user (login: {user.Email})."
                }.ToJson();
            }

            try
            {
                user.Password = _hashingService.HashWithDatabaseSalt(user.Password , user.Email);
                user.NewPassword = _hashingService.HashWithDatabaseSalt(user.NewPassword , user.Email);
                _changePasswordToUserFactory.Create().Invoke(user);
            }
            catch (Exception e)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = e.Message
                }.ToJson();
            }

            return new JsonObject
            {
                Result = JsonValues.UsersPasswordChanged
            }.ToJson();
        }

        public string SetPasswordForUser(string token , string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.ActivationToken == token);

            if (user == null)
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find user with token: {token}."
                }.ToJson();

            var projects = _userRepository.GetAllProjectsFromUser(user.Id);
            if (projects == null)
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find projects of user with id: {user.Id}."
                }.ToJson();

            try
            {
                foreach (var project in projects)
                {
                    var policies = _getPoliciesFromProjectFactory.Create().Invoke(project.Id).ToList();

                    foreach (var policy in policies)
                        switch (policy.Item1)
                        {
                            case Policy.MinimumPasswordLength:
                                if (
                                    !Check.MinimumPasswordLength((MinimumPasswordLengthConfiguration) policy.Item2 ,
                                        password))
                                    return new JsonObject {Result = JsonValues.PasswordLengthIncorrect}.ToJson();
                                break;
                            case Policy.RequiredPasswordCharacters:
                                if (
                                    !Check.RequiredPasswordCharacters(
                                        (RequiredPasswordCharactersConfiguration) policy.Item2 , password))
                                    return new JsonObject {Result = JsonValues.RequiredCharactersViolation}.ToJson();
                                break;
                            case Policy.ExcludeCommonPasswords:
                                if (
                                    !Check.ExcludeCommonPasswords((ExcludeCommonPasswordsConfiguration) policy.Item2 ,
                                        password))
                                    return new JsonObject {Result = JsonValues.PasswordTooCommon}.ToJson();
                                break;
                        }
                }
            }
            catch (Exception e)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = $"Could not find and check all policies for user (id: {user.Id}) ({e.Message})."
                }.ToJson();
            }

            try
            {
                user.PasswordSalt = _hashingService.GenerateNewSalt();
                user.Password = _hashingService.HashWithGivenSalt(password , user.PasswordSalt);
                user.Status = UserStatus.Active;
                user.LastPasswordChange = DateTime.UtcNow;
                _userRepository.Edit(user);
            }
            catch (Exception e)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails =
                        $"Could not generate salt & password hash and edit user in the database ({e.Message})."
                }.ToJson();
            }

            return new JsonObject
            {
                Result = JsonValues.PasswordSet
            }.ToJson();
        }
    }
}