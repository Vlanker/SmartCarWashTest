using System.Threading.Tasks;
using SmartCarWashTest.EventBus.Events;

namespace SmartCarWashTest.EventBus.Abstractions.EventHandlers
{
    /// <summary>
    /// Contravariant Integration Event Handler interface.
    /// </summary>
    /// <typeparam name="TIntegrationEvent"> Integration Event Handler. </typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IInternalIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Handle of Integration Event Handler interface.
        /// </summary>
        /// <param name="event"> Event. </param>
        /// <returns> <see cref="Task"/>. </returns>
        Task Handle(TIntegrationEvent @event);
    }
}