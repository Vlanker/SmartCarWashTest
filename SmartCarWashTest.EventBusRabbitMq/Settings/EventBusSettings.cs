using RabbitMQ.Client;

namespace SmartCarWashTest.EventBusRabbitMq.Settings
{
    /// <summary>
    /// Settings EventBus.
    /// </summary>
    /// <param name="HostName">The host to connect to.</param>
    /// <param name="Port">
    /// The port to connect on. <see cref="AmqpTcpEndpoint.UseDefaultPort"/>
    /// indicates the default for the protocol should be used.
    /// </param>
    /// <param name="UserName">Username to use when authenticating to the server.</param>
    /// <param name="Password">Password to use when authenticating to the server.</param>
    /// <param name="RetryCount">The retry count.</param>
    /// <param name="SubscriptionClientName">
    /// Subscription client name.
    /// Set the name of the queue.
    /// Pass an empty string to make the server generate a name.
    /// </param>
    public sealed record EventBusSettings(string HostName, int Port, string UserName, string Password, int RetryCount,
        string SubscriptionClientName)
    {
        public EventBusSettings() : this(string.Empty, 0, string.Empty, string.Empty, 0, string.Empty)
        {
        }
    }
}