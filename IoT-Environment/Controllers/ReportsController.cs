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

namespace IoT_Environment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IoTContext _context;

        public ReportsController(IoTContext context)
        {
            _context = context;
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
            var report = await _context.Reports.FindAsync(id);

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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(DeviceData report)
        {
            //_context.Reports.Add(report);
            //await _context.SaveChangesAsync();
            //
            //return CreatedAtAction("GetReport", new { id = report.Id }, report);
            return Ok();
        }
    }
}
