using IoT_Environment.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace IoT_EnvironmentTest.ControllerTests
{
    public class RelaysControllerTest
    {
        protected DbContextOptions<IoTContext> ContextOptions { get; }

        protected RelaysControllerTest(DbContextOptions<IoTContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        private void Seed()
        {
            using var context = new IoTContext(ContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Relay relay = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Description = "Relay description",
                Name = "Relay name",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "127.0.0.1",
                Stale = false,
            };

            context.Add(relay);

            context.SaveChanges();
        }
    }
}
