using System.Collections.Generic;
using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IGetProjectsFromUser
    {
        IEnumerable<ProjectModel> Invoke(int userId);
    }
}