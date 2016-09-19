using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.ManagementApp.Alerts;
using Fingo.Auth.ManagementApp.Configuration;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    [Route("users")]
    public class UsersController : BaseController
    {
        private readonly IGetProjectFactory getProjectFactory;
        private readonly IGetUserFactory _getUserFactory;
        private readonly IGetAllUserFactory getAllUserFactory;
        private readonly ICsvService csvService;
        private readonly IAddImportedUsersFactory addImportedUsersFactory;
        private readonly IMessageSender messageSender;

        public UsersController(IGetProjectFactory getProjectFactory , IGetAllUserFactory getAllUserFactory ,
            IEventWatcher eventWatcher , IGetUserFactory getUserFactory , IAddImportedUsersFactory addImportedUsersFactory,
            ICsvService csvService, IEventBus eventBus , IMessageSender messageSender,
            ILogger<UsersController> logger) : base(eventWatcher , eventBus)
        {
            this.getProjectFactory = getProjectFactory;
            _getUserFactory = getUserFactory;
            this.getAllUserFactory = getAllUserFactory;
            this.csvService = csvService;
            this.addImportedUsersFactory= addImportedUsersFactory;
            this.messageSender = messageSender;

            eventBus.SubscribeAll(m => logger.Log(LogLevel.Information , $"Event message:{m.ToString()}"));
        }

        [HttpGet]
        public IActionResult All()
        {
            var users = getAllUserFactory.Create().Invoke();
            ViewBag.UsersCount = users.Count() / 10;
            return View(users);
        }


        [HttpGet("getByLogin")]
        public IActionResult GetByLogin(string login)
        {
           var userID = getAllUserFactory.Create().Invoke().FirstOrDefault(x => x.Login == login).Id;
           return RedirectToAction("GetById", new { id = userID });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _getUserFactory.Create().Invoke(id);
                return View("UserDetails" , user);
            }
            catch (Exception)
            {
                Alert(AlertType.Warning , "Cannot load data.");
                return View("ErrorPage");
            }
        }

        [HttpPost]
        [Route("import/{projectId}")]
        public IActionResult Import(IFormFile file, int projectId)
        {
            try
            {
                var usersAdded = 0;
                var usersDuplicated = 0;
                var importedUsers = csvService.CsvToUsersList(file);
                var userAddedEmails = new List<Tuple<string,string>>();
                var projectName = getProjectFactory.Create().Invoke(projectId).Name;

                addImportedUsersFactory.Create().Invoke(importedUsers, projectId, ref usersAdded, ref usersDuplicated, ref userAddedEmails);

                var content = $"\na new account has been created for you in {projectName} project.\n";

                foreach (var userAddedEmail in userAddedEmails)
                    SendMessageAboutImportUser(projectName, content, userAddedEmail.Item1, userAddedEmail.Item2);

                if (usersAdded == importedUsers.Count)
                    Alert(AlertType.Success, $"All loaded users ({usersAdded}) were added correctly.");
                else
                    Alert(AlertType.Information, $"Successfully added {usersAdded} users. {usersDuplicated} users were not added " +
                                                 $"because of being a duplicate, {importedUsers.Count - usersAdded - usersDuplicated} " +
                                                 $"were not added because of having not valid e-mail adress.");

                return RedirectToAction("GetById", "Projects", new { id = projectId });
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning, $"Something went wrong ({ex.Message}).");
                return RedirectToAction("GetById", "Projects", new { id = projectId });
            }
        }


        [HttpGet("getAllProjectsFormUser")]
        public IActionResult GetProjectsFromUser(int id, int pageSize = 10, int page = 1)
        {
            ViewBag.RowsPerPage = pageSize;
            ViewBag.Number = pageSize * page - pageSize;
            try
            {
                var projects = _getUserFactory.Create().Invoke(id).Projects.AsEnumerable();

                ViewBag.Id = id;
                ViewBag.UsersCount = projects.Count() / pageSize;
                ViewBag.Page = page;
                ViewBag.TotalRows = projects.Count();

                projects = projects.OrderBy(m => m.Id).Skip(pageSize * (page - 1)).Take(pageSize);


                return PartialView("Partial/AllProjectFromUser", projects);
            }
            catch
            {
                return PartialView("ErrorPartialView");
            }
        }


        [HttpGet("UsersWithCollapseProject")]
        public IActionResult UsersWithCollapseProjectParialView(string firstName, string lastName, string login, int pageSize = 10, int page = 1)
        {
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.Login = login;
            ViewBag.RowsPerPage = pageSize;
            ViewBag.Number = pageSize * page - pageSize;

            var users = getAllUserFactory.Create().Invoke();

            users = FilterUsers(firstName, lastName, login, users);

            ViewBag.UsersCount = users.Count() / pageSize;
            ViewBag.Page = page;
            ViewBag.TotalRows = users.Count();

            users = users.OrderBy(m => m.Id).Skip(pageSize * (page - 1)).Take(pageSize);

            return PartialView("Partial/UsersTableWithCollapseProjectPartial", users);
        }

        private IEnumerable<BaseUserModel> FilterUsers(string firstName, string lastName, string login, IEnumerable<BaseUserModel> users)
        {
            if (!string.IsNullOrEmpty(firstName))
                users = users.Where(m => m.FirstName.ToLower().Contains(firstName.ToLower()));
            if (!string.IsNullOrEmpty(lastName))
                users = users.Where(m => m.LastName.ToLower().Contains(lastName.ToLower()));
            if (!string.IsNullOrEmpty(login))
                users = users.Where(m => m.Login.ToLower().Contains(login.ToLower()));
            return users;
        }

        private void SendMessageAboutImportUser(string projectName, string content, string email, string activationToken)
        {
            var emailContent = messageSender.CreateContent(content+EmailConfiguration.ContentSetPassword
                    + EmailConfiguration.SetPassword + activationToken, EmailConfiguration.Greeting, EmailConfiguration.Sender);

            var message = messageSender.CreateMessage(projectName, email, EmailConfiguration.Sender,
               EmailConfiguration.SenderEmail, EmailConfiguration.NewAccountCreated, emailContent);
            messageSender.SendEmail(message, EmailConfiguration.ServerEmail, EmailConfiguration.ServerPassword, EmailConfiguration.ServerName, 465);
        }
    }
}
