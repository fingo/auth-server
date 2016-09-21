using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Implementation.GenericImplementation;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fingo.Auth.DbAccess.Repository.Implementation
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        private readonly IAuthServerContext _db;

        public UserRepository(IAuthServerContext context) : base(context)
        {
            _db = context;
        }

        public override IEnumerable<User> GetAll()
        {
            return _db.Set<User>()
                .Include(u => u.ProjectUsers)
                .Include(m => m.UserCustomData)
                .ThenInclude(m => m.ProjectCustomData);
        }

        public override User GetById(int id)
        {
            var user = _db.Set<User>()
                .Include(u => u.ProjectUsers)
                .Include(u => u.UserCustomData)
                .ThenInclude(m => m.ProjectCustomData)
                .Include(m => m.UserPolicies)
                .ThenInclude(m => m.ProjectPolicies)
                .FirstOrDefault(u => u.Id == id);
            return user;
        }

        public void UpdateUserPassword(User user , string password)
        {
            user.Password = password;
            _db.Set<User>().Update(user);
            _db.SaveChanges();
        }

        public IEnumerable<Project> GetAllProjectsFromUser(int id)
        {
            var d = _db.Set<User>()
                .Where(m => m.Id == id)
                .Include(p => p.ProjectUsers).ThenInclude(u => u.Project)
                .FirstOrDefault();

            var projects = d.ProjectUsers.Select(m => m.Project);

            return projects;
        }

        public User GetByIdWithCustomDatas(int id)
        {
            var user = _db.Set<User>()
                .Include(u => u.UserCustomData)
                .FirstOrDefault(u => u.Id == id);
            return user;
        }
    }
}