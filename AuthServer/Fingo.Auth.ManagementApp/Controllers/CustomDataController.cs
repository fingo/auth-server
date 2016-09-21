using System;
using System.Net;
using System.Net.Http;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.UserView;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.ManagementApp.Alerts;
using Fingo.Auth.ManagementApp.Configuration;
using Fingo.Auth.ManagementApp.Models.Enums;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Route("customData")]
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    public class CustomDataController : BaseController
    {
        private readonly IAddProjectCustomDataToProjectFactory addProjectCustomDataToProjectFactory;
        private readonly IDeleteCustomDataFromProjectFactory deleteCustomDataFromProjectFactory;
        private readonly IEditProjectCustomDataToProjectFactory editProjectCustomDataToProjectFactory;

        private readonly IGetCustomDataConfigurationOrDefaultForProjectFactory
            getCustomDataConfigurationOrDefaultForProjectFactory;

        private readonly IGetUserCustomDataConfigurationViewFactory getUserCustomDataConfigurationViewFactory;
        private readonly IGetUserFactory getUserFactory;
        private readonly IMessageSender messageSender;
        private readonly ISaveUserCustomDataFactory saveUserCustomDataFactory;

        public CustomDataController(IGetUserFactory getUserFactory , IEventWatcher eventWatcher , IEventBus eventBus ,
            IDeleteCustomDataFromProjectFactory deleteCustomDataFromProjectFactory ,
            IGetCustomDataConfigurationOrDefaultForProjectFactory getCustomDataConfigurationOrDefaultForProjectFactory ,
            IAddProjectCustomDataToProjectFactory addProjectCustomDataToProjectFactory ,
            IEditProjectCustomDataToProjectFactory editProjectCustomDataToProjectFactory ,
            ISaveUserCustomDataFactory saveUserCustomDataFactory ,
            IGetUserCustomDataConfigurationViewFactory getUserCustomDataConfigurationViewFactory ,
            IMessageSender messageSender)
            : base(eventWatcher , eventBus)
        {
            this.deleteCustomDataFromProjectFactory = deleteCustomDataFromProjectFactory;
            this.getCustomDataConfigurationOrDefaultForProjectFactory =
                getCustomDataConfigurationOrDefaultForProjectFactory;
            this.addProjectCustomDataToProjectFactory = addProjectCustomDataToProjectFactory;
            this.editProjectCustomDataToProjectFactory = editProjectCustomDataToProjectFactory;
            this.saveUserCustomDataFactory = saveUserCustomDataFactory;
            this.getUserCustomDataConfigurationViewFactory = getUserCustomDataConfigurationViewFactory;
            this.messageSender = messageSender;
            this.getUserFactory = getUserFactory;
        }

        [HttpGet("getPartialView")]
        public IActionResult GetProjectParialViewResult(ConfigurationType configurationType , int projectId ,
            Crud crudOption , string configurationName = null)
        {
            try
            {
                ViewBag.ProjectId = projectId;
                ViewBag.ConfigurationName = configurationName;
                ViewBag.CrudOption = crudOption;
                switch (configurationType)
                {
                    case ConfigurationType.Boolean:
                        return PartialView("Project/BooleanCustomData" ,
                            (BooleanProjectConfiguration)
                            getCustomDataConfigurationOrDefaultForProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Boolean));
                    case ConfigurationType.Number:
                        return PartialView("Project/NumberCustomData" ,
                            (NumberProjectConfiguration)
                            getCustomDataConfigurationOrDefaultForProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Number));
                    case ConfigurationType.Text:
                        return PartialView("Project/TextCustomData" ,
                            (TextProjectConfiguration)
                            getCustomDataConfigurationOrDefaultForProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Text));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(configurationType) , configurationType , null);
                }
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return StatusCode(400);
            }
        }

        [HttpGet("getUserPartialView")]
        public IActionResult GetUserParialViewResult(ConfigurationType configurationType , string configurationName ,
            int projectId , int userId)
        {
            try
            {
                switch (configurationType)
                {
                    case ConfigurationType.Boolean:
                        return PartialView("User/BooleanCustomData" ,
                            (BooleanUserConfigurationView)
                            getUserCustomDataConfigurationViewFactory.Create()
                                .Invoke(projectId , userId , configurationType , configurationName));
                    case ConfigurationType.Number:
                        return PartialView("User/NumberCustomData" ,
                            (NumberUserConfigurationView)
                            getUserCustomDataConfigurationViewFactory.Create()
                                .Invoke(projectId , userId , configurationType , configurationName));
                    case ConfigurationType.Text:
                        return PartialView("User/TextCustomData" ,
                            (TextUserConfigurationView)
                            getUserCustomDataConfigurationViewFactory.Create()
                                .Invoke(projectId , userId , configurationType , configurationName));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(configurationType) , configurationType , null);
                }
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return StatusCode(400);
            }
        }

        [HttpPost("project/Boolean")]
        public IActionResult SaveBooleanCustomData(int projectId ,
            BooleanProjectConfiguration booleanProjectConfiguration , string configurationName ,
            Crud crudOption , string oldConfigurationName)
        {
            try
            {
                SaveConfiguration(projectId , booleanProjectConfiguration , configurationName , crudOption ,
                    ConfigurationType.Boolean , oldConfigurationName);
                Alert(AlertType.Success , "Data saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
        }

        [HttpPost("project/Number")]
        public IActionResult SaveNumberCustomData(int projectId , NumberProjectConfiguration numberProjectConfiguration ,
            string configurationName ,
            Crud crudOption , string oldConfigurationName)
        {
            try
            {
                SaveConfiguration(projectId , numberProjectConfiguration , configurationName , crudOption ,
                    ConfigurationType.Number , oldConfigurationName);
                Alert(AlertType.Success , "Data saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
        }

        [HttpPost("project/Text")]
        public IActionResult SaveTextCustomData(int projectId , TextProjectConfiguration textProjectConfiguration ,
            string configurationName ,
            Crud crudOption , string oldConfigurationName)
        {
            try
            {
                if (textProjectConfiguration.PossibleValues.Contains(null))
                    textProjectConfiguration.PossibleValues.Remove(null);
                SaveConfiguration(projectId , textProjectConfiguration , configurationName , crudOption ,
                    ConfigurationType.Text , oldConfigurationName);
                Alert(AlertType.Success , "Data saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
        }

        [HttpPost("user/Boolean")]
        public IActionResult SaveUserBooleanCustomData(BooleanUserConfigurationView booleanUserConfigurationView)
        {
            try
            {
                saveUserCustomDataFactory.Create()
                    .Invoke(booleanUserConfigurationView.ProjectId , booleanUserConfigurationView.UserId ,
                        booleanUserConfigurationView.ConfigurationName ,
                        new BooleanUserConfiguration {Value = booleanUserConfigurationView.CurrentValue});
                Alert(AlertType.Success , "Data for user saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = booleanUserConfigurationView.ProjectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return RedirectToAction("GetById" , "Projects" , new {id = booleanUserConfigurationView.ProjectId});
            }
        }

        [HttpPost("user/Number")]
        public IActionResult SaveUserNumberCustomData(NumberUserConfigurationView numberUserConfiguration)
        {
            try
            {
                saveUserCustomDataFactory.Create()
                    .Invoke(numberUserConfiguration.ProjectId , numberUserConfiguration.UserId ,
                        numberUserConfiguration.ConfigurationName ,
                        new NumberUserConfiguration {Value = numberUserConfiguration.CurrentValue});
                Alert(AlertType.Success , "Data for user saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = numberUserConfiguration.ProjectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return RedirectToAction("GetById" , "Projects" , new {id = numberUserConfiguration.ProjectId});
            }
        }

        [HttpPost("user/Text")]
        public IActionResult SaveUserTextCustomData(TextUserConfigurationView textUserConfiguration)
        {
            try
            {
                saveUserCustomDataFactory.Create()
                    .Invoke(textUserConfiguration.ProjectId , textUserConfiguration.UserId ,
                        textUserConfiguration.ConfigurationName ,
                        new TextUserConfiguration {Value = textUserConfiguration.CurrentValue});
                Alert(AlertType.Success , "Data for user saved correctly.");

                if (textUserConfiguration.ConfigurationName != "registration_state")
                    return RedirectToAction("GetById" , "Projects" , new {id = textUserConfiguration.ProjectId});

                var user = getUserFactory.Create().Invoke(textUserConfiguration.UserId);
                SendMessage(
                    textUserConfiguration.CurrentValue == "registered"
                        ? EmailConfiguration.GrantRole
                        : EmailConfiguration.RevokeRole , user.FirstName , user.LastName , user.Login);
                return RedirectToAction("GetById" , "Projects" , new {id = textUserConfiguration.ProjectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return RedirectToAction("GetById" , "Projects" , new {id = textUserConfiguration.ProjectId});
            }
        }

        [HttpDelete("{projectId}/{configurationName}")]
        public HttpResponseMessage RemoveCustomData(int projectId , string configurationName)
        {
            try
            {
                deleteCustomDataFromProjectFactory.Create().Invoke(projectId , configurationName);
                Alert(AlertType.Success , "Data was correctly removed from the project.");
            }
            catch (Exception)
            {
                Alert(AlertType.Warning , "Could not find such policy in the database.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        private void SaveConfiguration(int projectId , ProjectConfiguration projectConfiguration ,
            string configurationName ,
            Crud crudOption , ConfigurationType configurationType , string oldConfigurationName)
        {
            switch (crudOption)
            {
                case Crud.Add:
                    switch (configurationType)
                    {
                        case ConfigurationType.Boolean:
                            addProjectCustomDataToProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Boolean , projectConfiguration);
                            break;
                        case ConfigurationType.Number:
                            addProjectCustomDataToProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Number , projectConfiguration);
                            break;
                        case ConfigurationType.Text:
                            addProjectCustomDataToProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Text , projectConfiguration);
                            break;
                    }
                    break;
                case Crud.Edit:
                    switch (configurationType)
                    {
                        case ConfigurationType.Boolean:
                            editProjectCustomDataToProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Boolean , projectConfiguration ,
                                    oldConfigurationName);
                            break;
                        case ConfigurationType.Number:
                            editProjectCustomDataToProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Number , projectConfiguration ,
                                    oldConfigurationName);
                            break;
                        case ConfigurationType.Text:
                            editProjectCustomDataToProjectFactory.Create()
                                .Invoke(projectId , configurationName , ConfigurationType.Text , projectConfiguration ,
                                    oldConfigurationName);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(crudOption) , crudOption , null);
            }
        }

        private void SendMessage(string content , string firstName , string lastName , string email)
        {
            var emailContent = messageSender.CreateContent(content , EmailConfiguration.Greeting ,
                EmailConfiguration.Sender);

            var message = messageSender.CreateMessage(firstName + lastName , email , EmailConfiguration.Sender ,
                EmailConfiguration.SenderEmail , EmailConfiguration.EmailGrantTitle , emailContent);
            messageSender.SendEmail(message , EmailConfiguration.ServerEmail , EmailConfiguration.ServerPassword ,
                EmailConfiguration.ServerName , 465);
        }
    }
}