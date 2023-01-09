using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.Controllers;
using SmartCarWashTest.WebApi.DTO.Models.Buyer;
using SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles;
using SmartCarWashTest.WebApi.Repositories.Repositories;
using Xunit;

namespace SmartCarWashTest.Tests
{
    public class SqliteInMemoryBuyersControllerTest : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<SmartCarWashContext> _contextOptions;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly CancellationTokenSource _source = new();

        private IMapper CreateMapper() => _mapperConfiguration.CreateMapper();
        private SmartCarWashContext CreateContext() => new(_contextOptions);

        private ILogger<BuyersController> CreateLogger() => LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole()
                    .AddEventLog();
            })
            .CreateLogger<BuyersController>();

        #region ConstructorAndDispose

        public SqliteInMemoryBuyersControllerTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<SmartCarWashContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new SmartCarWashContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
CREATE VIEW AllResources AS
    SELECT Url
    FROM Buyer;";
                viewCommand.ExecuteNonQuery();
            }

            var buyers = new Buyer[]
            {
                new(1, "Buyer test 1"),
                new(2, "Buyer test 2"),
            };

            context.Set<Buyer>().AddRange(buyers);
            context.SaveChanges();

            // mapper
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<BuyerProfile>());
        }


        public void Dispose() => _connection.Dispose();

        #endregion


        [Fact]
        public async Task GetAllBuyersOkObjectResultAndPresentationsNotNullEndEquals()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);

            // Arrange
            var getBuyersResult = await controller.GetBuyers(_source.Token);

            // Arrange
            var okObjectResult = getBuyersResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var presentations = okObjectResult.Value as IEnumerable<BuyerReadModel>;
            Assert.NotNull(presentations);
            Assert.Collection(presentations,
                b => Assert.Equal("Buyer test 1", b.Name),
                b => Assert.Equal("Buyer test 2", b.Name),
                b => Assert.Equal("Buyer test 3", b.Name));
        }

        [Fact]
        public async Task GetByIdBuyerOkObjectResultAndPresentationNotNullEndEquals()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);

            // Arrange
            var getBuyerResult = await controller.GetBuyer(1, _source.Token);

            // Arrange
            var okObjectResult = getBuyerResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var presentation = okObjectResult.Value as BuyerReadModel;
            Assert.NotNull(presentation);
            Assert.Equal("Buyer test 1", presentation.Name);
        }

        [Fact]
        public async Task CreateBuyerCreatedAtRouteResultAndPresentationNotNullEndEquals()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);
            var createBuyer = new BuyerCreateModel(0, "Buyer test 3");

            // Arrange
            var createResult = await controller.Create(createBuyer, _source.Token);

            // Arrange
            var createdAtRouteResult = createResult as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);

            var presentation = createdAtRouteResult.Value as BuyerReadModel;
            Assert.NotNull(presentation);
            Assert.Equal(createBuyer.Name, presentation.Name);
        }

        [Fact]
        public async Task UpdateBuyerNoContentResultAndGetByIdBuyerOkObjectResultAndPresentationNotNullEndEquals()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);

            var updateBuyer = new BuyerUpdateModel(2, "Buyer test 2 rename to 2");

            // Arrange
            var updateResult = await controller.Update(updateBuyer.Id, updateBuyer, _source.Token);

            // Arrange
            var noContentResult = updateResult as NoContentResult;
            Assert.NotNull(noContentResult);

            // Arrange
            var getResult = await controller.GetBuyer(updateBuyer.Id, _source.Token);

            // Arrange
            var okObjectResult = getResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var presentation = okObjectResult.Value as BuyerReadModel;
            Assert.NotNull(presentation);
            Assert.Equal(updateBuyer.Name, presentation.Name);
        }

        [Fact]
        public async Task DeleteBuyerNoContentResultAndGetByIdBuyerOkObjectResultAndNotFoundResult()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);
            const int id = 3;

            // Arrange
            var deleteResult = await controller.Delete(id, _source.Token);

            // Arrange
            var noContentResult = deleteResult as NoContentResult;
            Assert.NotNull(noContentResult);

            // Arrange
            var getResult = await controller.GetBuyer(id, _source.Token);

            // Arrange
            var notFoundResult = getResult as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }
    }
}