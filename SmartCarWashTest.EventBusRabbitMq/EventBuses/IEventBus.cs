using SmartCarWashTest.EventBus.Abstractions.EventHandlers;
using SmartCarWashTest.EventBus.Events;

namespace SmartCarWashTest.EventBusRabbitMq.EventBuses
{
    /// <summary>
    /// EventBus interface.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publish.
        /// </summary>
        /// <param name="event"> <see cref="IntegrationEvent"/>. </param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// Subscribe.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <typeparam name="TH"> <see cref="IIntegrationEventHandler{TIntegrationEvent}"/>. </typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Subscribe dynamic.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <typeparam name="TH"> <see cref="IDynamicIntegrationEventHandler"/>. </typeparam>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        /// <typeparam name="T"> <see cref="IntegrationEvent"/>. </typeparam>
        /// <typeparam name="TH"> <see cref="IIntegrationEventHandler{TIntegrationEvent}"/>. </typeparam>
        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Unsubscribe dynamic.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        /// <typeparam name="TH"> <see cref="IDynamicIntegrationEventHandler"/>. </typeparam>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;
    }
}