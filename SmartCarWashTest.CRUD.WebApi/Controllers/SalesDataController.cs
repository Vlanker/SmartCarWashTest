using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.Logger.Events;

namespace SmartCarWashTest.CRUD.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SalesDataController : ControllerBase
    {
        private readonly ILogger<SalesDataController> _logger;
        private readonly ISalesDataCacheRepository _repository;

        public SalesDataController(ILogger<SalesDataController> logger, ISalesDataCacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/salesData
        // this will always return a list of customers (but it might be empty)
        /// <summary>
        /// Get a list of SalesData.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a list of SalesData.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SalesDataReadModel>))]
        public async Task<IActionResult> GetSalesDataAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.ListItems, "Listing all items");

            var collections = await _repository.RetrieveAllAsync(cancellationToken);

            return Ok(collections);
        }


        // GET: api/salesData/[id]
        /// <summary>
        /// Get SalesData by ID.
        /// </summary>
        /// <param name="id">The ID of the SalesData to get.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a item of SalesData.</returns>
        /// <response code="400">If the item is not found.</response>
        [HttpGet("{id:int}", Name = nameof(GetSalesDataAsync))] // named route
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesDataReadModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSalesDataAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.GetItem, "Getting item {Id}", id);

            var salesData = await _repository.RetrieveAsync(id, cancellationToken);

            if (salesData != null)
                return Ok(salesData); // 200 OK with SalesData in body

            _logger.LogWarning(LoggingEventIds.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

            return NotFound(); // 404 Resource not found
        }

        // POST: api/salesData
        // BODY: SalesData (JSON, XML)
        /// <summary>
        /// Create a SalesData.
        /// </summary>
        /// <param name="salesData">The item to create.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A newly created SalesData.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  api/buyers
        ///     {
        ///         "id": 0,
        ///         "productId": 0,
        ///         "productQuantity": 0,
        ///         "productIdAmount": 0,
        ///         "saleId": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SalesDataCreateModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] SalesDataCreateModel salesData,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.InsertItem, "Inserting item {Name}", nameof(salesData));

            if (salesData == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemIsNullBadRequest,
                    "Item {SalesData} is null. BAD REQUEST",
                    nameof(salesData));

                return BadRequest(); // 400 Bad request
            }

            var addedSalesData = await _repository.CreateAsync(salesData, cancellationToken);

            if (addedSalesData == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemNotAddedBadRequest,
                    "Item {Buyer} is not added. BAD REQUEST",
                    nameof(addedSalesData));

                return BadRequest("Repository failed to create SalesData.");
            }

            _logger.LogInformation(LoggingEventIds.InsertItem, "Item {Id} Created", addedSalesData.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetSalesDataAsync),
                routeValues: new { id = addedSalesData.Id },
                value: addedSalesData);
        }

        // PUT: api/salesData/[id]
        // BODY: SalesData (JSON, XML)
        /// <summary>
        /// Update a existing SalesData.
        /// </summary>
        /// <param name="id">The ID of the SalesData to update.</param>
        /// <param name="salesData">SalesData object that needs to be added.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/salesData/[id]
        ///     {
        ///         "id": 0,
        ///         "productId": 0,
        ///         "productQuantity": 0,
        ///         "productIdAmount": 0,
        ///         "saleId": 0
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
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SalesDataUpdateModel salesData,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.UpdateItem, "Updating item {Name}", nameof(salesData));


            if (salesData == null || salesData.Id != id)
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

            var updatedSalesData = await _repository.UpdateAsync(id, salesData, cancellationToken);

            if (updatedSalesData == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.UpdateItemNotUpdatedBadRequest,
                    "Update({Id}) not updated. BAD REQUEST",
                    id);

                return BadRequest(); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.UpdateItemNotContent, "Item {Id} Updated", id);

            return new NoContentResult(); // 204 No content
        }

        // DELETE: api/salesData/[id]
        /// <summary>
        /// Deletes a specific SalesData.
        /// </summary>
        /// <param name="id">The ID of the SalesData to delete.</param>
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

            if (!deleted.HasValue || !deleted.Value) // short circuit AND
            {
                _logger.LogWarning(
                    LoggingEventIds.DeletedItemNotDeletedBadRequest,
                    "Delete({Id}) NOT DELETED",
                    id);

                return BadRequest($"SalesData {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}