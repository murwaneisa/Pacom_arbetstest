using System.Net.Sockets;
using arbetstest_murwan.Data;
using arbetstest_murwan.Models;
using Microsoft.EntityFrameworkCore;
using Modbus.Device;


namespace arbetstest_murwan.Services.Imp
{
    public class ModbusService : IModbusService
    {

        private readonly string _ip = "127.0.0.1";
        private readonly int _port = 502;
        private readonly byte _slaveId = 1;
        private readonly ApplicationDbContext _context;

        public ModbusService(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetDeviceName(ushort address)
        {
            return address switch
            {
                0 => "Door Sensor",
                1 => "Fire Alarm",
                2 => "Siren",
                _ => $"Device {address}"
            };
        }

        /// <summary>
        /// Creates and connects a Modbus master client for TCP/IP communication.
        /// </summary>
        /// <returns>An instance of ModbusIpMaster ready for communication.</returns>
        private async Task<IModbusMaster> CreateModbusMasterAsync()
        {
            var client = new TcpClient();
            await client.ConnectAsync(_ip, _port);
            return ModbusIpMaster.CreateIp(client);
        }

        /// <summary>
        /// Reads a range of coil values from the Modbus simulator.
        /// </summary>
        /// <param name="startAddress">The starting coil address to read from.</param>
        /// <param name="count">Number of coils to read.</param>
        /// <returns>A boolean array of coil values (true = ON, false = OFF).</returns>
        public async Task<bool[]> ReadCoilsAsync(ushort startAddress, ushort count)
        {
            var master = await CreateModbusMasterAsync();
            return await Task.Run(() => master.ReadCoils(_slaveId, startAddress, count));
        }

        /// <summary>
        /// Writes a single boolean value to a specific coil address in the Modbus simulator.
        /// </summary>
        /// <param name="address">Coil address to write to.</param>
        /// <param name="value">Boolean value to write (true = ON, false = OFF).</param>
        public async Task WriteCoilAsync(ushort address, bool value)
        {
            var master = await CreateModbusMasterAsync();
            await Task.Run(() => master.WriteSingleCoil(_slaveId, address, value));
        }

        public async Task SyncCoilsToDatabaseAsync(ushort start, ushort count)
        {
            var values = await ReadCoilsAsync(start, count);

            for (ushort i = 0; i < values.Length; i++)
            {
                var existing = await _context.ModbusValues
                    .FirstOrDefaultAsync(m => m.RegisterAddress == (ushort)(start + i));

                if (existing != null)
                {
                    existing.IsOn = values[i];
                    existing.LastUpdated = DateTime.Now;
                }
                else
                {
                    _context.ModbusValues.Add(new ModbusValue
                    {
                        Name = GetDeviceName((ushort)(start + i)),
                        RegisterAddress = (ushort)(start + i),
                        IsOn = values[i],
                        LastUpdated = DateTime.Now
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ModbusValue>> GetAllDevicesAsync()
        {
            return await _context.ModbusValues.ToListAsync();
        }

        /// <summary>
        /// Updates the IsOn state of a Modbus device and writes to the simulator.
        /// </summary>
        /// <param name="id">Device ID in the database.</param>
        /// <param name="newState">New boolean value (true = ON, false = OFF).</param>
        /// <returns>True if the update succeeded; false if the device was not found.</returns>
        public async Task<bool> UpdateDeviceStateAsync(int id, bool newState)
        {
            var device = await _context.ModbusValues.FindAsync(id);
            if (device == null) return false;

            try
            {
                // Write new value to Modbus simulator
                await WriteCoilAsync(device.RegisterAddress, newState);

                // Only update the DB after Modbus write is successful
                device.IsOn = newState;
                device.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle Modbus communication error if needed
                return false;
            }
        }

        /// <summary>
        /// Deletes a Modbus device from the database.
        /// </summary>
        /// <param name="id">ID of the device to delete.</param>
        /// <returns>True if deletion succeeded; false if device was not found.</returns>
        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var device = await _context.ModbusValues.FindAsync(id);
            if (device == null) return false;

            _context.ModbusValues.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
