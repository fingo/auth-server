using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IGetProjectWithAll
    {
        ProjectDetailWithAll Invoke(int projectId);
    }
}