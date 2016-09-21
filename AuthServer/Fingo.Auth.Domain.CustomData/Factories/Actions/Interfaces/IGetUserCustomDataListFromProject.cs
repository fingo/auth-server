using System;
using System.Collections.Generic;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface IGetUserCustomDataListFromProject
    {
        List<Tuple<string , string>> Invoke(Guid projectGuid , string userLogin);
    }
}