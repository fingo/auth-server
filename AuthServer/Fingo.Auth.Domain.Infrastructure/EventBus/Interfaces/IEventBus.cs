using System;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;

        void SubscribeAll(Action<EventBase> action);
        void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase;
    }
}