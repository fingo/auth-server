namespace Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces
{
    public interface IEventWatcher
    {
        void StoreEvent(string userId , string eventType , string eventMessage);
    }
}