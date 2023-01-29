using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.Logger.Events;

namespace SmartCarWashTest.CRUD.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SalesPointsController : ControllerBase
    {
        private readonly ILogger<SalesPointsController> _logger;
        private readonly ISalesPointCacheRepository _repository;

        public SalesPointsController(ILogger<SalesPointsController> logger, ISalesPointCacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/salesPoints
        // this will always return a list of customers (but it might be empty)
        /// <summary>
        /// Get a list of SalesPoint.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a list of SalesPoint.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SalesPointReadModel>))]
        public async Task<IActionResult> GetSalesPointAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.ListItems, "Listing all items");

            var collections = await _repository.RetrieveAllAsync(cancellationToken);

            return Ok(collections);
        }


        // GET: api/salesPoints/[id]
        /// <summary>
        /// Get SalesPoint by ID.
        /// </summary>
        /// <param name="id">The ID of the SalesPoint to get.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a item of SalesPoint.</returns>
        /// <response code="404">If the item is not found.</response>
        [HttpGet("{id:int}", Name = nameof(GetSalesPointAsync))] // named route
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesPointReadModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSalesPointAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.GetItem, "Getting item {Id}", id);

            var salesPoint = await _repository.RetrieveAsync(id, cancellationToken);

            if (salesPoint != null)
                return Ok(salesPoint); // 200 OK with SalesPoint in body

            _logger.LogWarning(LoggingEventIds.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

            return NotFound(); // 404 Resource not found
        }

        // POST: api/salesPoints
        // BODY: SalesPoint (JSON, XML)
        /// <summary>
        /// Create a SalesPoint.
        /// </summary>
        /// <param name="salesPoint">The item to create.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A newly created SalesPoint.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  api/salesPoints
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SalesPointCreateModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] SalesPointCreateModel salesPoint,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.InsertItem, "Inserting item {Name}", nameof(salesPoint));

            if (salesPoint == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemIsNullBadRequest,
                    "Item {SalesPoint} is null. BAD REQUEST",
                    nameof(salesPoint));

                return BadRequest(); // 400 Bad request
            }

            var addedSalesPoint = await _repository.CreateAsync(salesPoint, cancellationToken);

            if (addedSalesPoint == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemNotAddedBadRequest,
                    "Item {Buyer} is not added. BAD REQUEST",
                    nameof(addedSalesPoint));

                return BadRequest("Repository failed to create SalesPoint.");
            }

            _logger.LogInformation(LoggingEventIds.InsertItem, "Item {Id} Created", addedSalesPoint.Id);


            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetSalesPointAsync),
                routeValues: new { id = addedSalesPoint.Id },
                value: addedSalesPoint);
        }

        // PUT: api/salesPoints/[id]
        // BODY: SalesPoint (JSON, XML)
        /// <summary>
        /// Update a existing SalesPoint.
        /// </summary>
        /// <param name="id">The ID of the SalesPoint to update.</param>
        /// <param name="salesPoint">SalesPoint object that needs to be added.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/salesPoints/[id]
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
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SalesPointUpdateModel salesPoint,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.UpdateItem, "Inserting item {Name}", nameof(salesPoint));

            if (salesPoint == null || salesPoint.Id != id)
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

            var updatedSalesPoint = await _repository.UpdateAsync(id, salesPoint, cancellationToken);

            if (updatedSalesPoint == null)
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

        // DELETE: api/salesPoints/[id]
        /// <summary>
        /// Deletes a specific SalesPoint.
        /// </summary>
        /// <param name="id">The ID of the SalesPoint to delete.</param>
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

                return BadRequest($"SalesPoint {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}