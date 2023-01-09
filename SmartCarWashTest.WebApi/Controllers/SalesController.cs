using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Logger;
using SmartCarWashTest.WebApi.DTO.Models.Sale;
using SmartCarWashTest.WebApi.Repositories.Interfaces;

namespace SmartCarWashTest.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISaleCacheRepository _repository;

        public SalesController(ILogger<SalesController> logger, ISaleCacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/sales
        // this will always return a list of customers (but it might be empty)
        /// <summary>
        /// Get a list of Sale.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a list of Sale.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SaleReadModel>))]
        public async Task<IActionResult> GetSales(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Listing all items");

            var collections = await _repository.RetrieveAllAsync(cancellationToken);

            return Ok(collections);
        }


        // GET: api/sales/[id]
        /// <summary>
        /// Get Sale by ID.
        /// </summary>
        /// <param name="id">The ID of the Sale to get.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a item of Sale.</returns>
        /// <response code="400">If the item is not found.</response>
        [HttpGet("{id:int}", Name = nameof(GetSale))] // named route
        [ProducesResponseType(200, Type = typeof(SaleReadModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSale(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting item {Id}", id);

            var sale = await _repository.RetrieveAsync(id, cancellationToken);

            if (sale != null)
                return Ok(sale); // 200 OK with Sale in body

            _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

            return NotFound(); // 404 Resource not found
        }

        // POST: api/sales
        // BODY: Sale (JSON, XML)
        /// <summary>
        /// Create a Sale.
        /// </summary>
        /// <param name="sale">The item to create.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A newly created Sale.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  api/sales
        ///     {
        ///       "id": 0,
        ///       "date": "2023-01-07",
        ///       "time": "45:02.638",
        ///       "salesPointId": 0,
        ///       "buyerId": 0,
        ///       "totalAmount": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item is null.</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(SaleCreateModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] SaleCreateModel sale, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Inserting item {Name}", nameof(sale));

            if (sale == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemIsNullBadRequest,
                    "Item {Sale} is null. BAD REQUEST",
                    nameof(sale));

                return BadRequest(); // 400 Bad request
            }

            var addedSale = await _repository.CreateAsync(sale, cancellationToken);

            if (addedSale == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemNotAddedBadRequest,
                    "Item {Sale} is not added. BAD REQUEST",
                    nameof(addedSale));

                return BadRequest("Repository failed to create Sale.");
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Item {Id} Created", addedSale.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetSale),
                routeValues: new { id = addedSale.Id },
                value: addedSale);
        }

        // PUT: api/sales/[id]
        // BODY: Sale (JSON, XML)
        /// <summary>
        /// Update a existing Sale.
        /// </summary>
        /// <param name="id">The ID of the Sale to update.</param>
        /// <param name="sale">Buyer object that needs to be added.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/sales/[id]
        ///     {
        ///       "id": 0,
        ///       "date": "2023-01-07",
        ///       "time": "45:02.638",
        ///       "salesPointId": 0,
        ///       "buyerId": 0,
        ///       "totalAmount": 0
        ///     }
        ///
        ///
        /// </remarks>
        /// <response code="204">Returns if the item has been updated.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">If the item is not found.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] SaleUpdateModel sale,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Updating item {Name}", nameof(sale));


            if (sale == null || sale.Id != id)
            {
                _logger.LogWarning(
                    LoggingEvents.UpdateItemIsNullOrDifferentIdBadRequest,
                    "Update({Id}) item is null or if and item.Id is different. BAD REQUEST",
                    id);

                return BadRequest(); // 400 Bad request
            }

            var existing = await _repository.RetrieveAsync(id, cancellationToken);

            if (existing == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update({Id}) NOT FOUND", id);

                return NotFound(); // 404 Resource not found
            }

            var updatedSale = await _repository.UpdateAsync(id, sale, cancellationToken);

            if (updatedSale == null)
            {
                _logger.LogWarning(
                    LoggingEvents.UpdateItemNotUpdatedBadRequest,
                    "Update({Id}) not updated. BAD REQUEST",
                    id);

                return BadRequest(); // 400 Bad request
            }

            _logger.LogInformation(LoggingEvents.UpdateItemNotContent, "Item {Id} Updated", id);

            return NoContent(); // 204 No content
        }

        // DELETE: api/sales/[id]
        /// <summary>
        /// Deletes a specific Sale.
        /// </summary>
        /// <param name="id">The ID of the Sale to delete.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <response code="204">Returns if the item has been delete.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">If the item is not found.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Deleting({Id})", id);

            var existing = await _repository.RetrieveAsync(id, cancellationToken);

            if (existing == null)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemNotFound, "Delete({Id}) NOT FOUND", id);

                return NotFound(); // 404 Resource not found
            }

            var deleted = await _repository.DeleteAsync(id, cancellationToken);

            if (!deleted.HasValue || !deleted.Value) // short circuit AND
            {
                _logger.LogWarning(
                    LoggingEvents.DeletedItemNotDeletedBadRequest,
                    "Delete({Id}) NOT DELETED",
                    id);

                return BadRequest($"Sale {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEvents.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}