using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using System;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.ManagementApp.Configuration;
using System.ComponentModel.DataAnnotations;
using Fingo.Auth.AuthServer.Client.Exceptions;
using Fingo.Auth.ManagementApp.Alerts;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    public class AccountController : BaseController
    {
        private ILogger<AccountController> _logger;
        private IRemoteTokenService _remoteTokenService;
        private ISessionService _sessionService;
        private IGetUserByLoginFactory _getUserByLoginFactory;
        private IGetUserByActivationTokenFactory _getUserByActivationTokenFactory;
        private IActivateByActivationTokenFactory _activateByActivationTokenFactory;
        private IMessageSender _messageSender;
        private IRemoteAccountService _remoteAccountService;

        public AccountController(IEventWatcher eventWatcher, IEventBus eventBus, ILogger<AccountController> logger,
            ISessionService sessionService, IRemoteTokenService remoteTokenService,
            IGetUserByLoginFactory getUserByLoginFactory, IMessageSender messageSender,
            IGetUserByActivationTokenFactory getUserByActivationTokenFactory,
            IActivateByActivationTokenFactory activateByActivationTokenFactory,
            IRemoteAccountService remoteAccountService) : base(eventWatcher, eventBus)
        {
            _logger = logger;
            _sessionService = sessionService;
            _remoteTokenService = remoteTokenService;
            _getUserByLoginFactory = getUserByLoginFactory;
            _getUserByActivationTokenFactory = getUserByActivationTokenFactory;
            _activateByActivationTokenFactory = activateByActivationTokenFactory;
            _messageSender = messageSender;
            _remoteAccountService = remoteAccountService;
            eventBus.SubscribeAll(m => logger.Log(LogLevel.Information, $"Event message:{m.ToString()}"));
        }

        [Route("SignUp")]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            _sessionService.LogOut(HttpContext);

            if (_sessionService.IsLoggedIn(User))
            {
                Alert(AlertType.Information, "You were logged out. Please try to sign up again.");
                return RedirectToAction("LoginPage", "Account");
            }
            return View("SignUp");
        }

        [Route("AddUser")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddUser(UserModel user)
        {
            if (_sessionService.IsLoggedIn(User))
            {
                _sessionService.LogOut(HttpContext);
                Alert(AlertType.Information, "You were logged out. Please sign up again.");
                return RedirectToAction("LoginPage", "Account");
            }
            if (user.Password == null || user.Login == null || user.ConfirmedPassword == null)
            {
                Alert(AlertType.Warning, "E-mail and passwords are required to create an account.");
                return View("SignUp");
            }
            if (user.Password != user.ConfirmedPassword)
            {
                Alert(AlertType.Warning, "Password must be the same as confirmed password.");
                return View("SignUp");
            }

            var attribute = new EmailAddressAttribute();
            if (!attribute.IsValid(user.Login))
            {
                Alert(AlertType.Warning, "E-mail address is not valid. Please enter the correct one.");
                return View("SignUp");
            }

            UserModel currentUser = _getUserByLoginFactory.Create().Invoke(user.Login);

            if (currentUser != null && currentUser.UserStatus != UserStatus.Deleted)
            {
                Alert(AlertType.Warning, "Such login already exists in the database.");
                return View("SignUp");
            }

            user.ActivationToken = Guid.NewGuid().ToString().Replace("-", string.Empty);


            try
            {
                _remoteAccountService.CreateNewUserInProject(user);
            }
            catch (ServerConnectionException)
            {
                Alert(AlertType.Warning, ServerConnectionException.Message);
                return View("SignUp");
            }
            catch (ServerNotValidAnswerException)
            {
                Alert(AlertType.Warning, ServerNotValidAnswerException.Message);
                return View("SignUp");
            }
            catch (UserNotCreatedInProjectException)
            {
                Alert(AlertType.Warning, UserNotCreatedInProjectException.Message);
                return View("SignUp");
            }
            catch (RequiredCharactersViolationException)
            {
                Alert(AlertType.Warning, "Password must contain at least one digit.");
                return View("SignUp");
            }
            catch (PasswordLengthIncorrectException)
            {
                Alert(AlertType.Warning, "Password must contain at least 5 characters.");
                return View("SignUp");
            }
            catch (PasswordTooCommonException)
            {
                Alert(AlertType.Warning, PasswordTooCommonException.Message);
                return View("SignUp");
            }

            try
            {
                SendMessage(user.ActivationToken, user.FirstName, user.LastName, user.Login);
            }
            catch (Exception)
            {
                Alert(AlertType.Warning, "Something went wrong. Your e-mail with activation link was not sent.");
                return View("SignUp");
            }

            Alert(AlertType.Information, "Your account has been created. Please confirm your e-mail address to log in.");
            return RedirectToAction("LoginPage");
        }

        [AllowAnonymous]
        public IActionResult LoginPage()
        {
            if (!_sessionService.IsLoggedIn(User))
                return View();

            return RedirectToAction("All", "Projects");
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginPage(UserModel user)
        {
            if (user.Password == null || user.Login == null)
            {
                Alert(AlertType.Warning, "E-mail and password are required to log in.");
                return View("LoginPage");
            }

            string jwt;

            try
            {
                jwt = _remoteTokenService.AcquireToken(user.Login, user.Password);
            }
            catch (ServerConnectionException)
            {
                Alert(AlertType.Warning, ServerConnectionException.Message);
                return View("LoginPage");
            }
            catch (NotAuthenticatedException)
            {
                Alert(AlertType.Warning, NotAuthenticatedException.Message);
                return View("LoginPage");
            }
            catch (ServerNotValidAnswerException)
            {
                Alert(AlertType.Warning, ServerNotValidAnswerException.Message);
                return View("LoginPage");
            }
            catch (AccountExpiredException)
            {
                Alert(AlertType.Warning, AccountExpiredException.Message);
                return RedirectToAction("LoginPage", "Account");
            }
            catch (PasswordExpiredException)
            {
                Alert(AlertType.Warning, PasswordExpiredException.Message);
                ViewBag.UserName = user.Login;
                return View("ChangePassword");
            }
            catch (Exception)
            {
                Alert(AlertType.Warning, "Some internal error occured.");
                return View("LoginPage");
            }

            _logger.Log(LogLevel.Information, "User " + user.Login + " logged in");

            try
            {
                user.Id = _getUserByLoginFactory.Create().Invoke(user.Login).Id;
            }
            catch (Exception)
            {
                //Exception
            }

            _sessionService.LogIn(user, jwt, HttpContext);
            return RedirectToAction("All", "Projects");
        }


        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword(string login)
        {
            if (login != User.Identity.Name)
            {
                return RedirectToAction("LoginPage");
            }

            ViewBag.UserName = login;

            return View("ChangePassword");
        }


        [AllowAnonymous]
        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangingPasswordUser user, string login)
        {
            if (user.NewPassword != user.ConfirmNewPassword)
            {
                Alert(AlertType.Warning, "New password must be the same as confirmed password.");
                ViewBag.userName = login;
                return View("ChangePassword");
            }

            if (user.Password == null || user.NewPassword == null || user.ConfirmNewPassword == null)
            {
                Alert(AlertType.Warning, "You have to fill each field.");
                ViewBag.UserName = login;
                return View("ChangePassword");
            }

            user.Email = login;

            try
            {
                _remoteAccountService.PasswordChangeForUser(user);
            }
            catch (ServerConnectionException)
            {
                Alert(AlertType.Warning, ServerConnectionException.Message);
                ViewBag.UserName = login;
                return View("ChangePassword");
            }
            catch (ServerNotValidAnswerException)
            {
                Alert(AlertType.Warning, ServerNotValidAnswerException.Message);
                ViewBag.UserName = login;
                return View("ChangePassword");
            }
            catch (PasswordNotChangedException)
            {
                Alert(AlertType.Warning, PasswordNotChangedException.Message);
                ViewBag.UserName = login;
                return View("ChangePassword");
            }
            catch (PasswordLengthIncorrectException)
            {
                Alert(AlertType.Warning, PasswordLengthIncorrectException.Message);

                ViewBag.UserName = login;
                return View("ChangePassword");
            }
            catch (RequiredCharactersViolationException)
            {
                Alert(AlertType.Warning, RequiredCharactersViolationException.Message);
                ViewBag.UserName = login;
                return View("ChangePassword");
            }
            catch (PasswordTooCommonException)
            {
                Alert(AlertType.Warning, PasswordTooCommonException.Message);

                ViewBag.UserName = login;
                return View("ChangePassword");
            }

            _sessionService.LogOut(HttpContext);
            Alert(AlertType.Information, "Please log in with new password.");
            return RedirectToAction("LoginPage");
        }

        [Route("SignOut")]
        [AllowAnonymous]
        public IActionResult SignOut()
        {
            _sessionService.LogOut(HttpContext);

            return RedirectToAction("LoginPage", "Account");
        }

        [AllowAnonymous]
        [Route("setpassword/{token}")]
        [HttpGet]
        public IActionResult SetPassword(string token)
        {
            var user = _getUserByActivationTokenFactory.Create().Invoke(token);
            if (user == null || user.UserStatus != UserStatus.AccountCreated)
                return RedirectToAction("LoginPage");

            return View("SetPassword", new SettingPasswordUser { Token = token });
        }

        [AllowAnonymous]
        [Route("setpassword/{token}")]
        [HttpPost]
        public IActionResult SetPassword(SettingPasswordUser user)
        {
            if (user.Password == null || user.ConfirmPassword == null)
            {
                Alert(AlertType.Warning, "You have to fill each field.");
                return View("SetPassword");
            }

            if (user.Password != user.ConfirmPassword)
            {
                Alert(AlertType.Warning, "Passwords must match.");
                return View("SetPassword");
            }

            try
            {
                _remoteAccountService.SetPasswordForUser(user.Token, user.Password);
            }
            catch (ServerConnectionException)
            {
                Alert(AlertType.Warning, ServerConnectionException.Message);
                return View("SetPassword");
            }
            catch (ServerNotValidAnswerException)
            {
                Alert(AlertType.Warning, ServerNotValidAnswerException.Message);
                return View("SetPassword");
            }
            catch (PasswordNotChangedException)
            {
                Alert(AlertType.Warning, PasswordNotChangedException.Message);
                return View("SetPassword");
            }
            catch (PasswordLengthIncorrectException)
            {
                Alert(AlertType.Warning, PasswordLengthIncorrectException.Message);
                return View("SetPassword");
            }
            catch (RequiredCharactersViolationException)
            {
                Alert(AlertType.Warning, RequiredCharactersViolationException.Message);
                return View("SetPassword");
            }
            catch (PasswordTooCommonException)
            {
                Alert(AlertType.Warning, PasswordTooCommonException.Message);
                return View("SetPassword");
            }
            catch (PasswordNotSetException)
            {
                Alert(AlertType.Warning, PasswordNotSetException.Message);
                return View("SetPassword");
            }

            _sessionService.LogOut(HttpContext);
            Alert(AlertType.Information, "Please log in with new password.");
            return RedirectToAction("LoginPage");
        }

        [AllowAnonymous]
        [Route("activate/{activationToken}")]
        [HttpGet]
        public IActionResult AccountActivation(string activationToken)
        {
            UserModel user = null;
            try
            {
                user = _getUserByActivationTokenFactory.Create().Invoke(activationToken);
            }
            catch (Exception)
            {
                // ignored
            }

            if (user != null && user.UserStatus == UserStatus.Registered)
            {
                _activateByActivationTokenFactory.Create().Invoke(activationToken);
                ViewBag.ActivatedSuccessfully = true;
            }
            else
                ViewBag.ActivatedSuccessfully = false;

            _sessionService.LogOut(HttpContext);
            return View("AccountActivation");
        }

        private void SendMessage(string activationToken, string firstName, string lastName, string email)
        {
            var emailContent = _messageSender.CreateContent(EmailConfiguration.Content
                    + EmailConfiguration.ActivateEmail + activationToken, EmailConfiguration.Greeting, EmailConfiguration.Sender);

            var message = _messageSender.CreateMessage(firstName + lastName, email, EmailConfiguration.Sender,
               EmailConfiguration.SenderEmail, EmailConfiguration.EmailTitle, emailContent);
            _messageSender.SendEmail(message, EmailConfiguration.ServerEmail, EmailConfiguration.ServerPassword, EmailConfiguration.ServerName, 465);
        }
    }
}