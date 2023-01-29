using System;
using System.Collections.Generic;
using SmartCarWashTest.EventBus.Abstractions.EventHandlers;
using SmartCarWashTest.EventBus.Events;
using SmartCarWashTest.EventBus.Infos;

namespace SmartCarWashTest.EventBus.EventBusSubscriptionManagers
{
    /// <summary>
    /// Interface EventBus subscriptions manager.
    /// </summary>
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// Handlers is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Raised when Event removed.
        /// </summary>
        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// Add Dynamic subscription.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <typeparam name="TH"> <see cref="IDynamicIntegrationEventHandler"/>. </typeparam>
        void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Add subscription.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <typeparam name="TH"> <see cref="IIntegrationEventHandler{TIntegrationEvent}"/>. </typeparam>
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Remove Subscription.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <typeparam name="TH"> <see cref="IIntegrationEventHandler{TIntegrationEvent}"/>. </typeparam>
        void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        /// <summary>
        /// Remove Dynamic Subscription.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <typeparam name="TH"> <see cref="IDynamicIntegrationEventHandler"/>. </typeparam>
        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Has Subscriptions for Event.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <returns>
        /// <see langword="true"/> if the Handlers contains an element with the specified key;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// Has Subscriptions for Event.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <returns>
        /// <see langword="true"/> if the Handlers contains an element with the specified key;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// Get EventType by name.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <returns>
        /// The single element of the input sequence that satisfies the condition,
        /// or default(<see cref="SubscriptionInfo"/>) if no such element is found.
        /// </returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// Removes all keys and values from the Handlers.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get Handlers for Event.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <returns> Enumerable SubscriptionInfo. </returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// Get Handlers for Event.
        /// </summary>
        /// <param name="eventName"> Event name.</param>
        /// <returns> Enumerable SubscriptionInfo. </returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        /// <summary>
        /// Get Event key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A <see cref="String"/> containing the name of this member.</returns>
        string GetEventKey<T>();
    }
}