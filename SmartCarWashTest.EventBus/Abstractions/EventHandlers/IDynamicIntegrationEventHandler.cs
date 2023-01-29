using System.Threading.Tasks;

namespace SmartCarWashTest.EventBus.Abstractions.EventHandlers
{
    /// <summary>
    /// Dynamic integration EventHandler interface.
    /// </summary>
    public interface IDynamicIntegrationEventHandler
    {
        /// <summary>
        /// Handle of Dynamic integration EventHandler interface.
        /// </summary>
        /// <param name="eventData"> Event data. </param>
        /// <returns> <see cref="System.Threading.Tasks.Task"/>. </returns>
        Task Handle(dynamic eventData);
    }
}