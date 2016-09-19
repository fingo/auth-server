using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IGetProject
    {
        ProjectDetailWithUsersModel Invoke(int id);
    }
}