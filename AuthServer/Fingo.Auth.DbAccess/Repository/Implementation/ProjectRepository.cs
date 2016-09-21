using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Implementation.GenericImplementation;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fingo.Auth.DbAccess.Repository.Implementation
{
    public class ProjectRepository : GenericRepository<Project> , IProjectRepository
    {
        private readonly IAuthServerContext _db;

        public ProjectRepository(IAuthServerContext context) : base(context)
        {
            _db = context;
        }

        public IEnumerable<User> GetAllUsersFromProject(int id)
        {
            var d = _db.Set<Project>()
                .Where(m => m.Id == id)
                .Include(p => p.ProjectUsers).ThenInclude(u => u.User)
                .FirstOrDefault();
            var users = d.ProjectUsers.Select(m => m.User);
            return users;
        }

        public override Project GetById(int id)
        {
            var project = _db.Set<Project>()
                .Where(m => m.Id == id)
                .Include(m => m.Information)
                .Include(m => m.ProjectPolicies)
                .Include(m => m.ProjectCustomData)
                .ThenInclude(m => m.UserCustomData)
                .FirstOrDefault();

            return project;
        }

        public Project GetByIdWithPolicies(int id)
        {
            var project = _db.Set<Project>()
                .Where(m => m.Id == id)
                .Include(m => m.ProjectPolicies)
                .ThenInclude(m => m.UserPolicies)
                .FirstOrDefault();
            return project;
        }

        public Project GetByIdWithAll(int id)
        {
            var project = _db.Set<Project>()
                .Where(m => m.Id == id)
                .Include(m => m.Information)
                .Include(m => m.ProjectPolicies)
                .Include(m => m.ProjectCustomData)
                .ThenInclude(m => m.UserCustomData)
                .Include(m => m.ProjectUsers)
                .FirstOrDefault();

            return project;
        }

        public Project GetByGuid(Guid guid)
        {
            var project =
                _db.Set<Project>()
                    .Where(m => m.ProjectGuid == guid)
                    .Include(m => m.Information)
                    .Include(m => m.ProjectCustomData)
                    .ThenInclude(m => m.UserCustomData)
                    .FirstOrDefault();
            return project;
        }

        public Project GetByIdWithCustomDatas(int projectId)
        {
            var project = _db.Set<Project>()
                .Where(m => m.Id == projectId)
                .Include(m => m.ProjectCustomData)
                .ThenInclude(m => m.UserCustomData)
                .Include(m => m.ProjectUsers)
                .FirstOrDefault();
            return project;
        }
    }
}