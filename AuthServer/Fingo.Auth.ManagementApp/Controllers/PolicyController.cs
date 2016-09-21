using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.ManagementApp.Alerts;
using Fingo.Auth.ManagementApp.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    public class PolicyController : BaseController
    {
        private readonly IGetPolicyConfigurationOrDefaultFromProjectFactory configurationOrDefaultFromProjectFactory;
        private readonly IDeletePolicyFromProjectFactory deletePolicyFromProjectFactory;
        private readonly IGetUserDataViewFactory getUserDataViewFactory;
        private readonly ISavePolicyToProjectFactory savePolicyToProjectFactory;
        private readonly ISaveUserPolicyToProjectFactory saveUserPolicyToProjectFactory;

        public PolicyController(IEventBus eventBus , IProjectRepository projectRepository ,
            IDeletePolicyFromProjectFactory deletePolicyFromProjectFactory , IEventWatcher eventWatcher ,
            ISavePolicyToProjectFactory savePolicyToProjectFactory ,
            ISaveUserPolicyToProjectFactory saveUserPolicyToProjectFactory ,
            IGetUserDataViewFactory getUserDataViewFactory ,
            IGetPolicyConfigurationOrDefaultFromProjectFactory configurationOrDefaultFromProjectFactory)
            : base(eventWatcher , eventBus)
        {
            this.deletePolicyFromProjectFactory = deletePolicyFromProjectFactory;
            this.savePolicyToProjectFactory = savePolicyToProjectFactory;
            this.saveUserPolicyToProjectFactory = saveUserPolicyToProjectFactory;
            this.getUserDataViewFactory = getUserDataViewFactory;
            this.configurationOrDefaultFromProjectFactory = configurationOrDefaultFromProjectFactory;
        }

        [HttpGet("{policy}/{projectId}/{userId}")]
        public IActionResult GetUserParialViewResult(Policy policy , int projectId , int userId)
        {
            try
            {
                ViewData["projectId"] = projectId;
                switch (policy)
                {
                    case Policy.AccountExpirationDate:
                        return PartialView("UserAccountExpirationDate" ,
                            getUserDataViewFactory.Create().Invoke(projectId , userId));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(policy) , policy , null);
                }
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return StatusCode(400);
            }
        }

        [HttpGet("{policy}/{projectId}")]
        public IActionResult GetProjectPartialViewResult(Policy policy , int projectId)
        {
            try
            {
                ViewBag.ProjectId = projectId;
                switch (policy)
                {
                    case Policy.AccountExpirationDate:
                        return PartialView("AccountExpirationDate" ,
                            (AccountExpirationDateConfiguration)
                            configurationOrDefaultFromProjectFactory.Create()
                                .Invoke(projectId , Policy.AccountExpirationDate));
                    case Policy.MinimumPasswordLength:
                        return PartialView("MinimumPasswordLength" ,
                            (MinimumPasswordLengthConfiguration)
                            configurationOrDefaultFromProjectFactory.Create()
                                .Invoke(projectId , Policy.MinimumPasswordLength));
                    case Policy.PasswordExpirationDate:
                        return PartialView("PasswordExpiration" ,
                            (PasswordExpirationDateConfiguration)
                            configurationOrDefaultFromProjectFactory.Create()
                                .Invoke(projectId , Policy.PasswordExpirationDate));
                    case Policy.RequiredPasswordCharacters:
                        return PartialView("RequiredCharacters" ,
                            (RequiredPasswordCharactersConfiguration)
                            configurationOrDefaultFromProjectFactory.Create()
                                .Invoke(projectId , Policy.RequiredPasswordCharacters));
                    case Policy.ExcludeCommonPasswords:
                        return PartialView("ExcludeCommonPasswords" ,
                            (ExcludeCommonPasswordsConfiguration)
                            configurationOrDefaultFromProjectFactory.Create()
                                .Invoke(projectId , Policy.ExcludeCommonPasswords));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(policy) , policy , null);
                }
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return StatusCode(400);
            }
        }

        [HttpPost("/RequiredPasswordCharacters/{projectId}")]
        public IActionResult SaveRequiredPasswordCharactersPolicy(Policy policy , int projectId ,
            RequiredPasswordCharactersConfiguration policySettings)
        {
            try
            {
                savePolicyToProjectFactory.Create().Invoke(projectId , policy , policySettings);
                Alert(AlertType.Success , "Policy saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpPost("/ExcludeCommonPasswords/{projectId}")]
        public IActionResult ExcludeCommonPasswordsPolicy(Policy policy , int projectId ,
            ExcludeCommonPasswordsConfiguration policySettings)
        {
            try
            {
                savePolicyToProjectFactory.Create().Invoke(projectId , policy , policySettings);
                Alert(AlertType.Success , "Policy saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpPost("/AccountExpirationDate/{projectId}")]
        public IActionResult SaveAccountExpirationDatePolicy(Policy policy , int projectId ,
            AccountExpirationDateConfiguration policySettings)
        {
            try
            {
                savePolicyToProjectFactory.Create().Invoke(projectId , policy , policySettings);
                Alert(AlertType.Success , "Policy saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpPost("{projectId}")]
        public IActionResult SaveUserAccountExpirationDatePolicy(int projectId , UserDateView userAccExpDate)
        {
            try
            {
                saveUserPolicyToProjectFactory.Create()
                    .Invoke(projectId , userAccExpDate.UserId , Policy.AccountExpirationDate ,
                        userAccExpDate.DateTime == null
                            ? default(DateTime)
                            : DateTime.ParseExact(userAccExpDate.DateTime , "dd-MM-yyyy" , CultureInfo.InvariantCulture));
                Alert(AlertType.Success , "Policy saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpPost("/MinimumPasswordLength/{projectId}")]
        public IActionResult SaveMinimumPasswordLengthPolicy(Policy policy , int projectId ,
            MinimumPasswordLengthConfiguration policySettings)
        {
            try
            {
                savePolicyToProjectFactory.Create().Invoke(projectId , policy , policySettings);
                Alert(AlertType.Success , "Policy saved correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpPost("/PasswordExpirationDate/{projectId}")]
        public IActionResult SavePasswordExpirationPolicy(Policy policy , int projectId ,
            PasswordExpirationDateConfiguration policySettings)
        {
            try
            {
                savePolicyToProjectFactory.Create().Invoke(projectId , policy , policySettings);
                Alert(AlertType.Success , "Policy added correctly.");
                return RedirectToAction("GetById" , "Projects" , new {id = projectId});
            }
            catch (Exception ex)
            {
                Alert(AlertType.Warning , ex.Message);
                return View("ErrorPage");
            }
        }

        [HttpDelete("{projectId}/{policy}")]
        public HttpResponseMessage RemovePolicy(int projectId , Policy policy)
        {
            try
            {
                deletePolicyFromProjectFactory.Create().Invoke(projectId , policy);
                Alert(AlertType.Success , "Policy was correctly removed from the project.");
            }
            catch (Exception)
            {
                Alert(AlertType.Warning , "Could not find such policy in the database.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}