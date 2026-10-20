using System;
using System.Dynamic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PulseLogicHMR.Controllers.Clinical;
using PulseLogicHMR.Core.Configuration;
using Xunit;

namespace PulseLogicHMR.Tests.Controllers.Clinical
{
    public class ScribeControllerTests
    {
        private readonly Mock<IOptions<MyConfiguration>> _mockConfig;
        private readonly ScribeController _controller;

        public ScribeControllerTests()
        {
            _mockConfig = new Mock<IOptions<MyConfiguration>>();
            _mockConfig.Setup(x => x.Value).Returns(new MyConfiguration { Connectionstring = "test" });
            _controller = new ScribeController(_mockConfig.Object);
        }

        [Fact]
        public void Transcribe_ValidTextWithoutFever_ReturnsAcutePharyngitis()
        {
            // Arrange
            dynamic payload = new ExpandoObject();
            payload.text = "sore throat";

            // Act
            var result = _controller.Transcribe(payload) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var json = JsonSerializer.Serialize(result.Value);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            Assert.Equal("OK", root.GetProperty("Status").GetString());
            var results = root.GetProperty("Results");
            Assert.Equal("J02.9", results.GetProperty("ICD10Code").GetString());
            Assert.Equal("Acute pharyngitis, unspecified", results.GetProperty("ICD10Description").GetString());
            Assert.Equal("Patient presents with sore throat. AI processed.", results.GetProperty("StructuredNote").GetString());
        }

        [Fact]
        public void Transcribe_ValidTextWithFever_ReturnsFever()
        {
            // Arrange
            dynamic payload = new ExpandoObject();
            payload.text = "I have a Fever and chills";

            // Act
            var result = _controller.Transcribe(payload) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var json = JsonSerializer.Serialize(result.Value);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            Assert.Equal("OK", root.GetProperty("Status").GetString());
            var results = root.GetProperty("Results");
            Assert.Equal("R50.9", results.GetProperty("ICD10Code").GetString());
            Assert.Equal("Fever, unspecified", results.GetProperty("ICD10Description").GetString());
            Assert.Equal("Patient presents with I have a Fever and chills. AI processed.", results.GetProperty("StructuredNote").GetString());
        }

        [Fact]
        public void Transcribe_NullText_ReturnsAcutePharyngitisWithEmptyNote()
        {
            // Arrange
            dynamic payload = new ExpandoObject();
            payload.text = null;

            // Act
            var result = _controller.Transcribe(payload) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var json = JsonSerializer.Serialize(result.Value);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            Assert.Equal("OK", root.GetProperty("Status").GetString());
            var results = root.GetProperty("Results");
            Assert.Equal("J02.9", results.GetProperty("ICD10Code").GetString());
            Assert.Equal("Patient presents with . AI processed.", results.GetProperty("StructuredNote").GetString());
        }
    }
}
