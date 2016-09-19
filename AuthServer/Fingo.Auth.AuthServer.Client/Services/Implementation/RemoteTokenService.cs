using System;
using System.Collections.Generic;
using Fingo.Auth.AuthServer.Client.Exceptions;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.JsonWrapper;
using Newtonsoft.Json;

namespace Fingo.Auth.AuthServer.Client.Services.Implementation
{
    public class RemoteTokenService : IRemoteTokenService
    {
        private readonly IPostService _postService;
        private readonly ILogger<RemoteTokenService> _logger;

        public RemoteTokenService(IPostService postService)
        {
            _postService = postService;
            _logger = new Logger<RemoteTokenService>();
        }

        public bool VerifyToken(string jwt)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"jwt", jwt},
                {"projectGuid", Configuration.Guid}
            };

            JsonObject parsed;
            try
            {
                string authServerAnswer = _postService.SendAndGetAnswer(Configuration.VerifyTokenAdress, parameters);
                parsed = JsonConvert.DeserializeObject<JsonObject>(authServerAnswer);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error,
                    $"<VerifyToken> _postService.SendAndGetAnswer({Configuration.VerifyTokenAdress}, parameters) threw a exception: {e.Message}, stacktrace: {e.StackTrace}");
                return false;
            }

            return parsed != null && parsed.Result == JsonValues.TokenValid;
        }

        public string AcquireToken(string login, string password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"login", login},
                {"password", password},
                {"projectGuid", Configuration.Guid}
            };

            string authServerAnswer;
            JsonObject parsed;

            try
            {
                authServerAnswer = _postService.SendAndGetAnswer(Configuration.AcquireTokenAdress, parameters);
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

            if (parsed.Result == JsonValues.NotAuthenticated)
                throw new NotAuthenticatedException();

            if (parsed.Result == JsonValues.AccountExpired)
                throw new AccountExpiredException();

            if (parsed.Result == JsonValues.PasswordExpired)
                throw new PasswordExpiredException();

            if (parsed.Result == JsonValues.Authenticated)
                return parsed.Jwt;

            throw new Exception();
        }
    }
}