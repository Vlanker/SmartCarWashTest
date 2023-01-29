using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.Logger.Events;

namespace SmartCarWashTest.CRUD.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidedProductsController : ControllerBase
    {
        private readonly ILogger<ProvidedProductsController> _logger;
        private readonly IProvidedProductCacheRepository _repository;

        public ProvidedProductsController(ILogger<ProvidedProductsController> logger,
            IProvidedProductCacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/providedProducts
        // this will always return a list of customers (but it might be empty)
        /// <summary>
        /// Get a list of ProvidedProduct.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a list of ProvidedProduct.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProvidedProductReadModel>))]
        public async Task<IActionResult> GetProvidedProductsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.ListItems, "Listing all items");

            var collections = await _repository.RetrieveAllAsync(cancellationToken);

            return Ok(collections);
        }


        // GET: api/providedProducts/[id]
        /// <summary>
        /// Get ProvidedProduct by ID.
        /// </summary>
        /// <param name="id">The ID of the ProvidedProduct to get.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a item of ProvidedProduct.</returns>
        /// <response code="404">If the item is not found.</response>
        [HttpGet("{id:int}", Name = nameof(GetProvidedProductAsync))] // named route
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProvidedProductReadModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProvidedProductAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.GetItem, "Getting item {Id}", id);

            var providedProduct = await _repository.RetrieveAsync(id, cancellationToken);

            if (providedProduct != null)
                return Ok(providedProduct); // 200 OK with ProvidedProduct in body

            _logger.LogWarning(LoggingEventIds.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

            return NotFound(); // 404 Resource not found
        }

        // POST: api/providedProducts
        // BODY: ProvidedProduct (JSON, XML)
        /// <summary>
        /// Create a ProvidedProduct.
        /// </summary>
        /// <param name="providedProduct">The item to create.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A newly created ProvidedProduct.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  api/providedProducts
        ///     {
        ///        "id": 0,
        ///        "productId": 0,
        ///        "productQuantity": 0,
        ///        "salesPointId": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProvidedProductCreateModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] ProvidedProductCreateModel providedProduct,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.InsertItem, "Inserting item {Name}", nameof(providedProduct));


            if (providedProduct == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemIsNullBadRequest,
                    "Item {ProvidedProduct} is null. BAD REQUEST",
                    nameof(providedProduct));

                return BadRequest(); // 400 Bad request
            }

            var addedProvidedProduct = await _repository.CreateAsync(providedProduct, cancellationToken);

            if (addedProvidedProduct == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemNotAddedBadRequest,
                    "Item {Buyer} is not added. BAD REQUEST",
                    nameof(addedProvidedProduct));

                return BadRequest("Repository failed to create ProvidedProduct.");
            }

            _logger.LogInformation(LoggingEventIds.InsertItem, "Item {Id} Created", addedProvidedProduct.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetProvidedProductAsync),
                routeValues: new { id = addedProvidedProduct.Id },
                value: addedProvidedProduct);
        }

        // PUT: api/providedProducts/[id]
        // BODY: ProvidedProduct (JSON, XML)
        /// <summary>
        /// Update a existing ProvidedProduct.
        /// </summary>
        /// <param name="id">The ID of the Buyer to update.</param>
        /// <param name="providedProduct">ProvidedProduct object that needs to be added.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/providedProducts/[id]
        ///     {
        ///        "id": 0,
        ///        "productId": 0,
        ///        "productQuantity": 0,
        ///        "salesPointId": 0
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
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProvidedProductUpdateModel providedProduct,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.UpdateItem, "Updating item {Name}", nameof(providedProduct));

            if (providedProduct == null || providedProduct.Id != id)
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

            var updateProvidedProduct = await _repository.UpdateAsync(id, providedProduct, cancellationToken);

            if (updateProvidedProduct == null)
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

        // DELETE: api/providedProducts/[id]
        /// <summary>
        /// Deletes a specific ProvidedProduct.
        /// </summary>
        /// <param name="id">The ID of the ProvidedProduct to delete.</param>
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

                return BadRequest($"ProvidedProduct {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}