using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Logger;
using SmartCarWashTest.WebApi.DTO.Models.SalesPoint;
using SmartCarWashTest.WebApi.Repositories.Interfaces;

namespace SmartCarWashTest.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<SalesPointReadModel>))]
        public async Task<IActionResult> GetSalesPoint(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Listing all items");

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
        [HttpGet("{id:int}", Name = nameof(GetSalesPoint))] // named route
        [ProducesResponseType(200, Type = typeof(SalesPointReadModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSalesPoint(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting item {Id}", id);

            var salesPoint = await _repository.RetrieveAsync(id, cancellationToken);

            if (salesPoint != null)
                return Ok(salesPoint); // 200 OK with SalesPoint in body

            _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

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
        [ProducesResponseType(201, Type = typeof(SalesPointCreateModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] SalesPointCreateModel salesPoint,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Inserting item {Name}", nameof(salesPoint));

            if (salesPoint == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemIsNullBadRequest,
                    "Item {SalesPoint} is null. BAD REQUEST",
                    nameof(salesPoint));

                return BadRequest(); // 400 Bad request
            }

            var addedSalesPoint = await _repository.CreateAsync(salesPoint, cancellationToken);

            if (addedSalesPoint == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemNotAddedBadRequest,
                    "Item {Buyer} is not added. BAD REQUEST",
                    nameof(addedSalesPoint));

                return BadRequest("Repository failed to create SalesPoint.");
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Item {Id} Created", addedSalesPoint.Id);


            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetSalesPoint),
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
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] SalesPointUpdateModel salesPoint,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Inserting item {Name}", nameof(salesPoint));

            if (salesPoint == null || salesPoint.Id != id)
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

            var updatedSalesPoint = await _repository.UpdateAsync(id, salesPoint, cancellationToken);

            if (updatedSalesPoint == null)
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

                return BadRequest($"SalesPoint {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEvents.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}