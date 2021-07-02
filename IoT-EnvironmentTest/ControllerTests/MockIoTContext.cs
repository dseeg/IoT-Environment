using IoT_Environment.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace IoT_EnvironmentTest.ControllerTests
{
    public class MockIoTContext
    {
        protected DbContextOptions<IoTContext> ContextOptions { get; }

        protected MockIoTContext(DbContextOptions<IoTContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        private void Seed()
        {
            using var context = new IoTContext(ContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Relay relay1 = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Name = "Relay name",
                Description = "Relay description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "127.0.0.1",
                Stale = false,
            };

            Relay relay2 = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Name = "Relay name",
                Description = "Relay description",
                PhysicalAddress = "00:11:22:AA:BB:CC",
                NetworkAddress = "127.0.0.1",
                Stale = false
            };

            context.AddRange(relay1, relay2);

            context.SaveChanges();
        }
    }
}
