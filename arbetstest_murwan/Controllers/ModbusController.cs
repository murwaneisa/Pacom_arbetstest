using arbetstest_murwan.Data;
using arbetstest_murwan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using arbetstest_murwan.Services;
using arbetstest_murwan.Services.Imp;



namespace arbetstest_murwan.Controllers
{
    /// <summary>
    /// Controller for handling Modbus device operations including reading values, 
    /// synchronizing with Modbus devices, and managing device states.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModbusController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IModbusService _modbusService;

        /// <summary>
        /// Initializes a new instance of the ModbusController
        /// </summary>
        /// <param name="context">Database context for accessing Modbus values</param>
        /// <param name="modbusService">Service for Modbus device operations</param>
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

        /// <summary>
        /// Reads coil values from Modbus devices
        /// </summary>
        /// <returns>Coil values from the Modbus devices</returns>
        [HttpGet("read")]
        public async Task<IActionResult> ReadCoils()
        {
            var result = await _modbusService.ReadCoilsAsync(0,3); // reads coils 0–2
            return Ok(result);
        }

        /// <summary>
        /// Synchronizes coil values from Modbus devices to the database
        /// </summary>
        /// <returns>Confirmation message when sync is complete</returns>
        [HttpPost("sync")]
        public async Task<IActionResult> SyncFromModbus()
        {
            await _modbusService.SyncCoilsToDatabaseAsync(0, 3);
            return Ok("Synced Modbus coils to DB");
        }

        /// <summary>
        /// Retrieves all Modbus devices from the database
        /// </summary>
        /// <returns>List of all Modbus devices</returns>
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
