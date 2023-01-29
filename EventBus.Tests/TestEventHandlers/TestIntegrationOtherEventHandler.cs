using System.Threading.Tasks;
using EventBus.Tests.TestIntegrationsEvents;
using SmartCarWashTest.EventBus.Abstractions.EventHandlers;

namespace EventBus.Tests.TestEventHandlers
{
    public class TestIntegrationOtherEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        public bool Handled { get; private set; }

        public TestIntegrationOtherEventHandler()
        {
            Handled = false;
        }

        public async Task Handle(TestIntegrationEvent @event)
        {
            Handled = true;
        }
    }
}