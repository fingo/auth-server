using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.AuthServer.Configuration;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Policies.CheckingFunctions;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Enums;
using Fingo.Auth.Domain.Policies.ExtensionMethods;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.JsonWrapper;

namespace Fingo.Auth.AuthServer.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly TimeSpan _authenticationSpan = TimeSpan.FromMinutes(20);
        private readonly IGetPoliciesFromProjectFactory _getPoliciesFromProjectFactory;
        private readonly IGetUserCustomDataListFromProjectFactory _getUserCustomDataListFromProjectFactory;
        private readonly IHashingService _hashingService;
        private readonly IJwtLibraryWrapperService _jwtLibraryWrapperService;
        private readonly ILogger<TokenService> _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        private readonly IGetUserPolicyConfigurationOrDefaultFromProjectFactory
            userConfigurationOrDefaultFromProjectFactory;

        public TokenService(IUserRepository userRepository , IProjectRepository projectRepository ,
            IJwtLibraryWrapperService jwtLibraryWrapperService , ILogger<TokenService> logger ,
            IGetPoliciesFromProjectFactory getPoliciesFromProjectFactory , IHashingService hashingService ,
            IGetUserCustomDataListFromProjectFactory getUserCustomDataListFromProjectFactory ,
            IGetUserPolicyConfigurationOrDefaultFromProjectFactory userConfigurationOrDefaultFromProjectFactory)
        {
            _getUserCustomDataListFromProjectFactory = getUserCustomDataListFromProjectFactory;
            _getPoliciesFromProjectFactory = getPoliciesFromProjectFactory;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _jwtLibraryWrapperService = jwtLibraryWrapperService;
            _logger = logger;
            _hashingService = hashingService;
            this.userConfigurationOrDefaultFromProjectFactory = userConfigurationOrDefaultFromProjectFactory;
        }

        public string AcquireToken(string userLogin , string userPassword , Guid projectGuid)
        {
            var guidCheckResult = GuidUniquenessCheck(projectGuid);

            if (guidCheckResult != null)
                return guidCheckResult;

            // policies check
            try
            {
                var projectId = _projectRepository.GetByGuid(projectGuid).Id;
                var policyTuples = _getPoliciesFromProjectFactory.Create().Invoke(projectId).WithTypes(PolicyType.LogIn);

                var user = _userRepository.FindBy(u => u.Login == userLogin).FirstOrDefault();
                if (user != null)
                {
                    var userId = user.Id;
                    var userLastPasswordChange = user.LastPasswordChange;

                    foreach (var policyTuple in policyTuples)
                        switch (policyTuple.Item1)
                        {
                            case Policy.AccountExpirationDate:
                                var userConfiguration =
                                    (UserAccountExpirationDateConfiguration)
                                    userConfigurationOrDefaultFromProjectFactory.Create()
                                        .Invoke(userId , Policy.AccountExpirationDate , projectId);
                                if (
                                    !Check.AccountExpirationDate(
                                        (AccountExpirationDateConfiguration) policyTuple.Item2 , userConfiguration))
                                    return new JsonObject
                                    {
                                        Result = JsonValues.AccountExpired
                                    }.ToJson();
                                break;
                            case Policy.PasswordExpirationDate:
                                if (
                                    !Check.PasswordExpirationDate(
                                        (PasswordExpirationDateConfiguration) policyTuple.Item2 ,
                                        userLastPasswordChange))
                                    return new JsonObject
                                    {
                                        Result = JsonValues.PasswordExpired
                                    }.ToJson();
                                break;
                            default:
                                throw new Exception("Invalid policy");
                        }
                }
            }
            catch (Exception)
            {
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = "Could not find and check active policies."
                }.ToJson();
            }

            var userCheckResult = UserAuthenticationCheck(userLogin , userPassword);

            if (userCheckResult != null)
                return userCheckResult;

            var token = EncodePayload(userLogin , projectGuid);

            return new JsonObject
            {
                Result = JsonValues.Authenticated ,
                Jwt = token
            }.ToJson();
        }

        public string VerifyToken(string jwt , Guid projectGuid)
        {
            var guidCheckResult = GuidUniquenessCheck(projectGuid);

            if (guidCheckResult != null)
                return guidCheckResult;

            var decodeResult = _jwtLibraryWrapperService.Decode(jwt , JwtConfiguration.SecretKey);

            switch (decodeResult)
            {
                case DecodeResult.TokenValid:
                    return new JsonObject
                    {
                        Result = JsonValues.TokenValid
                    }.ToJson();
                case DecodeResult.TokenExpired:
                    return new JsonObject
                    {
                        Result = JsonValues.TokenExpired
                    }.ToJson();
                default:
                    return new JsonObject
                    {
                        Result = JsonValues.TokenInvalid
                    }.ToJson();
            }
        }

        private string GuidUniquenessCheck(Guid projectGuid)
        {
            IEnumerable<Project> projects;
            int projectsCount;

            try
            {
                projects =
                    _projectRepository.FindBy(p => p.ProjectGuid == projectGuid).WithStatuses(ProjectStatus.Active);
                projectsCount = projects.Count();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error , "Database error occured when server was looking for projects with GUID: "
                                             + projectGuid + ", exception message: " + e.Message);

                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails =
                        "Database error occured when server was looking for projects with GUID: " + projectGuid
                }.ToJson();
            }

            if (projectsCount == 0)
                return new JsonObject
                {
                    Result = JsonValues.WrongAccessToken
                }.ToJson();

            if (projectsCount >= 2)
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = "There is more than one project corresponding given GUID."
                }.ToJson();

            return null;
        }

        private string UserAuthenticationCheck(string userLogin , string userPassword)
        {
            IEnumerable<User> users;
            int usersCount;

            try
            {
                users = _userRepository.FindBy(u => u.Login == userLogin).WithStatuses(UserStatus.Active).ToList();
                usersCount = users.Count();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error , "Database error occured when server was looking for users with login: "
                                             + userLogin + ", exception message: " + e.Message);

                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = "Database error occured when server was looking for users with login: " + userLogin
                }.ToJson();
            }

            if (usersCount == 0)
                return new JsonObject
                {
                    Result = JsonValues.NotAuthenticated
                }.ToJson();

            if (usersCount >= 2)
                return new JsonObject
                {
                    Result = JsonValues.Error ,
                    ErrorDetails = "There is more than one user corresponding given login."
                }.ToJson();

            string hashedPassword;
            try
            {
                hashedPassword = _hashingService.HashWithDatabaseSalt(userPassword , userLogin);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error ,
                    $"HashWithDatabaseSalt((password), {userLogin}) threw exception: {e.Message}.");
                return new JsonObject
                {
                    Result = JsonValues.NotAuthenticated
                }.ToJson();
            }

            if (users.First().Password != hashedPassword)
                return new JsonObject
                {
                    Result = JsonValues.NotAuthenticated
                }.ToJson();

            return null;
        }

        private string EncodePayload(string userLogin , Guid projectGuid)
        {
            var unixEpoch = new DateTime(1970 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Utc);
            var exp = (int) Math.Round((DateTime.UtcNow.Add(_authenticationSpan) - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string , object>
            {
                {"login" , userLogin} ,
                {"project-guid" , projectGuid} ,
                {"exp" , exp}
            };

            var customDataList = _getUserCustomDataListFromProjectFactory.Create().Invoke(projectGuid , userLogin);
            foreach (var data in customDataList)
                payload.Add(data.Item1 , data.Item2);

            return _jwtLibraryWrapperService.Encode(payload , JwtConfiguration.SecretKey);
        }
    }
}