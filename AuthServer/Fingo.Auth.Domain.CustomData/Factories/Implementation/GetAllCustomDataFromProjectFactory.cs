using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class GetAllCustomDataFromProjectFactory : IGetAllCustomDataFromProjectFactory
    {
        private readonly IProjectRepository _projectRepository;

        public GetAllCustomDataFromProjectFactory(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IGetAllCustomDataFromProject Create()
        {
            return new GetAllCustomDataFromProject(_projectRepository);
        }
    }
}