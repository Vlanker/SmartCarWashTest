using System;
using RabbitMQ.Client;

namespace SmartCarWashTest.EventBusRabbitMq.PersistentConnections
{
    /// <summary>
    /// RabbitMQ persistent connection interface.
    /// </summary>
    public interface IRabbitMqPersistentConnection
        : IDisposable
    {
        /// <summary>
        /// Returns true if the connection is still in a state where it can be used.
        /// Identical to checking if CloseReason equal null.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Trying to connect to a RabbitMQ client.
        /// </summary>
        /// <returns> Returns true if created a connection to the specified endpoint. </returns>
        bool TryConnect();

        /// <summary>
        /// Create and return a fresh channel, session, and model.
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();
    }
}