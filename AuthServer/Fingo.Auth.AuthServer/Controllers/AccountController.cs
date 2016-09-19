using System;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.AuthServer.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IEventBus eventBus,
            IEventWatcher eventWatcher) : base(eventBus, eventWatcher)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("CreateNewUserInProject")]
        public string CreateNewUserInProject(string activationToken, string login, string password, string firstName, string lastName,
            Guid projectGuid)
        {
            if (lastName == null)
            {
                lastName = string.Empty;
            }
            if (firstName == null)
            {
                firstName = string.Empty;
            }

            _logger.Log(LogLevel.Information, $"api/account/createnewuserinproject called with activationToken={activationToken}," +
                                              $"login={login}, firstName={firstName}, lastName = {lastName}," +
                                              $"projectGuid={projectGuid}");

            var resultJson = _accountService.CreateNewUserInProject(new UserModel
                {
                    ActivationToken = activationToken,
                    Login = login,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                }, projectGuid);

            _logger.Log(LogLevel.Information, $"api/account/createnewuserinproject returned: {resultJson}");

            Response.ContentType = "application/json; charset=utf-8";
            return resultJson;
        }

        [HttpPost("PasswordChangeForUser")]
        public string PasswordChangeForUser(string userEmail, string password, string newPassword,
            string confirmedNewPassword)
        {
            _logger.Log(LogLevel.Information, $"api/account/passwordchangeforuser called with user email={userEmail}");

            var resultJson = _accountService.PasswordChangeForUser(new ChangingPasswordUser
            {
                Password = password,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmedNewPassword,
                Email = userEmail
            });

            _logger.Log(LogLevel.Information, $"api/account/passwordchangeforuser returned: {resultJson}");

            Response.ContentType = "application/json; charset=utf-8";
            return resultJson;
        }

        [HttpPost("SetPasswordForUser")]
        public string SetPasswordForUser(string token, string password)
        {
            _logger.Log(LogLevel.Information, $"api/account/setpasswordforuser called with user token={token}");

            var resultJson = _accountService.SetPasswordForUser(token, password);

            _logger.Log(LogLevel.Information, $"api/account/setpasswordforuser returned: {resultJson}");

            Response.ContentType = "application/json; charset=utf-8";
            return resultJson;
        }
    }
}