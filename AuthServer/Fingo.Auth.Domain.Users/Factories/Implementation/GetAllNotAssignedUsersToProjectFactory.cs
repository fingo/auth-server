using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetAllNotAssignedUsersToProjectFactory : IGetAllNotAssignedUsersToProjectFactory
    {
        private readonly IUserRepository repository;

        public GetAllNotAssignedUsersToProjectFactory(IUserRepository repository)
        {
            this.repository = repository;
        }

        public IGetAllNotAssignedUsersToProject Create()
        {
            return new GetAllNotAssignedUsersToProject(repository);
        }
    }
}