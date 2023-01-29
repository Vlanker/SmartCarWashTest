using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.Logger.Events;

namespace SmartCarWashTest.CRUD.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class BuyersController : ControllerBase
    {
        private readonly ILogger<BuyersController> _logger;
        private readonly IBuyerCacheRepository _repository;

        public BuyersController(ILogger<BuyersController> logger, IBuyerCacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/buyers
        // this will always return a list of customers (but it might be empty)
        /// <summary>
        /// Get a list of Buyers.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a list of Buyers.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BuyerReadModel>))]
        public async Task<IActionResult> GetBuyersAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.ListItems, "Listing all items");

            var collections = await _repository.RetrieveAllAsync(cancellationToken);

            return Ok(collections);
        }


        // GET: api/buyers/[id]
        /// <summary>
        /// Get Buyer by ID.
        /// </summary>
        /// <param name="id">The ID of the Buyer to get.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a item of Buyer.</returns>
        /// <response code="404">If the item is not found.</response>
        [HttpGet("{id:int}", Name = nameof(GetBuyerAsync))] // named route
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuyerReadModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBuyerAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.GetItem, "Getting item {Id}", id);

            var buyer = await _repository.RetrieveAsync(id, cancellationToken);

            if (buyer != null)
                return Ok(buyer); // 200 OK with buyer in body

            _logger.LogWarning(LoggingEventIds.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

            return NotFound(); // 404 Resource not found
        }

        // POST: api/buyers
        // BODY: Buyer (JSON, XML)
        /// <summary>
        /// Create a Buyer.
        /// </summary>
        /// <param name="buyer">The item to create.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A newly created Buyer.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  api/buyers
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BuyerCreateModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] BuyerCreateModel buyer, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.InsertItem, "Inserting item {Name}", nameof(buyer));

            if (buyer == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemIsNullBadRequest,
                    "Item {Buyer} is null. BAD REQUEST",
                    nameof(buyer));

                return BadRequest(); // 400 Bad request
            }

            var addedBuyer = await _repository.CreateAsync(buyer, cancellationToken);

            if (addedBuyer == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemNotAddedBadRequest,
                    "Item {Buyer} is not added. BAD REQUEST",
                    nameof(addedBuyer));

                return BadRequest("Repository failed to create buyer.");
            }

            _logger.LogInformation(LoggingEventIds.InsertItem, "Item {Id} Created", addedBuyer.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetBuyerAsync),
                routeValues: new { id = addedBuyer.Id },
                value: addedBuyer);
        }

        // PUT: api/buyers/[id]
        // BODY: Buyer (JSON, XML)
        /// <summary>
        /// Update a existing Buyer.
        /// </summary>
        /// <param name="id">The ID of the Buyer to update.</param>
        /// <param name="buyer">Buyer object that needs to be added.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/buyers/[id]
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Returns if the item has been updated.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">If the item is not found.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] BuyerUpdateModel buyer,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.UpdateItem, "Inserting item {Name}", nameof(buyer));

            if (buyer == null || buyer.Id != id)
            {
                _logger.LogWarning(
                    LoggingEventIds.UpdateItemIsNullOrDifferentIdBadRequest,
                    "Update({Id}) item is null or if and item.Id is different. BAD REQUEST",
                    id);

                return BadRequest(); // 400 Bad request
            }

            var existing = await _repository.RetrieveAsync(id, cancellationToken);

            if (existing == null)
            {
                _logger.LogWarning(LoggingEventIds.UpdateItemNotFound, "Update({Id}) NOT FOUND", id);

                return NotFound(); // 404 Resource not found
            }

            var updatedBuyer = await _repository.UpdateAsync(id, buyer, cancellationToken);

            if (updatedBuyer == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.UpdateItemNotUpdatedBadRequest,
                    "Update({Id}) not updated. BAD REQUEST",
                    id);

                return BadRequest(); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.UpdateItemNotContent, "Item {Id} Updated", id);

            return NoContent(); // 204 No content
        }

        // DELETE: api/buyers/[id]
        /// <summary>
        /// Deletes a specific Buyer.
        /// </summary>
        /// <param name="id">The ID of the Buyer to delete.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <response code="204">Returns if the item has been delete.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">If the item is not found.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.DeleteItem, "Deleting({Id})", id);

            var existing = await _repository.RetrieveAsync(id, cancellationToken);

            if (existing == null)
            {
                _logger.LogWarning(LoggingEventIds.DeleteItemNotFound, "Delete({Id}) NOT FOUND", id);

                return NotFound(); // 404 Resource not found
            }

            var deleted = await _repository.DeleteAsync(id, cancellationToken);

            if (!deleted.HasValue || !deleted.Value)
            {
                _logger.LogWarning(
                    LoggingEventIds.DeletedItemNotDeletedBadRequest,
                    "Delete({Id}) NOT DELETED",
                    id);

                return BadRequest($"Buyer {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}