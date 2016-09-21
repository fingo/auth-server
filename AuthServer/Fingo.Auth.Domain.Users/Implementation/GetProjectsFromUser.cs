using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetProjectsFromUser : IGetProjectsFromUser
    {
        private readonly IUserRepository userRepository;

        public GetProjectsFromUser(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<ProjectModel> Invoke(int userId)
        {
            var projects = userRepository.GetAllProjectsFromUser(userId);
            return projects.Select(project => new ProjectModel(project));
        }
    }
}