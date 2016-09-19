using System.Collections.Generic;
using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IGetAllProjects
    {
        IEnumerable<ProjectModel> Invoke();
    }
}