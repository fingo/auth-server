using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface IGetPoliciesFromProject
    {
        List<Tuple<Policy, PolicyConfiguration>> Invoke(int projectId);
    }
}