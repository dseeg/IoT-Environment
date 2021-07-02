using IoT_Environment.Controllers;
using IoT_Environment.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace IoT_EnvironmentTest.ControllerTests
{
    public class InMemoryRelaysControllerTest : RelaysControllerTest
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
    }
}
