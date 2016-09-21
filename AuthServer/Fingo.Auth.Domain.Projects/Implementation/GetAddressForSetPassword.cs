using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class GetAddressForSetPassword : IGetAddressForSetPassword
    {
        private readonly IProjectRepository projectRepository;

        public GetAddressForSetPassword(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public string Invoke(int projectId)
        {
            return projectRepository.GetById(projectId).SetPasswordAddress;
        }
    }
}