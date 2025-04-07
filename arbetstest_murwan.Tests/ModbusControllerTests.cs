using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using arbetstest_murwan.Controllers;
using arbetstest_murwan.Models;
using arbetstest_murwan.Services;
using arbetstest_murwan.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arbetstest_murwan.Tests.Controllers
{
    /// <summary>
    /// Unit tests for the <see cref="ModbusController"/> class.
    /// These tests verify controller actions related to retrieving, updating, and deleting Modbus devices.
    /// </summary>
    public class ModbusControllerTests
    {
        private readonly Mock<IModbusService> _mockService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ModbusController _controller;

        public ModbusControllerTests()
        {
            _mockService = new Mock<IModbusService>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _dbContext = new ApplicationDbContext(options);

            _dbContext.ModbusValues.Add(new ModbusValue { Id = 1, Name = "Device1", IsOn = true });
            _dbContext.SaveChanges();

            _controller = new ModbusController(_dbContext, _mockService.Object);
        }

        /// <summary>
        /// Tests whether <see cref="ModbusController.GetModbusValues"/> returns all Modbus values from the database.
        /// </summary>
        [Fact]
        public async Task GetModbusValues_ReturnsAllValues()
        {
            // Act
            var result = await _controller.GetModbusValues();

            // Assert
            var values = Assert.IsType<ActionResult<IEnumerable<ModbusValue>>>(result);
            Assert.Single(values.Value);
        }

        /// <summary>
        /// Tests whether <see cref="ModbusController.GetAllDevices"/> returns a list of devices via OkObjectResult.
        /// </summary>
        [Fact]
        public async Task GetAllDevices_ReturnsOkResultWithDevices()
        {
            // Arrange
            var devices = new List<ModbusValue> { new ModbusValue { Id = 2, Name = "TestDevice" } };
            _mockService.Setup(s => s.GetAllDevicesAsync()).ReturnsAsync(devices);

            // Act
            var result = await _controller.GetAllDevices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDevices = Assert.IsAssignableFrom<IEnumerable<ModbusValue>>(okResult.Value);
            Assert.Single(returnedDevices);
        }


        /// <summary>
        /// Tests whether <see cref="ModbusController.UpdateDeviceState"/> returns Ok when the update is successful.
        /// </summary>
        [Fact]
        public async Task UpdateDeviceState_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(s => s.UpdateDeviceStateAsync(id, false)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateDeviceState(id, false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("updated", okResult.Value.ToString());
        }


        /// <summary>
        /// Tests whether <see cref="ModbusController.UpdateDeviceState"/> returns NotFound when the update fails.
        /// </summary>
        [Fact]
        public async Task UpdateDeviceState_ReturnsNotFound_WhenFails()
        {
            // Arrange
            int id = 99;
            _mockService.Setup(s => s.UpdateDeviceStateAsync(id, true)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateDeviceState(id, true);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Tests whether <see cref="ModbusController.DeleteDevice"/> returns Ok when the deletion is successful.
        /// </summary>
        [Fact]
        public async Task DeleteDevice_ReturnsOk_WhenSuccessful()
        {
            int id = 1;
            _mockService.Setup(s => s.DeleteDeviceAsync(id)).ReturnsAsync(true);

            var result = await _controller.DeleteDevice(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("deleted", okResult.Value.ToString());
        }

        /// <summary>
        /// Tests whether <see cref="ModbusController.DeleteDevice"/> returns NotFound when the device does not exist.
        /// </summary>
        [Fact]
        public async Task DeleteDevice_ReturnsNotFound_WhenFails()
        {
            int id = 100;
            _mockService.Setup(s => s.DeleteDeviceAsync(id)).ReturnsAsync(false);

            var result = await _controller.DeleteDevice(id);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
