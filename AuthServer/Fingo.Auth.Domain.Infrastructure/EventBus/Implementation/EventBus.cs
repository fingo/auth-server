using System;
using System.Collections.Generic;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Implementation
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type , List<ISubscription>> _subscriptions;

        public EventBus()
        {
            _subscriptions = new Dictionary<Type , List<ISubscription>>();
        }

        public void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (!_subscriptions.ContainsKey(typeof(TEventBase)))
                _subscriptions.Add(typeof(TEventBase) , new List<ISubscription>());

            var subscription = new Subscription<TEventBase>(typeof(TEventBase) , action);

            _subscriptions[typeof(TEventBase)].Add(subscription);
        }

        public void SubscribeAll(Action<EventBase> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (!_subscriptions.ContainsKey(typeof(EventBase)))
                _subscriptions.Add(typeof(EventBase) , new List<ISubscription>());

            var subscription = new Subscription<EventBase>(typeof(EventBase) , action);

            _subscriptions[typeof(EventBase)].Add(subscription);
        }

        public void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            if (eventItem == null)
                throw new ArgumentNullException("eventItem");

            var subscriptions = new List<ISubscription>();

            if (_subscriptions.ContainsKey(typeof(TEventBase)))
                subscriptions = _subscriptions[typeof(TEventBase)];

            if (_subscriptions.ContainsKey(typeof(EventBase)))
                subscriptions.AddRange(_subscriptions[typeof(EventBase)]);

            foreach (var subscription in subscriptions)
                try
                {
                    subscription.Publish(eventItem);
                }
                catch (Exception)
                {
                    // ignored
                }
        }
    }
}