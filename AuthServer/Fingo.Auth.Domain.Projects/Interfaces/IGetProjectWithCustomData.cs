using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IGetProjectWithCustomData
    {
        ProjectWithCustomDataModel Invoke(int projectId);
    }
}