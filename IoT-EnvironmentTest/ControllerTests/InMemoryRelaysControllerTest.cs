using IoT_Environment.Controllers;
using IoT_Environment.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Microsoft.AspNetCore.Mvc;
using IoT_Environment.DTO;

namespace IoT_EnvironmentTest.ControllerTests
{
    public class InMemoryRelaysControllerTest : MockIoTContext
    {
        public InMemoryRelaysControllerTest()
            : base(
                new DbContextOptionsBuilder<IoTContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options)
        {
        }

        [Fact]
        public async void Can_Get_Relays()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.GetRelays();

            Assert.NotEmpty(actionResult.Value);
        }

        [Fact]
        public async void Can_Get_Relay()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.GetRelay(1);

            Assert.NotNull(actionResult.Value);
            Assert.Equal(1, actionResult.Value.Id);
        }

        [Fact]
        public async void Get_Relay_NotFound()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.GetRelay(100);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void Can_Put_Relay()
        {
            RelayRequest request = new()
            {
                Id = 1,
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PutRelay(1, request);
            var noContentResult = actionResult as NoContentResult;
            var relay = context.Relays.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Equal(request.NetworkAddress, relay.NetworkAddress);
        }

        [Fact]
        public async void Can_Put_Relay_Null_Name()
        {
            RelayRequest request = new()
            {
                Id = 1,
                Name = null,
                Description = "Relay Description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PutRelay(1, request);
            var noContentResult = actionResult as NoContentResult;
            var relay = context.Relays.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Equal(request.NetworkAddress, relay.NetworkAddress);
        }

        [Fact]
        public async void Can_Put_Relay_Null_Description()
        {
            RelayRequest request = new()
            {
                Id = 1,
                Name = "Relay name",
                Description = null,
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PutRelay(1, request);
            var noContentResult = actionResult as NoContentResult;
            var relay = context.Relays.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Equal(request.NetworkAddress, relay.NetworkAddress);
        }

        [Fact]
        public async void Can_Put_Relay_Null_NetworkAddress()
        {
            RelayRequest request = new()
            {
                Id = 1,
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = null
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PutRelay(1, request);
            var noContentResult = actionResult as NoContentResult;
            var relay = context.Relays.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Equal(request.NetworkAddress, relay.NetworkAddress);
        }

        [Fact]
        public async void Put_Relay_Id_Mismatch_BadRequest()
        {
            RelayRequest request = new()
            {
                Id = 1,
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PutRelay(2, request);
            var badRequestResult = actionResult as BadRequestObjectResult;
            var relay = context.Relays.Find(1);

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.NotEqual(request.NetworkAddress, relay.NetworkAddress);
        }

        [Fact]
        public async void Put_Relay_NotFound()
        {
            RelayRequest request = new()
            {
                Id = 100,
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PutRelay(100, request);
            var notFoundResult = actionResult as NotFoundObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void Can_Post_Relay()
        {
            RelayRequest request = new()
            {
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "00:AA:22:BB:33:CC",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PostRelay(request);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;

            Assert.NotNull(createdAtResult);
            Assert.Equal(201, createdAtResult.StatusCode);
        }

        [Fact]
        public async void Can_Post_Relay_Null_Name()
        {
            RelayRequest request = new()
            {
                Name = null,
                Description = "Relay Description",
                PhysicalAddress = "00:AA:22:BB:33:CC",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PostRelay(request);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;

            Assert.NotNull(createdAtResult);
            Assert.Equal(201, createdAtResult.StatusCode);
        }

        [Fact]
        public async void Can_Post_Relay_Null_Description()
        {
            RelayRequest request = new()
            {
                Name = "Relay name",
                Description = null,
                PhysicalAddress = "00:AA:22:BB:33:CC",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PostRelay(request);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;

            Assert.NotNull(createdAtResult);
            Assert.Equal(201, createdAtResult.StatusCode);
        }

        [Fact]
        public async void Can_Post_Relay_Null_NetworkAddress()
        {
            RelayRequest request = new()
            {
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "00:AA:22:BB:33:CC",
                NetworkAddress = null
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PostRelay(request);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;

            Assert.NotNull(createdAtResult);
            Assert.Equal(201, createdAtResult.StatusCode);
        }

        [Fact]
        public async void Post_Relay_Null_PhysicalAddress_BadRequest()
        {
            RelayRequest request = new()
            {
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = null,
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PostRelay(request);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void Post_Relay_PhysicalAddress_Conflict()
        {
            RelayRequest request = new()
            {
                Name = "Relay name",
                Description = "Relay Description",
                PhysicalAddress = "00:11:22:AA:BB:CC",
                NetworkAddress = "192.168.0.1"
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.PostRelay(request);
            var conflictResult = actionResult.Result as ConflictObjectResult;

            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async void Can_Delete_Relay()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.DeleteRelay(1);
            var noContentResult = actionResult as NoContentResult;
            var relay = context.Relays.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Null(relay);
        }

        [Fact]
        public async void Delete_Relay_NotFound()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new RelaysController(context, NullLogger<RelaysController>.Instance);

            var actionResult = await controller.DeleteRelay(100);
            var notFoundResult = actionResult as NotFoundObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
