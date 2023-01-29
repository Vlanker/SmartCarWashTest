using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;
using SmartCarWashTest.CRUD.WebApi.DTOs.Events;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.EventBus.Abstractions.EventHandlers;
using SmartCarWashTest.EventBus.Events;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.IntegrationEvents.EventHandlers
{
    public class SaleCreateIntegrationEventHandler : IIntegrationEventHandler<SalePublishedIntegrationEvent>
    {
        private readonly ILogger<SaleCreateIntegrationEventHandler> _logger;
        private readonly IBuyerCacheRepository _buyerCacheRepository;
        private readonly ISaleCacheRepository _saleCacheRepository;
        private readonly ISalesDataCacheRepository _salesDataCacheRepository;
        private readonly ISalesPointCacheRepository _salesPointCacheRepository;
        private readonly IProvidedProductCacheRepository _providedProductCacheRepository;
        private readonly IMapper _mapper;

        public SaleCreateIntegrationEventHandler(ILogger<SaleCreateIntegrationEventHandler> logger,
            ISaleCacheRepository saleCacheRepository,
            ISalesDataCacheRepository salesDataCacheRepository,
            IBuyerCacheRepository buyerCacheRepository,
            ISalesPointCacheRepository salesPointCacheRepository,
            IProvidedProductCacheRepository providedProductCacheRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _buyerCacheRepository = buyerCacheRepository ??
                                    throw new ArgumentNullException(nameof(buyerCacheRepository));
            _saleCacheRepository = saleCacheRepository ?? throw new ArgumentNullException(nameof(saleCacheRepository));
            _salesDataCacheRepository = salesDataCacheRepository ??
                                        throw new ArgumentNullException(nameof(salesDataCacheRepository));
            _salesPointCacheRepository = salesPointCacheRepository ??
                                         throw new ArgumentNullException(nameof(salesPointCacheRepository));
            _providedProductCacheRepository = providedProductCacheRepository ??
                                              throw new ArgumentNullException(nameof(providedProductCacheRepository));
        }

        public async Task Handle(SalePublishedIntegrationEvent @event)
        {
            _logger.LogInformation(
                "----- Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
                @event.Id, @event);

            var cancellationTokenSource = new CancellationTokenSource();
            var buyer = await GetBuyer(@event, cancellationTokenSource.Token) ??
                        await CreateBuyer(@event, cancellationTokenSource.Token);

            var salesPoint = await GetSalesPoint(@event, cancellationTokenSource.Token);

            if (salesPoint is null)
            {
                return;
            }

            var createdSale = await CreateSale(@event, buyer, cancellationTokenSource.Token);
            await HandelSalesData(@event, createdSale, salesPoint, cancellationTokenSource.Token);
        }

        /// <summary>
        /// Get SalesPoint from IntegrationEvent to repository. 
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        /// <returns> <see cref="SalesPointReadModel"/>. </returns>
        private async Task<SalesPointReadModel> GetSalesPoint(SalePublishedIntegrationEvent @event,
            CancellationToken cancellationToken)
        {
            var salesPoint = await _salesPointCacheRepository.RetrieveAsync(@event.SalesPointId, cancellationToken);

            if (salesPoint is not null)
            {
                return salesPoint;
            }

            _logger.LogInformation(
                "----- Integration event: {IntegrationEventId} - ({@IntegrationEvent}){NewLine}" +
                "NOT FOUND SalesPoint by ID: {SalePointId}. Can't create Sale",
                @event.Id, @event, Environment.NewLine, @event.SalesPointId);

            return null;
        }

        /// <summary>
        /// Handel SalesData.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="sale"> <see cref="SaleReadModel"/>. </param>
        /// <param name="salesPoint"> <see cref="SalesPointReadModel"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        private async Task HandelSalesData(SalePublishedIntegrationEvent @event, IHaveIdentifier sale,
            SalesPointReadModel salesPoint, CancellationToken cancellationToken)
        {
            foreach (var salesDataPublishedIntegrationEvent in @event.SalesDataPublishedIntegrationEvent)
            {
                if (GetProvidedProduct(@event, salesPoint, salesDataPublishedIntegrationEvent, out var providedProduct))
                {
                    continue;
                }

                var salesDataCreateModel = new SalesDataCreateModel(providedProduct.ProductId,
                    salesDataPublishedIntegrationEvent.ProductQuantity,
                    salesDataPublishedIntegrationEvent.ProductIdAmount,
                    sale.Id);

                if (CheckQuantityInProvidedProduct(@event, providedProduct, salesDataCreateModel))
                {
                    continue;
                }

                await CreateSalesData(@event, salesDataCreateModel, cancellationToken);
                await UpdateProvidedProduct(providedProduct, salesDataCreateModel, cancellationToken);
            }
        }

        /// <summary>
        /// Update ProvidedProduct to repository.
        /// </summary>
        /// <param name="providedProduct"> <see cref="ProvidedProductReadModel"/>. </param>
        /// <param name="salesDataCreateModel"> <see cref="SalesDataCreateModel"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        private async Task UpdateProvidedProduct(ProvidedProductReadModel providedProduct,
            SalesDataCreateModel salesDataCreateModel, CancellationToken cancellationToken)
        {
            var providedProductUpdateModel = _mapper.Map<ProvidedProductUpdateModel>(providedProduct) with
            {
                ProductQuantity = providedProduct.ProductQuantity - salesDataCreateModel.ProductQuantity
            };

            await _providedProductCacheRepository.UpdateAsync(providedProduct.Id, providedProductUpdateModel,
                cancellationToken);
        }

        /// <summary>
        /// Create SalesData from IntegrationEvent to repository.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="salesDataCreateModel"> <see cref="SalesDataCreateModel"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        private async Task CreateSalesData(IntegrationEvent @event, SalesDataCreateModel salesDataCreateModel,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "----- Integration event: {IntegrationEventId} - ({@IntegrationEvent}){NewLine}" +
                "CREATE new SalesData from IntegrationEvent ({SalesDataCreateModel})",
                @event.Id, @event, Environment.NewLine, salesDataCreateModel);

            await _salesDataCacheRepository.CreateAsync(salesDataCreateModel, cancellationToken);
        }

        /// <summary>
        /// Check quantity in ProvidedProduct.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="providedProduct"> <see cref="ProvidedProductReadModel"/>. </param>
        /// <param name="salesDataCreateModel"> <see cref="SalesDataCreateModel"/>. </param>
        /// <returns></returns>
        private bool CheckQuantityInProvidedProduct(IntegrationEvent @event,
            ProvidedProductReadModel providedProduct, SalesDataCreateModel salesDataCreateModel)
        {
            var productQuantity = providedProduct.ProductQuantity - salesDataCreateModel.ProductQuantity;

            if (productQuantity >= 0)
            {
                return false;
            }

            _logger.LogInformation(
                "----- Integration event: {IntegrationEventId} - ({@IntegrationEvent}){NewLine}" +
                "ProvidedProduct ({SalesDataCreateModel}) DON'T HAVE the right quantity of " +
                "Product ({ProductQuantity}). Can't CREATE new SalesData",
                @event.Id, @event, Environment.NewLine, salesDataCreateModel, providedProduct.ProductQuantity);

            return true;
        }

        /// <summary>
        /// Get ProvidedProduct.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="salesPoint"> <see cref="SalesPointReadModel"/>. </param>
        /// <param name="internalSalesDataPublishedIntegrationEvent"> <see cref="InternalSalesDataPublishedIntegrationEvent"/>. </param>
        /// <param name="providedProduct"> <see cref="ProvidedProductReadModel"/>. </param>
        /// <returns></returns>
        private bool GetProvidedProduct(IntegrationEvent @event, SalesPointReadModel salesPoint,
            InternalSalesDataPublishedIntegrationEvent internalSalesDataPublishedIntegrationEvent,
            out ProvidedProductReadModel providedProduct)
        {
            providedProduct = salesPoint.ProvidedProducts.FirstOrDefault(model =>
                model.ProductId == internalSalesDataPublishedIntegrationEvent.ProductId);

            if (providedProduct is not null)
            {
                return false;
            }

            _logger.LogInformation(
                "----- Integration event: {IntegrationEventId} - ({@IntegrationEvent}){NewLine}" +
                "NOT FOUND Product in ProvidedProduct on SalesPoint ({SalesPoint}) from IntegrationEvent",
                @event.Id, @event, Environment.NewLine, salesPoint);

            return true;
        }

        /// <summary>
        /// Create Sale from IntegrationEvent to repository.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="buyer"> <see cref="BuyerReadModel"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        /// <returns></returns>
        private async Task<SaleReadModel> CreateSale(SalePublishedIntegrationEvent @event, IHaveIdentifier buyer,
            CancellationToken cancellationToken)
        {
            var saleCreateModel = new SaleCreateModel(@event.SalesPointId, buyer?.Id, @event.TotalAmount);

            _logger.LogInformation(
                "----- Integration event: {IntegrationEventId} - ({@IntegrationEvent}){NewLine}" +
                "CREATE Sale from IntegrationEvent ({SaleCreateModel})",
                @event.Id, @event, Environment.NewLine, saleCreateModel);

            return await _saleCacheRepository.CreateAsync(saleCreateModel, cancellationToken);
        }

        /// <summary>
        /// Get or create Buyer.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        /// <returns></returns>
        private async Task<BuyerReadModel> GetBuyer(SalePublishedIntegrationEvent @event,
            CancellationToken cancellationToken)
        {
            if (@event.BuyerId == null)
            {
                return null;
            }

            return await _buyerCacheRepository.RetrieveAsync(@event.BuyerId.Value, cancellationToken);
        }

        /// <summary>
        /// Create Buyer from IntegrationEvent to repository.
        /// </summary>
        /// <param name="event"> <see cref="SalePublishedIntegrationEvent"/>. </param>
        /// <param name="cancellationToken"> <see cref="CancellationToken"/>. </param>
        /// <returns> <see cref="BuyerReadModel"/>. </returns>
        private async Task<BuyerReadModel> CreateBuyer(SalePublishedIntegrationEvent @event,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(@event.BuyerName))
            {
                return null;
            }

            var buyerCreateModel = new BuyerCreateModel(@event.BuyerName);

            _logger.LogInformation(
                "----- Integration event: {IntegrationEventId} - ({@IntegrationEvent}){NewLine}" +
                "CREATE Buyer from IntegrationEvent ({BuyerCreateModel})",
                @event.Id, @event, Environment.NewLine, buyerCreateModel);

            return await _buyerCacheRepository.CreateAsync(buyerCreateModel, cancellationToken);
        }
    }
}