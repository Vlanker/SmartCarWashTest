using System;
using System.Text.Json.Serialization;

namespace SmartCarWashTest.EventBus.Events
{
    /// <summary>
    /// Integration event.
    /// </summary>
    public record IntegrationEvent
    {
        /// <summary>
        /// IntegrationEvent ID.
        /// </summary>
        [JsonInclude]
        public Guid Id { get; private init; }

        /// <summary>
        /// IntegrationEvent Date create.
        /// </summary>
        [JsonInclude]
        public DateTime CreationDate { get; private init; }


        /// <summary>
        /// .ctor.
        /// </summary>
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// .ctor.
        /// </summary>
        /// <param name="id">IntegrationEvent ID.</param>
        /// <param name="createDate">IntegrationEvent Date create.</param>
        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }
    }
}