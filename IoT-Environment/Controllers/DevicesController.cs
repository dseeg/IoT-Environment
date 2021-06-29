using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoT_Environment.Models;
using IoT_Environment.DTO;
using IoT_Environment.Extensions;

namespace IoT_Environment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IoTContext _context;

        public DevicesController(IoTContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound($"Could not find Device with Id {id}");
            }

            return device;
        }

        // PUT: api/Devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, DeviceRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest($"Request Id mismatch: {id}, {request.Id}");
            }

            Relay relay = await _context.Relays.FirstOrDefaultAsync(r => r.PhysicalAddress == request.RelayPhysicalAddress);
            if (relay == null)
            {
                return NotFound($"Could not find Relay with network address {request.RelayPhysicalAddress}");
            }

            Device device = _context.Devices.Find(id);
            if (device == null)
            {
                return NotFound($"Could not find Device with Id {id}");
            }

            if (relay.TryUpdateNetworkAddress(request.RelayNetworkAddress))
            { 
                _context.Entry(relay).State = EntityState.Modified;
            }

            device.Name = request.Name;
            device.RelayNavigation = relay;
            device.Description = request.Description;
            device.Address = request.Address;
            device.ConnectionType = request.ConnectionType;

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An unknown error occured while processing the request");
            }

            return NoContent();
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(DeviceRequest request)
        {
            Relay relay = await _context.Relays.FirstOrDefaultAsync(r => r.PhysicalAddress == request.RelayPhysicalAddress);
            if (relay == null)
            {
                return NotFound($"Could not find Relay with network address {request.RelayPhysicalAddress}");
            }

            Device device = await _context.Devices.FirstOrDefaultAsync(d => d.Address == request.Address && d.Relay == relay.Id);
            if (device != null)
            {
                return Conflict($"Device with address {request.Address} already exists on Relay {relay.PhysicalAddress}");
            }

            if (relay.NetworkAddress != request.RelayNetworkAddress)
            {
                relay.NetworkAddress = request.RelayNetworkAddress;
                _context.Entry(relay).State = EntityState.Modified;
            }

            device = new Device
            {
                Address = request.Address,
                Name = request.Name,
                Description = request.Description,
                ConnectionType = request.ConnectionType,
                DateRegistered = DateTime.UtcNow,
                Relay = relay.Id,
                Active = true,
            };

            _context.Devices.Add(device);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An unknown error occured while processing the request");
            }

            return CreatedAtAction("GetDevice", new { id = request.Id }, request);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound($"Could not find Device with Id {id}");
            }

            _context.Devices.Remove(device);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An unknown error occured while processing the request");
            }

            return NoContent();
        }
    }
}
