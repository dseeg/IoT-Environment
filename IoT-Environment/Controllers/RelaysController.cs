using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoT_Environment.Models;
using IoT_Environment.DTO;

namespace IoT_Environment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelaysController : ControllerBase
    {
        private readonly IoTContext _context;

        public RelaysController(IoTContext context)
        {
            _context = context;
        }

        // GET: api/Relays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relay>>> GetRelays()
        {
            return await _context.Relays.ToListAsync();
        }

        // GET: api/Relays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relay>> GetRelay(int id)
        {
            var relay = await _context.Relays.FindAsync(id);

            if (relay == null)
            {
                return NotFound($"Could not find Relay with Id {id}");
            }

            return relay;
        }

        // PUT: api/Relays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelay(int id, RelayRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest($"Request Id mismatch: {id}, {request.Id}");
            }

            Relay relay = _context.Relays.Find(id);
            if (relay == null)
            {
                return NotFound($"Could not find Relay with Id {id}");
            }

            relay.Name = request.Name;
            relay.Description = request.Description;
            relay.NetworkAddress = request.NetworkAddress;

            // should physical address be read only?
            relay.PhysicalAddress = request.PhysicalAddress;

            _context.Entry(relay).State = EntityState.Modified;

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

        // POST: api/Relays
        [HttpPost]
        public async Task<ActionResult<Relay>> PostRelay(RelayRequest request)
        {
            if (_context.Relays.Any(r => r.PhysicalAddress == request.PhysicalAddress))
            {
                return Conflict($"Relay with physical address {request.PhysicalAddress} already exists");
            }

            Relay relay = new()
            {
                Name = request.Name,
                NetworkAddress = request.NetworkAddress,
                PhysicalAddress = request.PhysicalAddress,
                DateRegistered = DateTime.UtcNow,
                Description = request.Description,
                Stale = false
            };

            _context.Relays.Add(relay);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An unknown error occured while processing the request");
            }

            return CreatedAtAction("GetRelay", new { id = relay.Id }, relay);
        }

        // DELETE: api/Relays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelay(int id)
        {
            var relay = await _context.Relays.FindAsync(id);
            if (relay == null)
            {
                return NotFound($"Could not find Relay with Id {id}");
            }

            _context.Relays.Remove(relay);

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
