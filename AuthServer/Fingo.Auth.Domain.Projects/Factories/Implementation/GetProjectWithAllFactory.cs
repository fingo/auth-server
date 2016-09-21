using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class GetProjectWithAllFactory : IGetProjectWithAllFactory
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectWithAllFactory(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IGetProjectWithAll Create()
        {
            return new GetProjectWithAll(_projectRepository);
        }
    }
}