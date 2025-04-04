using arbetstest_murwan.Data;
using arbetstest_murwan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using arbetstest_murwan.Services;
using arbetstest_murwan.Services.Imp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace arbetstest_murwan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModbusController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IModbusService _modbusService;

        /// <summary>
        /// Initializes a new instance of the ModbusValuesController
        /// </summary>
        /// <param name="context">Database context</param>
        public ModbusController(ApplicationDbContext context, IModbusService modbusService)
        {
            _context = context;
            _modbusService = modbusService;
        }

        /// <summary>
        /// Gets all Modbus values
        /// </summary>
        /// <returns>A list of all Modbus values</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModbusValue>>> GetModbusValues()
        {
            return await _context.ModbusValues.ToListAsync();
        }

        [HttpGet("read")]
        public async Task<IActionResult> ReadCoils()
        {
            var result = await _modbusService.ReadCoilsAsync(0,3); // reads coils 0–9
            return Ok(result);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncFromModbus()
        {
            await _modbusService.SyncCoilsToDatabaseAsync(0, 3);
            return Ok("Synced Modbus coils to DB");
        }

        [HttpGet("devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _modbusService.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// Updates only the IsOn value of a Modbus device and synchronizes with the simulator.
        /// </summary>
        /// <param name="id">The ID of the Modbus device to update.</param>
        /// <param name="newState">The new IsOn value (true = ON, false = OFF).</param>
        /// <returns>Returns 200 OK if successful, 404 Not Found if device doesn't exist.</returns>
        [HttpPut("devices/{id}")]
        public async Task<IActionResult> UpdateDeviceState(int id, [FromBody] bool newState)
        {
            var success = await _modbusService.UpdateDeviceStateAsync(id, newState);
            if (!success) return NotFound("Device not found or Modbus write failed.");
            return Ok($"Device {id} updated to {(newState ? "ON" : "OFF")}.");
        }

        /// <summary>
        /// Deletes a Modbus device from the database.
        /// </summary>
        /// <param name="id">The ID of the Modbus device to delete.</param>
        /// <returns>Returns 200 OK if successful, or 404 Not Found if device is missing.</returns>
        [HttpDelete("devices/{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var success = await _modbusService.DeleteDeviceAsync(id);
            if (!success) return NotFound("Device not found.");
            return Ok($"Device {id} deleted.");
        } 


    }
}
