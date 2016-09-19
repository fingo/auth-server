using System;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces
{
    public interface ISubscription
    {
        Type SubscriptionType { get; }

        void Publish(EventBase eventItem);
    }
}