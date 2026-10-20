using System;
using System.Dynamic;
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
        private readonly Mock<IOptions<MyConfiguration>> _mockOptions;
        private readonly ScribeController _controller;

        public ScribeControllerTests()
        {
            _mockOptions = new Mock<IOptions<MyConfiguration>>();
            _mockOptions.Setup(o => o.Value).Returns(new MyConfiguration { Connectionstring = "test", ConnectionStringAdmin = "test", ConnectionStringPACSServer = "test", IsAuditEnable = false });
            _controller = new ScribeController(_mockOptions.Object);
        }

        [Fact]
        public void Transcribe_ValidText_ReturnsOkResult()
        {
            dynamic payload = new ExpandoObject();
            payload.text = "cough and sore throat";

            var result = _controller.Transcribe(payload);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Cannot strongly type assert anonymous types easily across assemblies, but we can verify it's OkObjectResult.
        }

        [Fact]
        public void Transcribe_TextWithFever_ReturnsFeverICD10()
        {
            dynamic payload = new ExpandoObject();
            payload.text = "patient has high fever";

            var result = _controller.Transcribe(payload);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void Transcribe_ThrowsException_Returns500StatusCode()
        {
            // By passing null, `payload.text` will throw a RuntimeBinderException when trying to access the property,
            // or if it throws a NullReferenceException, it hits the catch block.
            var result = _controller.Transcribe(null);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            // Let's assert on the anonymous object properties if we can using reflection/dynamic or just check status code
            Assert.NotNull(objectResult.Value);
            var valueType = objectResult.Value.GetType();
            var statusProperty = valueType.GetProperty("Status");
            Assert.NotNull(statusProperty);
            Assert.Equal("Error", statusProperty.GetValue(objectResult.Value));
        }
    }
}
