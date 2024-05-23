using BeatGeneratorAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BeatGeneratorTests
{
    public class BeatGeneratorControllerTests
    {
        [Fact]
        public void GetBeatSequence_ValidInput_ReturnsExpectedSequence()
        {
            // Arrange
            var controller = new BeatGeneratorController();

            // Act
            var result = controller.GetBeatSequence(10, 1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var sequence = result.Value as List<string>;
            Assert.NotNull(sequence);
            Assert.Equal(new List<string>
            {
                "Low Floor Tom", // 10
                "kick",         // 9
                "snare",        // 8
                "Low Floor Tom", // 7
                "kick",         // 6
                "Low Floor Tom", // 5
                "snare",        // 4
                "kick",         // 3
                "Low Floor Tom", // 2
                "Low Floor Tom"  // 1
            }, sequence);
        }

        [Fact]
        public void GetBeatSequence_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new BeatGeneratorController();

            // Act
            var result = controller.GetBeatSequence(1, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Configure_ValidInput_UpdatesConfigurationAsync()
        {
            // Arrange
            var controller = new BeatGeneratorController();

            // Act
            var result = controller.Configure(4, "Bass Drum") as ObjectResult;

            Assert.Equal(200, result.StatusCode);

            // Verify the configuration change
            var sequenceResult = controller.GetBeatSequence(8, 1) as OkObjectResult;
            var sequence = sequenceResult.Value as List<string>;

            Assert.Contains("Bass Drum", sequence);
            Assert.Equal(new List<string>
            {
                "Bass Drum",
                "Low Floor Tom",
                "kick",
                "Low Floor Tom",
                "Bass Drum",
                "kick",
                "Low Floor Tom",
                "Low Floor Tom"
            }, sequence);
        }

        [Fact]
        public void Configure_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new BeatGeneratorController();

            // Act
            var result = controller.Configure(5, "Bass Drum");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Reset_ResetsToDefaultConfiguration()
        {
            // Arrange
            var controller = new BeatGeneratorController();

            // Act
            var result = controller.Reset();            

            // Verify the reset
            var sequenceResult = controller.GetConfiguration() as OkObjectResult;
            var sequence = sequenceResult.Value as Dictionary<int, string>;

            Assert.Equal(new Dictionary<int, string>
            {
                { 1, "Low Floor Tom" },
                { 3, "kick" },
                { 4, "snare" },
                { 12, "Hi-Hat" }
            }, sequence);
        }
    }
}
