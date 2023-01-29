using System;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.EventBusRabbitMq.EventBuses;
using SmartCarWashTest.Sale.WebApi.DTOs.Events;

namespace SmartCarWashTest.Sale.WebApi.Controllers
{
    /// <summary>
    /// Sale.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/s/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        /// <summary>
        /// Sale.
        /// </summary>
        /// <param name="logger"> <see cref="ILogger{TCategoryName}"/>. </param>
        /// <param name="eventBus"> <see cref="IEventBus"/>. </param>
        /// <param name="mapper"> <see cref="IMapper"/>. </param>
        public SalesController(ILogger<SalesController> logger, IEventBus eventBus, IMapper mapper)
        {
            _logger = logger;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        /// <summary>
        /// Send sale.
        /// </summary>
        /// <param name="sale"> Sale. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        /// <returns></returns>
        /// <response code="202"> Published.</response>
        /// <response code="400"> Bad request. Not Published. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] DTOs.Models.Sale sale, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Get Sale to Publish ({Sale})", nameof(sale));

            var eventMessage = _mapper.Map<SalePublishedIntegrationEvent>(sale);

            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}",
                    eventMessage.Id, Program.AppName);

                return BadRequest();
            }

            return Accepted();
        }
    }
}