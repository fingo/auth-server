using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.Statuses;
using System.Linq;

namespace Fingo.Auth.Domain.Infrastructure.ExtensionMethods
{
    public static class CheckIsInProjectStatuses
    {
        public static T WithStatuses<T>(this T model , params ProjectStatus[] list) where T : BaseEntityWithProjectStatus
        {
            if(list.Length == 0)
                return model;

            if(model == null)
                return null;
            return list.Any(t => model.Status == t) ? model : null;
        }
        public static IEnumerable<T> WithStatuses<T>(this IEnumerable<T> modelList , params ProjectStatus[] list) where T : BaseEntityWithProjectStatus
        {
            if(list.Length == 0)
                return modelList;

            if(modelList == null)
                return null;

            List<T> modelWithStatuses = new List<T>();

            foreach(var model in modelList)
            {
                if(list.Any(s => model.Status == s))
                    modelWithStatuses.Add(model);
            }

            return modelWithStatuses;
        }

        public static T WithoutStatuses<T>(this T model , params ProjectStatus[] list) where T : BaseEntityWithProjectStatus
        {
            if(list.Length == 0)
                return model;

            if(model == null)
                return null;

            return list.Any(t => model.Status == t) ? null : model;
        }
        public static IEnumerable<T> WithoutStatuses<T>(this IEnumerable<T> modelList , params ProjectStatus[] list) where T : BaseEntityWithProjectStatus
        {
            if(list.Length == 0)
                return modelList;

            if(modelList == null)
                return null;

            List<T> modelWithStatuses = new List<T>();

            foreach(var model in modelList)
            {
                if(list.All(s => model.Status != s))
                    modelWithStatuses.Add(model);
            }

            return modelWithStatuses;
        }
    }
}