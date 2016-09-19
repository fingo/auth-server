using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class GetProjectWithCustomDataFactory: IGetProjectWithCustomDataFactory
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectWithCustomDataFactory(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IGetProjectWithCustomData Create()
        {
            return new GetProjectWithCustomData(_projectRepository);
        }
    }
}