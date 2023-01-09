using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Logger;
using SmartCarWashTest.WebApi.DTO.Models.Product;
using SmartCarWashTest.WebApi.Repositories.Interfaces;

namespace SmartCarWashTest.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductReadModel>))]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Listing all items");

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
        [HttpGet("{id:int}", Name = nameof(GetProduct))] // named route
        [ProducesResponseType(200, Type = typeof(ProductReadModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting item {Id}", id);

            var product = await _repository.RetrieveAsync(id, cancellationToken);

            if (product != null)
                return Ok(product); // 200 OK with Product in body

            _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({Id}) NOT FOUND", id);

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
        [ProducesResponseType(201, Type = typeof(ProductCreateModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductCreateModel product,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Inserting item {Name}", nameof(product));


            if (product == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemIsNullBadRequest,
                    "Item {Product} is null. BAD REQUEST",
                    nameof(product));

                return BadRequest(); // 400 Bad request
            }

            var addedProduct = await _repository.CreateAsync(product, cancellationToken);

            if (addedProduct == null)
            {
                _logger.LogWarning(
                    LoggingEvents.InsertItemNotAddedBadRequest,
                    "Item {Product} is not added. BAD REQUEST",
                    nameof(addedProduct));

                return BadRequest("Repository failed to create Product.");
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Item {Id} Created", addedProduct.Id);

            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetProduct),
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
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateModel product,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Inserting item {Name}", nameof(product));

            if (product == null || product.Id != id)
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

            var updatedProduct = await _repository.UpdateAsync(id, product, cancellationToken);

            if (updatedProduct == null)
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
                return BadRequest($"Product {id} was found but failed to delete."); // 400 Bad request
            }

            _logger.LogInformation(LoggingEvents.DeleteItemNotContent, "Item {Id} Deleted", id);

            return NoContent(); // 204 No content
        }
    }
}