using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PulseLogicHMR.Controllers;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Security;
using Xunit;
using Newtonsoft.Json;

namespace PulseLogicHMR.Tests.Controllers
{
    public class ClinicalViewControllerTests
    {
        private Mock<IOptions<MyConfiguration>> _mockConfig;
        private Mock<ISession> _mockSession;
        private Mock<HttpContext> _mockHttpContext;
        private ClinicalViewController _controller;

        public ClinicalViewControllerTests()
        {
            _mockConfig = new Mock<IOptions<MyConfiguration>>();
            _mockConfig.Setup(c => c.Value).Returns(new MyConfiguration { Connectionstring = "TestConnectionString" });

            _mockSession = new Mock<ISession>();
            byte[]? outValue = null;
            _mockSession.Setup(s => s.TryGetValue("validRouteList", out outValue)).Returns(false);

            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpContext.Setup(c => c.Session).Returns(_mockSession.Object);

            _controller = new ClinicalViewController(_mockConfig.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };
        }

        [Fact]
        public void Vitals_SetsConnectionStringAndReturnsView()
        {
            // Act
            var result = _controller.Vitals() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Vitals", result.ViewName);
            Assert.Equal("TestConnectionString", _controller.ViewData["ConnectionString"]);
        }

        [Fact]
        public void Vitals_ThrowsException_WhenErrorOccurs()
        {
            // Arrange
            // We can force an error by setting HttpContext to null since GetView uses it
            _controller.ControllerContext.HttpContext = null!;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _controller.Vitals());
        }
    }
}
