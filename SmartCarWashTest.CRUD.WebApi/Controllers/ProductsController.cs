using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Product;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.Logger.Events;

namespace SmartCarWashTest.CRUD.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductCacheRepository _repository;

        public ProductsController(ILogger<ProductsController> logger, IProductCacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/Products
        // this will always return a list of customers (but it might be empty)
        /// <summary>
        /// Get a list of Products.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a list of Products.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductReadModel>))]
        public async Task<IActionResult> GetProductsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.ListItems, "Listing all items");

            var collections = await _repository.RetrieveAllAsync(cancellationToken);

            return Ok(collections);
        }


        // GET: api/products/[id]
        /// <summary>
        /// Get Product by ID.
        /// </summary>
        /// <param name="id">The ID of the Product to get.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>Return a item of Product.</returns>
        /// <response code="404">If the item is not found.</response>
        [HttpGet("{id:int}", Name = nameof(GetProductAsync))] // named route
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReadModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.GetItem, "Getting item {Id}", id);

            var product = await _repository.RetrieveAsync(id, cancellationToken);

            if (product != null)
                return Ok(product); // 200 OK with Product in body

            _logger.LogWarning(LoggingEventIds.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

            return NotFound(); // 404 Resource not found
        }

        // POST: api/products
        // BODY: product (JSON, XML)
        /// <summary>
        /// Create a Product.
        /// </summary>
        /// <param name="product">The item to create.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A newly created Product.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  api/products
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "price": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductCreateModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] ProductCreateModel product,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.InsertItem, "Inserting item {Name}", nameof(product));


            if (product == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemIsNullBadRequest,
                    "Item {Product} is null. BAD REQUEST",
                    nameof(product));

                return BadRequest(); // 400 Bad request
            }

            var addedProduct = await _repository.CreateAsync(product, cancellationToken);

            if (addedProduct == null)
            {
                _logger.LogWarning(
                    LoggingEventIds.InsertItemNotAddedBadRequest,
                    "Item {Product} is not added. BAD REQUEST",
                    nameof(addedProduct));

                return BadRequest("Repository failed to create Product.");
            }

            _logger.LogInformation(LoggingEventIds.InsertItem, "Item {Id} Created", addedProduct.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetProductAsync),
                routeValues: new { id = addedProduct.Id },
                value: addedProduct);
        }

        // PUT: api/products/[id]
        // BODY: product (JSON, XML)
        /// <summary>
        /// Update a existing Product.
        /// </summary>
        /// <param name="id">The ID of the Product to update.</param>
        /// <param name="product">Buyer object that needs to be added.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The result of an action method.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/products/[id]
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "price": 0
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
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductUpdateModel product,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEventIds.UpdateItem, "Inserting item {Name}", nameof(product));

            if (product == null || product.Id != id)
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

            var updatedProduct = await _repository.UpdateAsync(id, product, cancellationToken);

            if (updatedProduct == null)
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

        // DELETE: api/products/[id]
        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        /// <param name="id">The ID of the Product to delete.</param>
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
                return BadRequest($"Product {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEventIds.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}