using arbetstest_murwan.Models;

/// <summary>
/// Interface for handling Modbus device operations.
/// </summary>
namespace arbetstest_murwan.Services
{
    public interface IModbusService
    {
        Task<bool[]> ReadCoilsAsync(ushort startAddress, ushort count);
        Task WriteCoilAsync(ushort address, bool value);
        Task SyncCoilsToDatabaseAsync(ushort start, ushort count);
        Task<List<ModbusValue>> GetAllDevicesAsync();
        /// <summary>
        /// Updates the IsOn state of a Modbus device and writes to the simulator.
        /// </summary>
        /// <param name="id">Device ID in the database.</param>
        /// <param name="newState">New boolean value (true = ON, false = OFF).</param>
        /// <returns>True if the update succeeded; false if the device was not found.</returns>
        Task<bool> UpdateDeviceStateAsync(int id, bool newState);

        /// <summary>
        /// Deletes a Modbus device from the database.
        /// </summary>
        /// <param name="id">ID of the device to delete.</param>
        /// <returns>True if deletion succeeded; false if device was not found.</returns>
        Task<bool> DeleteDeviceAsync(int id);
    }
}
