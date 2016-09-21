using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class GetAddressForSetPasswordFactory : IGetAddressForSetPasswordFactory
    {
        private readonly IProjectRepository projectRepository;

        public GetAddressForSetPasswordFactory(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public IGetAddressForSetPassword Create()
        {
            return new GetAddressForSetPassword(projectRepository);
        }
    }
}