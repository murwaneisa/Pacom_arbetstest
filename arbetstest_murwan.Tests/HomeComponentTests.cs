using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using arbetstest_murwan.Models;
using arbetstest_murwan.Components.Pages;
using RichardSzalay.MockHttp;

namespace arbetstest_murwan.BUnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Home"/> Blazor component using bUnit.
    /// These tests verify rendering behavior based on different device states.
    /// </summary>
    public class HomeComponentTests : TestContext
    {
        /// <summary>
        /// Verifies that the <see cref="Home"/> component displays a message
        /// when no devices are returned from the API (empty array).
        /// </summary>
        [Fact]
        public void Home_ShouldRenderLoading_WhenDevicesNull()
        {
            // Arrange
            var mockHandler = new RichardSzalay.MockHttp.MockHttpMessageHandler();
            mockHandler.When("/api/Modbus/devices")
                       .Respond(HttpStatusCode.OK, "application/json", "[]");

            Services.AddScoped(sp => new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("http://localhost")
            });

            // Act
            var cut = RenderComponent<Home>();

            // Assert
            cut.Markup.Contains("No devices found.");
        }


        /// <summary>
        /// Verifies that the <see cref="Home"/> component correctly renders a list of devices
        /// when a valid JSON response is returned from the API.
        /// </summary>
        [Fact]
        public void Home_ShouldRenderDevices()
        {
            // Arrange
            var device = new ModbusValue
            {
                Id = 1,
                Name = "Test Device",
                IsOn = true,
                RegisterAddress = 5,
                LastUpdated = DateTime.Now
            };

            var deviceJson = JsonSerializer.Serialize(new[] { device });

            var mockHandler = new RichardSzalay.MockHttp.MockHttpMessageHandler();
            mockHandler.When("/api/Modbus/devices")
                       .Respond(HttpStatusCode.OK, "application/json", deviceJson);

            Services.AddScoped(sp => new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("http://localhost")
            });

            // Act
            var cut = RenderComponent<Home>();

            // Assert
            cut.Markup.Contains("Test Device");
            cut.Markup.Contains("Toggle OFF");
        }
    }
}
