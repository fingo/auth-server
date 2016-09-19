using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.ExtensionMethods
{
    public static class CheckType
    {
        public static IEnumerable<Policy> ConfigurablePerUser(this IEnumerable<Policy> policies)
        {
            return policies?.Where(policy => PolicyData.IsConfigurablePerUser[policy]);
        }

        public static IEnumerable<Tuple<Policy, T>> ConfigurablePerUser<T> (this IEnumerable<Tuple<Policy, T>> policies)
        {
            return policies?.Where(policy => PolicyData.IsConfigurablePerUser[policy.Item1]);
        }

        public static IEnumerable<Policy> WithTypes
            (this IEnumerable<Policy> policies, params PolicyType[] types)
        {
            return types.Length == 0 ? policies : policies?.Where(policy => types.Any(pt => PolicyData.Type[policy] == pt));
        }

        public static IEnumerable<Tuple<Policy, T>> WithTypes<T>
            (this IEnumerable<Tuple<Policy, T>> policies, params PolicyType[] types)
        {
            return types.Length == 0 ? policies : policies?.Where(policy => types.Any(pt => PolicyData.Type[policy.Item1] == pt));
        }
    }
}