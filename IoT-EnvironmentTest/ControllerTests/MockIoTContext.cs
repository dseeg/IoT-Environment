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
                Name = "Relay 1",
                Description = "Relay description",
                PhysicalAddress = "A1:B2:C3:D4:E5:F6",
                NetworkAddress = "127.0.0.1",
                Stale = false,
            };

            Relay relay2 = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Name = "Relay 2",
                Description = "Relay description",
                PhysicalAddress = "00:11:22:AA:BB:CC",
                NetworkAddress = "127.0.0.1",
                Stale = false
            };

            Device device1 = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Name = "Device 1",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                Active = true,
                RelayNavigation = relay1
            };

            Device device2 = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Name = "Device 2",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/2",
                Active = true,
                RelayNavigation = relay1
            };

            Device device3 = new()
            {
                DateRegistered = new DateTime(2021, 7, 1),
                Name = "Device 1",
                Description = "Device description",
                ConnectionType = "Connection type",
                Address = "/addr/1",
                Active = true,
                RelayNavigation = relay2
            };

            Report report1 = new()
            {
                Posted = new DateTime(2021, 7, 1),
                Value = 100.0M,
                DeviceNavigation = device1,
                DataType = 1
            };

            Report report2 = new()
            {
                Posted = new DateTime(2021, 7, 1),
                Value = 100.0M,
                DeviceNavigation = device1,
                DataType = 2
            };

            Report report3 = new()
            {
                Posted = new DateTime(2021, 7, 1),
                Value = 100.0M,
                DeviceNavigation = device2,
                DataType = 1
            };

            Report report4 = new()
            {
                Posted = new DateTime(2021, 7, 1),
                Value = 100.0M,
                DeviceNavigation = device3,
                DataType = 2
            };

            context.AddRange(relay1, relay2, device1, device2, device3);

            context.SaveChanges();
        }
    }
}
