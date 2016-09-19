using System;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Implementation
{
    public class Subscription<TEventBase> : ISubscription where TEventBase : EventBase
    {
        public Type SubscriptionType { get; private set; }
        private readonly Action<TEventBase> _action;

        public Subscription(Type subscriptionType, Action<TEventBase> action)
        {
            SubscriptionType = subscriptionType;
            _action = action;
        }

        public void Publish(EventBase eventItem)
        {
            if (!(eventItem is TEventBase))
                throw new ArgumentException("Event Item is not the correct type.");

            _action.Invoke(eventItem as TEventBase);
        }
    }
}