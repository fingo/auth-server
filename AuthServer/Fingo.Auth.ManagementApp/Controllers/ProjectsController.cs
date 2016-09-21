using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.ManagementApp.Alerts;
using Fingo.Auth.ManagementApp.Configuration;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    [Route("projects")]
    public class ProjectsController : BaseController
    {
        private readonly IAddProjectFactory _addProjectFactory;
        private readonly IAssignUserFactory _assignUserFactory;
        private readonly ICsvService _csvGenerator;
        private readonly IDeleteByIdUserFactory _deleteByIdUserFactory;
        private readonly IDeleteProjectFactory _deleteProjectFactory;
        private readonly IGetAllProjectFactory _getAllProjectFactory;
        private readonly IGetProjectFactory _getProjectFactory;
        private readonly IGetUserFactory _getUserFactory;
        private readonly IMessageSender _messageSender;
        private readonly IGetProjectWithAllFactory getProjectWithAllFactory;


        public ProjectsController(IEventWatcher eventWatcher , ILogger<ProjectsController> logger , IEventBus eventBus ,
            IDeleteByIdUserFactory deleteByIdUserFactory , IGetProjectFactory getProjectFactory ,
            IDeleteProjectFactory deleteProjectFactory , IAddProjectFactory addProjectFactory ,
            IGetAllProjectFactory getAllProjectFactory , IAssignUserFactory assignUserFactory ,
            IMessageSender messageSender , IGetUserFactory getUserFactory ,
            IGetProjectWithAllFactory getProjectWithAllFactory , ICsvService csvGenerator)
            : base(eventWatcher , eventBus)
        {
            _csvGenerator = csvGenerator;
            _assignUserFactory = assignUserFactory;
            _deleteByIdUserFactory = deleteByIdUserFactory;
            _getProjectFactory = getProjectFactory;
            _getAllProjectFactory = getAllProjectFactory;
            _deleteProjectFactory = deleteProjectFactory;
            _addProjectFactory = addProjectFactory;
            _messageSender = messageSender;
            _getUserFactory = getUserFactory;
            this.getProjectWithAllFactory = getProjectWithAllFactory;

            eventBus.SubscribeAll(m => logger.Log(LogLevel.Information , $"Event message:{m.ToString()}"));
        }

        [HttpGet]
        public IActionResult All()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var model = getProjectWithAllFactory.Create().Invoke(id);
                return View("ProjectDetails" , model);
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpGet("export/{projectId}")]
        public FileResult Export(int projectId)
        {
            Response.Headers.Add("Content-disposition" , $"attachment; filename=project{projectId}.csv");

            try
            {
                var csvFile = _csvGenerator.ExportProject(projectId);
                return File(Encoding.UTF8.GetBytes(csvFile) , "text/csv");
            }
            catch (Exception e)
            {
                return File(Encoding.UTF8.GetBytes($"Error\r\nSomething went wrong: {e.Message}.") , "text/csv");
            }
        }

        [HttpPost("{projectId}")]
        public IActionResult AsignUsersToProject(int projectId , int[] usersId)
        {
            try
            {
                var succesAssignAllUsers = true;
                var projectName = _getProjectFactory.Create().Invoke(projectId).Name;
                foreach (var userId in usersId)
                    try
                    {
                        _assignUserFactory.Create().Invoke(projectId , userId);
                        try
                        {
                            SendMessageAboutAssignment(projectName ,
                                $"your account was assigned to a new project: {projectName}." ,
                                _getUserFactory.Create().Invoke(userId).Login);
                        }
                        catch (Exception)
                        {
                            //ignored
                        }
                    }
                    catch (Exception)
                    {
                        succesAssignAllUsers = false;
                    }
                if (succesAssignAllUsers)
                    Alert(AlertType.Success , "Users were assigned correctly.");
                else
                    Alert(AlertType.Warning , "Some users were not assigned because of an error.");
                return RedirectToAction("GetById" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpDelete("{projectId}/{userId}")]
        public HttpResponseMessage DeleteUserFromProject(int projectId , int userId)
        {
            try
            {
                _deleteByIdUserFactory.Create().Invoke(projectId , userId);

                try
                {
                    var user = _getUserFactory.Create().Invoke(userId);
                    SendMessageAboutUnassignment(user.FirstName + user.LastName ,
                        $"your account was unassigned from project: {_getProjectFactory.Create().Invoke(projectId).Name}." ,
                        user.Login);
                }
                catch (Exception)
                {
                    //ignored
                }

                Alert(AlertType.Success , "User was correctly removed from the project.");
            }
            catch (ArgumentNullException)
            {
                Alert(AlertType.Warning , "Could not find such user in the database.");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                Alert(AlertType.Warning , "User wasn't removed from the project.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public HttpResponseMessage Add(string name)
        {
            try
            {
                var project = new ProjectModel();
                project.Name = name;
                _addProjectFactory.Create().Invoke(project);

                Alert(AlertType.Success , "Project was added correctly.");
            }
            catch (Exception)
            {
                Alert(AlertType.Warning , "Project was not added.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpGet("userInProject")]
        public IActionResult UserInProject(int id , int pageSize = 10 , int page = 1)
        {
            try
            {
                ViewBag.Id = id;
                ViewBag.RowsPerPage = pageSize;
                ViewBag.Number = pageSize * page - pageSize;
                var users = _getProjectFactory.Create().Invoke(id).Users.AsEnumerable();
                ViewBag.UsersCount = users.Count() / pageSize;
                ViewBag.Page = page;
                ViewBag.TotalRows = users.Count();

                users = users.OrderBy(m => m.Id).Skip(pageSize * (page - 1)).Take(pageSize);

                return PartialView("Partial/UserInProjectPartialView" , users);
            }
            catch
            {
                return PartialView("ErrorPartialView");
            }
        }

        [HttpGet("getAllProjects")]
        public IActionResult GetAllProjects(int pageSize = 10 , int page = 1)
        {
            try
            {
                ViewBag.RowsPerPage = pageSize;
                var projects = _getAllProjectFactory.Create().Invoke().AsEnumerable();
                ViewBag.UsersCount = projects.Count() / pageSize;
                ViewBag.Page = page;
                ViewBag.TotalRows = projects.Count();
                projects = projects.OrderBy(m => m.Id).Skip(pageSize * (page - 1)).Take(pageSize);
                return PartialView("Partial/AllProjectsPartialView" , projects);
            }

            catch
            {
                return PartialView("ErrorPartialView");
            }
        }

        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _deleteProjectFactory.Create().Invoke(id);

                Alert(AlertType.Success , "Project was removed correctly.");
            }
            catch (ArgumentNullException)
            {
                Alert(AlertType.Information , "Project doesn't exist in our database.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                Alert(AlertType.Warning , "Project was not removed.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        private void SendMessageAboutAssignment(string projectName , string content , string email)
        {
            var emailContent = _messageSender.CreateContent(content , EmailConfiguration.Greeting ,
                EmailConfiguration.Sender);

            var message = _messageSender.CreateMessage(projectName , email , EmailConfiguration.Sender ,
                EmailConfiguration.SenderEmail , "Your account was assigned to a new project" , emailContent);
            _messageSender.SendEmail(message , EmailConfiguration.ServerEmail , EmailConfiguration.ServerPassword ,
                EmailConfiguration.ServerName , 465);
        }

        private void SendMessageAboutUnassignment(string userName , string content , string email)
        {
            var emailContent = _messageSender.CreateContent(content , EmailConfiguration.Greeting ,
                EmailConfiguration.Sender);

            var message = _messageSender.CreateMessage(userName , email , EmailConfiguration.Sender ,
                EmailConfiguration.SenderEmail , "Your account was unassigned from a project" , emailContent);
            _messageSender.SendEmail(message , EmailConfiguration.ServerEmail , EmailConfiguration.ServerPassword ,
                EmailConfiguration.ServerName , 465);
        }
    }
}