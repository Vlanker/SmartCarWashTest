using System;
using System.Collections.Generic;
using System.Linq;
using SmartCarWashTest.EventBus.Abstractions.EventHandlers;
using SmartCarWashTest.EventBus.Events;
using SmartCarWashTest.EventBus.Infos;

namespace SmartCarWashTest.EventBus.EventBusSubscriptionManagers
{
    /// <summary>
    /// Memory EventBus subscriptions manager.
    /// </summary>
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        /// <summary>
        /// List of Handler.
        /// </summary>
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;

        /// <summary>
        /// List of Event type.
        /// </summary>
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public bool IsEmpty => _handlers is { Count: 0 };
        public void Clear() => _handlers.Clear();

        /// <summary>
        /// .ctor.
        /// </summary>
        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        /// <summary>
        /// Doing add Subscription.
        /// </summary>
        /// <param name="handlerType"> Handler type. </param>
        /// <param name="eventName"> Event name. </param>
        /// <param name="isDynamic"> Event is Dynamic. </param>
        /// <exception cref="ArgumentException">
        /// Handler Type <paramref name="handlerType.Name"/> already registered for '<paramref name="eventName"/>'.
        /// </exception>
        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'",
                    nameof(handlerType));
            }

            _handlers[eventName]
                .Add(isDynamic 
                    ? SubscriptionInfo.Dynamic(handlerType) 
                    : SubscriptionInfo.Typed(handlerType));
        }

        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// Doing remove Subscription.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <param name="subsToRemove"> Subscription to remove. </param>
        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove == null)
            {
                return;
            }

            _handlers[eventName].Remove(subsToRemove);

            if (_handlers[eventName].Any())
            {
                return;
            }

            _handlers.Remove(eventName);
            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

            if (eventType != null)
            {
                _eventTypes.Remove(eventType);
            }

            RaiseOnEventRemoved(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        /// <summary>
        /// Find Dynamic Subscription to remove.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <typeparam name="TH"> <see cref="IDynamicIntegrationEventHandler"/>. </typeparam>
        /// <returns> Found <see cref="SubscriptionInfo"/>. </returns>
        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        /// <summary>
        /// Find Subscription to remove.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <typeparam name="TH"> <see cref="IIntegrationEventHandler{TIntegrationEvent}"/>. </typeparam>
        /// <returns> Found <see cref="SubscriptionInfo"/>. </returns>
        private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        /// <summary>
        /// Doing find Subscription to remove.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <param name="handlerType"> Handler type. </param>
        /// <returns>
        /// The single element of the input sequence that satisfies the condition,
        /// or default(<see cref="SubscriptionInfo"/>) if no such element is found.
        /// </returns>
        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            return !HasSubscriptionsForEvent(eventName)
                ? null
                : _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}