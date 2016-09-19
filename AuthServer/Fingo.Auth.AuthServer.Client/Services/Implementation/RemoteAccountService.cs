using System;
using System.Collections.Generic;
using Fingo.Auth.AuthServer.Client.Exceptions;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.JsonWrapper;
using Newtonsoft.Json;

namespace Fingo.Auth.AuthServer.Client.Services.Implementation
{
    public class RemoteAccountService : IRemoteAccountService
    {
        private readonly IPostService _postService;

        public RemoteAccountService(IPostService postService)
        {
            _postService = postService;
        }

        public void CreateNewUserInProject(UserModel user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"activationToken", user.ActivationToken},
                {"login", user.Login},
                {"password", user.Password},
                {"firstName", user.FirstName},
                {"lastName", user.LastName},
                {"projectGuid", Configuration.Guid}
            };

            string authServerAnswer;
            JsonObject parsed;

            try
            {
                authServerAnswer = _postService.SendAndGetAnswer(Configuration.CreateNewUserInProjectAdress, parameters);
            }
            catch
            {
                throw new ServerConnectionException();
            }

            try
            {
                parsed = JsonConvert.DeserializeObject<JsonObject>(authServerAnswer);
                if (parsed == null)
                    throw new Exception();
            }
            catch
            {
                throw new ServerNotValidAnswerException();
            }

            switch (parsed.Result)
            {
                case JsonValues.PasswordLengthIncorrect:
                    throw new PasswordLengthIncorrectException();
                case JsonValues.RequiredCharactersViolation:
                    throw new RequiredCharactersViolationException();
                case JsonValues.PasswordTooCommon:
                    throw new PasswordTooCommonException();
                case JsonValues.UserCreatedInProject:
                    break;
                default:
                    throw new UserNotCreatedInProjectException();
            }
        }

        public void PasswordChangeForUser(ChangingPasswordUser user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"userEmail", user.Email },
                {"password", user.Password},
                {"newPassword", user.NewPassword},
                {"confirmedNewPassword", user.ConfirmNewPassword}
            };

            string authServerAnswer;
            JsonObject parsed;

            try
            {
                authServerAnswer = _postService.SendAndGetAnswer(Configuration.ChangingPasswordUserAddress, parameters);
            }
            catch (Exception)
            {
                throw new ServerConnectionException();
            }

            try
            {
                parsed = JsonConvert.DeserializeObject<JsonObject>(authServerAnswer);
                if (parsed == null)
                    throw new Exception();
            }
            catch
            {
                throw new ServerNotValidAnswerException();
            }

            switch (parsed.Result)
            {
                case JsonValues.RequiredCharactersViolation:
                    throw new RequiredCharactersViolationException();
                case JsonValues.PasswordLengthIncorrect:
                    throw new PasswordLengthIncorrectException();
                case JsonValues.PasswordTooCommon:
                    throw new PasswordTooCommonException();
                case JsonValues.UsersPasswordChanged:
                    break;
                default:
                    throw new PasswordNotChangedException();
            }
        }

        public void SetPasswordForUser(string token, string password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"token", token },
                {"password", password}
            };

            string authServerAnswer;
            JsonObject parsed;

            try
            {
                authServerAnswer = _postService.SendAndGetAnswer(Configuration.SetPasswordForUserAdress, parameters);
            }
            catch (Exception)
            {
                throw new ServerConnectionException();
            }

            try
            {
                parsed = JsonConvert.DeserializeObject<JsonObject>(authServerAnswer);
                if (parsed == null)
                    throw new Exception();
            }
            catch
            {
                throw new ServerNotValidAnswerException();
            }

            switch (parsed.Result)
            {
                case JsonValues.RequiredCharactersViolation:
                    throw new RequiredCharactersViolationException();
                case JsonValues.PasswordLengthIncorrect:
                    throw new PasswordLengthIncorrectException();
                case JsonValues.PasswordTooCommon:
                    throw new PasswordTooCommonException();
                case JsonValues.PasswordSet:
                    break;
                default:
                    throw new PasswordNotSetException();
            }
        }
    }
}
