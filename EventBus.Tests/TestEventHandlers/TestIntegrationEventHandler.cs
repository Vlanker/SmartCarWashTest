using System.Threading.Tasks;
using EventBus.Tests.TestIntegrationsEvents;
using SmartCarWashTest.EventBus.Abstractions.EventHandlers;

namespace EventBus.Tests.TestEventHandlers
{
    public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        public bool Handled { get; private set; }

        public TestIntegrationEventHandler()
        {
            Handled = false;
        }

        public async Task Handle(TestIntegrationEvent @event)
        {
            Handled = true;
        }
    }
}