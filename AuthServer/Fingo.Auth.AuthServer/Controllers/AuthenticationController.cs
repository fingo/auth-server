using System;
using Fingo.Auth.AuthServer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.JsonWrapper;

namespace Fingo.Auth.AuthServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        private ILogger<AuthenticationController> _logger;
        private ITokenService _service;

        public AuthenticationController(ILogger<AuthenticationController> logger, ITokenService service, IEventBus eventBus,
            IEventWatcher eventWatcher) : base(eventBus, eventWatcher)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("AcquireToken")]
        public string AcquireToken(string login, string password, Guid projectGuid)
        {
            _logger.Log(LogLevel.Information, $"api/authentication/acquiretoken called with login={login}, projectGuid={projectGuid}");
            string acquireTokenResultJson = _service.AcquireToken(login, password, projectGuid);
            _logger.Log(LogLevel.Information, $"api/authentication/acquiretoken returned: {acquireTokenResultJson}");

            if (acquireTokenResultJson == new JsonObject {Result = JsonValues.PasswordExpired}.ToJson())
                Response.Redirect(RedirectConfiguration.PasswordExpiredRedirectAdress);

            Response.ContentType = "application/json; charset=utf-8";
            return acquireTokenResultJson;
        }

        [HttpPost("VerifyToken")]
        public string VerifyToken(string jwt, Guid projectGuid)
        {
            _logger.Log(LogLevel.Information, $"api/authentication/verifytoken called with jwt={jwt}, projectGuid={projectGuid}");
            string verifyTokenResultJson = _service.VerifyToken(jwt, projectGuid);
            _logger.Log(LogLevel.Information, $"api/authentication/verifytoken returned: {verifyTokenResultJson}");

            Response.ContentType = "application/json; charset=utf-8";
            return verifyTokenResultJson;
        }
    }
}