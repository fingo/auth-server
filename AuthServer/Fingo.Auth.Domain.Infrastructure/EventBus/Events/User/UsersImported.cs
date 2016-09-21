using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UsersImported : EventBase
    {
        public UsersImported(int added , int duplicated , int all , int projectId)
        {
            Added = added;
            Duplicated = duplicated;
            All = all;
            ProjectId = projectId;
        }

        public int ProjectId { get; }
        public int Added { get; }
        public int Duplicated { get; }
        public int All { get; }

        public override string ToString()
        {
            return
                $"Successfully added {Added} users from file to project (id: {ProjectId}). {Duplicated} users were not added " +
                $"because of being a duplicate, {All - Added - Duplicated} were not added because of having not valid " +
                $"e-mail adress.";
        }
    }
}