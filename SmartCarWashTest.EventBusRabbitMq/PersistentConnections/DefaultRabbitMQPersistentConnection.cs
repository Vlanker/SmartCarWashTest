using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace SmartCarWashTest.EventBusRabbitMq.PersistentConnections
{
    /// <summary>
    /// Default RabbitMQ persistent connection.
    /// </summary>
    public class DefaultRabbitMqPersistentConnection
        : IRabbitMqPersistentConnection
    {
        /// <summary>
        /// RabbitMQ .NET AMQP client API.
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger<DefaultRabbitMqPersistentConnection> _logger;

        /// <summary>
        /// The retry count.
        /// </summary>
        private readonly int _retryCount;

        /// <summary>
        /// Sync object.
        /// </summary>
        private readonly object _syncRoot = new();

        /// <summary>
        /// A connection to the specified endpoint.
        /// </summary>
        private IConnection _connection;

        public bool Disposed;
        public bool IsConnected => _connection is { IsOpen: true } && !Disposed;

        /// <summary>
        /// Default RabbitMQ persistent connection.
        /// </summary>
        /// <param name="connectionFactory"> RabbitMQ .NET AMQP client API. </param>
        /// <param name="logger"> Logger. </param>
        /// <param name="retryCount"> The retry count. </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionFactory"/> or <paramref name="logger"/> is null.
        /// </exception>
        public DefaultRabbitMqPersistentConnection(IConnectionFactory connectionFactory,
            ILogger<DefaultRabbitMqPersistentConnection> logger, int retryCount)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;

            try
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (_syncRoot)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (ex, time) =>
                        {
                            _logger.LogWarning(ex,
                                "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
                                $"{time.TotalSeconds:n1}", ex.Message);
                        }
                    );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                        .CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation(
                        "RabbitMQ Client acquired a persistent connection to '{HostName}' " +
                        "and is subscribed to failure events",
                        _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        /// <summary>
        /// Signals when the connection is blocked.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e">
        /// An <see cref="RabbitMQ.Client.Events.ConnectionBlockedEventArgs"/> that contains the event data.
        /// </param>
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (Disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        /// <summary>
        /// Signalled when an exception occurs in a callback invoked by the connection.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e">
        /// An <see cref="RabbitMQ.Client.Events.CallbackExceptionEventArgs"/> that contains the event data.
        /// </param>
        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (Disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        /// <summary>
        /// Raised when the connection is destroyed.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> An <see cref="RabbitMQ.Client .ShutdownEventArgs"/> that contains the event data. </param>
        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (Disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}