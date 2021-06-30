using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoT_Environment.Models;
using IoT_Environment.DTO;
using Microsoft.Extensions.Logging;
using IoT_Environment.Logging;

namespace IoT_Environment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelaysController : ControllerBase
    {
        private readonly IoTContext _context;
        private readonly ILogger<RelaysController> _logger;


        public RelaysController(IoTContext context, ILogger<RelaysController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Relays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relay>>> GetRelays()
        {
            _logger.LogInformation(ApiEventIds.ReadAllRelays, "Getting all Relays");
            List<Relay> relays = await _context.Relays.ToListAsync();

            _logger.LogInformation(ApiEventIds.ReadAllRelays, "Found {Count} Relays", relays.Count);
            return relays;
        }

        // GET: api/Relays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relay>> GetRelay(int id)
        {
            _logger.LogInformation(ApiEventIds.ReadRelay, "Searching for Relay with Id {Id}", id);
            var relay = await _context.Relays.FindAsync(id);

            if (relay == null)
            {
                _logger.LogInformation(ApiEventIds.ReadRelay, "Could not find Relay {Id}", id);
                return NotFound($"Could not find Relay with Id {id}");
            }

            _logger.LogInformation(ApiEventIds.ReadRelay, "Found Relay {Id}: {Address}", relay.Id, relay.PhysicalAddress);
            return relay;
        }

        // PUT: api/Relays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelay(int id, RelayRequest request)
        {
            _logger.LogInformation(ApiEventIds.UpdateRelay, "Starting update for Relay Id {Id}", request.PhysicalAddress, id);

            if (id != request.Id)
            {
                _logger.LogInformation(ApiEventIds.UpdateRelay, "Relay update failed -- Id mismatch: {ResourceId}, {RequestId}", id, request.Id);
                return BadRequest($"Request Id mismatch: {id}, {request.Id}");
            }

            Relay relay = _context.Relays.Find(id);
            if (relay == null)
            {
                _logger.LogInformation(ApiEventIds.UpdateRelay, "Could not find Relay {Id}", id);
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
                _logger.LogWarning(ApiEventIds.UnknownException, ex, "Exception occured while saving changes from PutRelay for Relay {Address}", relay.PhysicalAddress);
                return BadRequest("An unknown error occured while processing the request");
            }

            _logger.LogInformation(ApiEventIds.UpdateRelay, "Update for Relay {Id} complete", id);
            return NoContent();
        }

        // POST: api/Relays
        [HttpPost]
        public async Task<ActionResult<Relay>> PostRelay(RelayRequest request)
        {
            _logger.LogInformation(ApiEventIds.CreateRelay, "Starting Relay registration for {Address}", request.PhysicalAddress);
            if (_context.Relays.Any(r => r.PhysicalAddress == request.PhysicalAddress))
            {
                _logger.LogInformation(ApiEventIds.CreateRelay, "Failed registering Relay: {Address} already exists", request.PhysicalAddress);
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
                _logger.LogWarning(ApiEventIds.UnknownException, ex, "Exception occured while saving changes from PostRelay for Relay {Address}", relay.PhysicalAddress);
                return BadRequest("An unknown error occured while processing the request");
            }

            _logger.LogInformation(ApiEventIds.CreateRelay, "Successfully created Relay {Address} with Id {Id}", relay.PhysicalAddress, relay.Id);
            return CreatedAtAction("GetRelay", new { id = relay.Id }, relay);
        }

        // DELETE: api/Relays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelay(int id)
        {
            _logger.LogInformation(ApiEventIds.DeleteRelay, "Starting delete for Relay Id {Id}", id);

            var relay = await _context.Relays.FindAsync(id);
            if (relay == null)
            {
                _logger.LogInformation(ApiEventIds.DeleteRelay, "Could not find Relay {Id}", id);
                return NotFound($"Could not find Relay with Id {id}");
            }

            _context.Relays.Remove(relay);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ApiEventIds.UnknownException, ex, "Exception occured while saving changes from DeleteRelay for Relay {Address}", relay.PhysicalAddress);
                return BadRequest("An unknown error occured while processing the request");
            }

            _logger.LogInformation(ApiEventIds.DeleteRelay, "Successfully deleted Relay {Address} with Id {Id}", relay.PhysicalAddress, relay.Id);
            return NoContent();
        }
    }
}
