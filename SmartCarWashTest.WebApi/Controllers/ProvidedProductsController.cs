using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Logger;
using SmartCarWashTest.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.WebApi.Repositories.Interfaces;

namespace SmartCarWashTest.WebApi.Controllers
{
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProvidedProductReadModel>))]
        public async Task<IActionResult> GetProvidedProducts(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Listing all items");

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
        [HttpGet("{id:int}", Name = nameof(GetProvidedProduct))] // named route
        [ProducesResponseType(200, Type = typeof(ProvidedProductReadModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProvidedProduct(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting item {Id}", id);

            var providedProduct = await _repository.RetrieveAsync(id, cancellationToken);

            if (providedProduct != null)
                return Ok(providedProduct); // 200 OK with ProvidedProduct in body

            _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

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
        [ProducesResponseType(201, Type = typeof(ProvidedProductCreateModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProvidedProductCreateModel providedProduct,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Inserting item {Name}", nameof(providedProduct));


            if (providedProduct == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemIsNullBadRequest,
                    "Item {ProvidedProduct} is null. BAD REQUEST",
                    nameof(providedProduct));

                return BadRequest(); // 400 Bad request
            }

            var addedProvidedProduct = await _repository.CreateAsync(providedProduct, cancellationToken);

            if (addedProvidedProduct == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemNotAddedBadRequest,
                    "Item {Buyer} is not added. BAD REQUEST",
                    nameof(addedProvidedProduct));

                return BadRequest("Repository failed to create ProvidedProduct.");
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Item {Id} Created", addedProvidedProduct.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetProvidedProduct),
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
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ProvidedProductUpdateModel providedProduct,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Updating item {Name}", nameof(providedProduct));

            if (providedProduct == null || providedProduct.Id != id)
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

            var updateProvidedProduct = await _repository.UpdateAsync(id, providedProduct, cancellationToken);

            if (updateProvidedProduct == null)
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

                return BadRequest($"ProvidedProduct {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEvents.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}