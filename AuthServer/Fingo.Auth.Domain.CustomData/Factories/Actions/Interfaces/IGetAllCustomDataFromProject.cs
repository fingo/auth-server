using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.CustomData;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface IGetAllCustomDataFromProject
    {
        List<ProjectCustomData> Invoke(int projectId);
    }
}
