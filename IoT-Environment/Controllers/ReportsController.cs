using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoT_Environment.Models;
using IoT_Environment.DTO;
using IoT_Environment.Filters;
using Microsoft.Extensions.Logging;
using IoT_Environment.Logging;

namespace IoT_Environment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IoTContext _context;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IoTContext context, ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataReport>>> GetReports([FromQuery] ReportFilters filters)
        {
            return await _context.Reports
                .Where(r => r.Posted > DateTime.UtcNow.AddMinutes(-(double)filters.LastMinutes))
                .Where(r => string.IsNullOrWhiteSpace(filters.DataType) ||
                       string.Equals(r.DataTypeNavigation.Name, filters.DataType, StringComparison.OrdinalIgnoreCase)) // could InvariantCultureIgnoreCase be better here?
                .OrderBy(r => r.Posted)
                .Select(r => new DataReport
                {
                    // null checks here?
                    DataType = r.DataTypeNavigation.Name,
                    DataUnits = r.DataTypeNavigation.Unit,
                    DeviceName = r.DeviceNavigation.Name,
                    DeviceType = r.DeviceNavigation.ConnectionType,
                    PostedOn = r.Posted,
                    RelayName = r.DeviceNavigation.RelayNavigation.Name,
                    Value = r.Value
                })
                .ToListAsync();
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataReport>> GetReport(int id)
        {
            Report report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            await _context.Entry(report)
                .Reference(r => r.DeviceNavigation)
                .Query()
                .Include(d => d.RelayNavigation)
                .LoadAsync();

            await _context.Entry(report)
                .Reference(r => r.DataTypeNavigation)
                .LoadAsync();

            return new DataReport
            {
                // null checks here?
                DataType = report.DataTypeNavigation.Name,
                DataUnits = report.DataTypeNavigation.Unit,
                DeviceName = report.DeviceNavigation.Name,
                DeviceType = report.DeviceNavigation.ConnectionType,
                PostedOn = report.Posted,
                RelayName = report.DeviceNavigation.RelayNavigation.Name,
                Value = report.Value
            };
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(DeviceData data)
        {
            Relay relay = await _context.Relays.FirstOrDefaultAsync(r => r.PhysicalAddress == data.RelayPhysicalAddress);
            if (relay == null)
            {
                NotFound($"Relay {data.RelayPhysicalAddress} not registered");
            }

            Device device = await _context.Devices.FirstOrDefaultAsync(d => d.Address == data.DeviceAddress && d.RelayNavigation == relay);
            if (device == null)
            {
                NotFound($"Device {data.DeviceAddress} for Relay {data.RelayPhysicalAddress} not found");
            }

            if (relay.NetworkAddress != data.RelayNetworkAddress)
            {
                relay.NetworkAddress = data.RelayNetworkAddress;
                _context.Entry(relay).State = EntityState.Modified;
            }

            if (await _context.DataTypes.FindAsync(data.DataType) == null)
                _context.DataTypes.Add(new() { Id = data.DataType });

            Report report = new()
            {
                DataType = data.DataType,
                Device = device.Id,
                Posted = DateTime.UtcNow,
                Value = data.Value
            };

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ApiEventIds.UnknownException, ex, "Exception occured while saving changes from PostReport with device data {Data}", data);
                return BadRequest("An unknown error occured while processing the request");
            }

            return CreatedAtAction("GetReport", new { id = report.Id }, report);
        }
    }
}
