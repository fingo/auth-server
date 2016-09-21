namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface IDeleteCustomDataFromProject
    {
        void Invoke(int projectId , string configurationName);
    }
}