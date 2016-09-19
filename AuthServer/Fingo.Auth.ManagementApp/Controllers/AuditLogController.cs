using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fingo.Auth.Domain.AuditLogs.Factories.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.AuditLog;
using Fingo.Auth.ManagementApp.Configuration;
using Fingo.Auth.ManagementApp.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.ManagementApp.Controllers
{
    [Authorize(Policy = AuthorizationConfiguration.PolicyName)]
    [Route("auditlog")]
    public class AuditLogController : BaseController
    {
        private readonly IGetAllAuditLogFactory getAllAuditLogFactory;
        public AuditLogController(IGetAllAuditLogFactory getAllAuditLogFactory, IEventWatcher eventWatcher, IEventBus eventBus)
            : base(eventWatcher, eventBus)
        {
            this.getAllAuditLogFactory = getAllAuditLogFactory;
        }
        public IActionResult All()
        {
            return View();
        }

        [HttpGet("auditTablePartial")]
        public IActionResult AuditLogTablePartialView(string name="", string type="", string message="", int page = 1,
            int pageSize = 20, FilterByDate filterOption = FilterByDate.All, string from = null, string to = null,
            SortByColumn sortOption = SortByColumn.CreationDate, bool descending = true)
        {
            ViewBag.FilterOption = filterOption;
            ViewBag.FromDate = from;
            ViewBag.ToDate = to;
            ViewBag.SortOption = sortOption;
            ViewBag.IdDescending = descending;
            ViewBag.Name = name;
            ViewBag.Type = type;
            ViewBag.Message = message;

            var auditLogs = getAllAuditLogFactory.Create().Invoke();
            auditLogs = FilterLogs(name , type , message , auditLogs);
            if (filterOption != FilterByDate.All)
                auditLogs = FilterByDateAuditLogs(auditLogs, filterOption, from, to);

            ViewBag.TotalRows = auditLogs.Count();
            ViewBag.UsersCount = auditLogs.Count() / pageSize;
            ViewBag.Page = page;
            ViewBag.RowsPerPage = pageSize;

            auditLogs = SortAuditLogs(auditLogs, sortOption, descending).Skip(pageSize * (page - 1)).Take(pageSize);

            return PartialView("Partials/AuditLogTable", auditLogs);
        }

        private IEnumerable<AuditLogModel> SortAuditLogs(IEnumerable<AuditLogModel> auditLogs, SortByColumn sortOption, bool descending)
        {
            switch (sortOption)
            {
                case SortByColumn.Name:
                    return descending ? auditLogs.OrderByDescending(m => m.UserName) : auditLogs.OrderBy(m => m.UserName);
                case SortByColumn.CreationDate:
                    return descending ? auditLogs.OrderByDescending(m => m.CreationDate) : auditLogs.OrderBy(m => m.CreationDate);
                case SortByColumn.EventMessage:
                    return descending ? auditLogs.OrderByDescending(m => m.EventType) : auditLogs.OrderBy(m => m.EventMessage);
                case SortByColumn.EventType:
                    return descending ? auditLogs.OrderByDescending(m => m.EventType) : auditLogs.OrderBy(m => m.EventType);
                default:
                    return null;
            }
        }

        private IEnumerable<AuditLogModel> FilterByDateAuditLogs(IEnumerable<AuditLogModel> auditLogs,
            FilterByDate filterOption, string from, string to)
        {
            switch (filterOption)
            {
                case FilterByDate.LastWeek:
                    return auditLogs.Where(m => m.CreationDate >= DateTime.Now.AddDays(-7));
                case FilterByDate.LastMonth:
                    return auditLogs.Where(m => m.CreationDate >= DateTime.Now.AddMonths(-1));
                case FilterByDate.Custom:
                    {
                        DateTime fromTime = DateTime.ParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime toTime = DateTime.ParseExact(to, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                        return auditLogs.Where(m => m.CreationDate.Date >= fromTime && m.CreationDate.Date <= toTime);
                    }
                default:
                    return null;
            }
        }
        private IEnumerable<AuditLogModel> FilterLogs(string name, string type, string message, IEnumerable<AuditLogModel> users)
        {
            if (!string.IsNullOrEmpty(name))
                users = users.Where(m => m.UserName.ToLower().Contains(name.ToLower()));
            if (!string.IsNullOrEmpty(type))
                users = users.Where(m => m.EventType.ToLower().Contains(type.ToLower()));
            if (!string.IsNullOrEmpty(message))
                users = users.Where(m => m.EventMessage.ToLower().Contains(message.ToLower()));
            return users;
        }
    }
}
