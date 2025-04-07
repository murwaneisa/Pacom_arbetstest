using arbetstest_murwan.Models;

/// <summary>
/// Interface for handling Modbus device operations including reading/writing coils,
/// synchronizing with devices, and managing device states in the database.
/// </summary>
namespace arbetstest_murwan.Services
{
    public interface IModbusService
    {
        /// <summary>
        /// Reads multiple coil values from Modbus devices
        /// </summary>
        /// <param name="startAddress">Starting address of the coils to read</param>
        /// <param name="count">Number of coils to read</param>
        /// <returns>Array of boolean values representing coil states</returns>
        Task<bool[]> ReadCoilsAsync(ushort startAddress, ushort count);

        /// <summary>
        /// Writes a single coil value to a Modbus device
        /// </summary>
        /// <param name="address">Address of the coil to write to</param>
        /// <param name="value">Value to write (true = ON, false = OFF)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task WriteCoilAsync(ushort address, bool value);

        /// <summary>
        /// Synchronizes coil values from Modbus devices to the database
        /// </summary>
        /// <param name="start">Starting address of the coils to sync</param>
        /// <param name="count">Number of coils to sync</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SyncCoilsToDatabaseAsync(ushort start, ushort count);

        /// <summary>
        /// Retrieves all Modbus devices from the database
        /// </summary>
        /// <returns>List of all Modbus devices with their current states</returns>
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