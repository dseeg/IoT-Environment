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
    public class InMemoryDevicesControllerTest : MockIoTContext
    {
        public InMemoryDevicesControllerTest()
            : base(
                new DbContextOptionsBuilder<IoTContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options)
        {
        }

        [Fact]
        public async void Can_Get_Devices()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.GetDevices();

            Assert.NotEmpty(actionResult.Value);
        }

        [Fact]
        public async void Can_Get_Device()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.GetDevice(1);

            Assert.NotNull(actionResult.Value);
            Assert.Equal(1, actionResult.Value.Id);
        }

        [Fact]
        public async void Get_Device_NotFound()
        {
            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.GetDevice(100);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void Can_Put_Device()
        {
            DeviceRequest request = new()
            {
                Id = 1,
                Name = "New device name",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "127.0.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(1, request);
            var noContentResult = actionResult as NoContentResult;
            var device = context.Devices.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Equal(request.Name, device.Name);
        }

        [Fact]
        public async void Can_Put_Device_Udpdate_Relay_NetworkAddress()
        {
            DeviceRequest request = new()
            {
                Id = 1,
                Name = "Device 1",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "192.168.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(1, request);
            var noContentResult = actionResult as NoContentResult;
            var device = context.Devices.Find(1);
            context.Entry(device).Reference(d => d.RelayNavigation).Load();

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Equal(request.RelayNetworkAddress, device.RelayNavigation.NetworkAddress);
        }

        [Fact]
        public async void Can_Put_Device_Null_Name()
        {
            DeviceRequest request = new()
            {
                Id = 1,
                Name = null,
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "127.0.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(1, request);
            var noContentResult = actionResult as NoContentResult;
            var device = context.Devices.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Null(device.Name);
        }

        [Fact]
        public async void Can_Put_Device_Null_Description()
        {
            DeviceRequest request = new()
            {
                Id = 1,
                Name = "Device 1",
                Description = null,
                ConnectionType = "Connection type",
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "127.0.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(1, request);
            var noContentResult = actionResult as NoContentResult;
            var device = context.Devices.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Null(device.Description);
        }

        [Fact]
        public async void Can_Put_Device_Null_ConnectioNType()
        {
            DeviceRequest request = new()
            {
                Id = 1,
                Name = "Device 1",
                Description = "Device description",
                ConnectionType = null,
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "127.0.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(1, request);
            var noContentResult = actionResult as NoContentResult;
            var device = context.Devices.Find(1);

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
            Assert.Null(device.ConnectionType);
        }

        [Fact]
        public async void Put_Device_Id_Mismatch_BadRequest()
        {
            DeviceRequest request = new()
            {
                Id = 1,
                Name = "New device name",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "127.0.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(2, request);
            var badRequestResult = actionResult as BadRequestObjectResult;
            var device = context.Devices.Find(1);

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.NotEqual(request.Name, device.Name);
        }

        [Fact]
        public async void Put_Device_NotFound()
        {
            DeviceRequest request = new()
            {
                Id = 100,
                Name = "New device name",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                RelayPhysicalAddress = "A1:B2:C3:D4:E5:F6",
                RelayNetworkAddress = "127.0.0.1",
            };

            using var context = new IoTContext(ContextOptions);
            var controller = new DevicesController(context, NullLogger<DevicesController>.Instance);

            var actionResult = await controller.PutDevice(100, request);
            var notFoundResult = actionResult as NotFoundObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
