using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.ManagementApp.Configuration;
using Fingo.Auth.ManagementApp.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Route("ModalWindow")]
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    public class ModalWindowsController : Controller
    {
        private readonly IGetAllNotAssignedUsersToProjectFactory allNotAssignedUsersToProjectFactory;
        private readonly IGetPoliciesUnassignedToProjectFactory getPoliciesUnassignedToProjectFactory;
        private readonly IGetProjectFactory getProjectFactory;
        private readonly IGetProjectsFromUserFactory getProjectsFromUserFactory;
        private readonly IGetProjectWithCustomDataFactory getProjectWithCustomDataFactory;
        private readonly IGetUserFactory getUserFactory;

        public ModalWindowsController(IGetProjectsFromUserFactory getProjectsFromUserFactory ,
            IGetUserFactory getUserFactory ,
            IGetPoliciesUnassignedToProjectFactory getPoliciesUnassignedToProjectFactory ,
            IGetAllNotAssignedUsersToProjectFactory allNotAssignedUsersToProjectFactory ,
            IGetProjectFactory getProjectFactory , IGetProjectWithCustomDataFactory getProjectWithCustomDataFactory)
        {
            this.getProjectsFromUserFactory = getProjectsFromUserFactory;
            this.getUserFactory = getUserFactory;
            this.getPoliciesUnassignedToProjectFactory = getPoliciesUnassignedToProjectFactory;
            this.allNotAssignedUsersToProjectFactory = allNotAssignedUsersToProjectFactory;
            this.getProjectFactory = getProjectFactory;
            this.getProjectWithCustomDataFactory = getProjectWithCustomDataFactory;
        }

        [HttpGet("~/CollapseWindow/{userId}")]
        public PartialViewResult GetUserProjectsCollapseWindow(int userId)
        {
            return PartialView("User/ProjectConnectedWithUserCollapseWindow" ,
                getProjectsFromUserFactory.Create().Invoke(userId));
        }

        [HttpGet("user")]
        public PartialViewResult UserModalWindow(ModalWidows modalOption , int projectId , int userId)
        {
            ViewBag.ProjectId = projectId;
            switch (modalOption)
            {
                case ModalWidows.EditUserPolicyModalWindow:
                    return PartialView("User/EditUserPolicyModalWindow" , getUserFactory.Create().Invoke(userId));
                case ModalWidows.DeleteUser:
                    return PartialView("User/DeleteUserModalWindow" , getUserFactory.Create().Invoke(userId));
                case ModalWidows.AssignUsers:
                    return PartialView("User/AssignUsersModalWindow" ,
                        allNotAssignedUsersToProjectFactory.Create().Invoke(projectId));
                case ModalWidows.ImportUsersFromCsv:
                    return PartialView("User/ImportUsersFromCsvModalWindow");
            }
            return new PartialViewResult();
        }

        [HttpGet("policy")]
        public PartialViewResult PolicyModalWindow(int projectId , Crud crudOption , Policy policy)
        {
            ViewBag.ProjectId = projectId;
            switch (crudOption)
            {
                case Crud.Add:
                    return PartialView("Policy/AddPolicyModalWindow" ,
                        getPoliciesUnassignedToProjectFactory.Create().Invoke(projectId));
                case Crud.Edit:
                    return PartialView("Policy/EditPolicyModalWindow" , policy);
                case Crud.Delete:
                    return PartialView("Policy/DeletePolicyModalWindow" , policy);
            }
            return new PartialViewResult();
        }


        [HttpGet("project")]
        public PartialViewResult ProjectModalWindow(ModalWidows modalOptions , int userId , int projectId)
        {
            ViewBag.UserId = userId;
            switch (modalOptions)
            {
                case ModalWidows.UnassignUser:
                    return PartialView("Project/UnassignFromProjectModalWindow" ,
                        getProjectFactory.Create().Invoke(projectId));
            }
            return new PartialViewResult();
        }

        [HttpGet("customData")]
        public PartialViewResult CustomDataModalWindow(int projectId , Crud crudOption , string configurationName ,
            ConfigurationType configurationType)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.ConfigurationType = configurationType;
            switch (crudOption)
            {
                case Crud.Add:
                    return PartialView("CustomData/AddCustomData" ,
                        Enum.GetValues(typeof(ConfigurationType)).OfType<ConfigurationType>());
                case Crud.Edit:
                    return PartialView("CustomData/EditCustomData" , configurationName);
                case Crud.Delete:
                    return PartialView("CustomData/DeleteCustomData" , configurationName);
            }
            return new PartialViewResult();
        }

        [HttpGet("userCustomData")]
        public PartialViewResult UserCustomDataModalWindow(int projectId , int userId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.UserId = userId;
            return PartialView("CustomData/EditUserCustomData" ,
                getProjectWithCustomDataFactory.Create().Invoke(projectId));
        }
    }
}