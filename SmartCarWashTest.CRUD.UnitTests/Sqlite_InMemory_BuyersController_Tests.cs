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
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.Controllers;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer;
using SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories;
using Xunit;

namespace SmartCarWashTest.CRUD.UnitTests
{
    public class Sqlite_InMemory_BuyersController_Tests : IDisposable
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

        public Sqlite_InMemory_BuyersController_Tests()
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
        public async Task Get_buyers_success()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);

            // Act
            var getBuyersResult = await controller.GetBuyersAsync(_source.Token);

            // Assert
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
        public async Task Get_buyer_by_ID_success()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);

            // Act
            var getBuyerResult = await controller.GetBuyerAsync(1, _source.Token);

            // Assert
            var okObjectResult = getBuyerResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var presentation = okObjectResult.Value as BuyerReadModel;
            Assert.NotNull(presentation);
            Assert.Equal("Buyer test 1", presentation.Name);
        }

        [Fact]
        public async Task Create_buyer_success()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);
            var createBuyer = new BuyerCreateModel("Buyer test 3");

            // Act
            var createResult = await controller.CreateAsync(createBuyer, _source.Token);

            // Assert
            var createdAtRouteResult = createResult as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);

            var presentation = createdAtRouteResult.Value as BuyerReadModel;
            Assert.NotNull(presentation);
            Assert.Equal(createBuyer.Name, presentation.Name);
        }

        [Fact]
        public async Task Update_buyer_success()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);

            var updateBuyer = new BuyerUpdateModel(2, "Buyer test 2 rename to 2");

            // Act
            var updateResult = await controller.UpdateAsync(updateBuyer.Id, updateBuyer, _source.Token);

            // Assert
            var noContentResult = updateResult as NoContentResult;
            Assert.NotNull(noContentResult);

            // Act
            var getResult = await controller.GetBuyerAsync(updateBuyer.Id, _source.Token);

            // Assert
            var okObjectResult = getResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var presentation = okObjectResult.Value as BuyerReadModel;
            Assert.NotNull(presentation);
            Assert.Equal(updateBuyer.Name, presentation.Name);
        }

        [Fact]
        public async Task Delete_buyer_success()
        {
            // Arrange
            var context = CreateContext();
            var mapper = CreateMapper();
            var repository = new BuyerCacheRepository(context, mapper);
            var logger = CreateLogger();
            var controller = new BuyersController(logger, repository);
            const int id = 3;

            // Act
            var deleteResult = await controller.DeleteAsync(id, _source.Token);

            // Assert
            var noContentResult = deleteResult as NoContentResult;
            Assert.NotNull(noContentResult);

            // Act
            var getResult = await controller.GetBuyerAsync(id, _source.Token);

            // Assert
            var notFoundResult = getResult as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }
    }
}