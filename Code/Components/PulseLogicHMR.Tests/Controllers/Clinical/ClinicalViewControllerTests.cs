using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PulseLogicHMR.Controllers;
using PulseLogicHMR.Core.Configuration;
using Xunit;

namespace PulseLogicHMR.Tests.Controllers.Clinical
{
    public class ClinicalViewControllerTests
    {
        [Fact]
        public void Vitals_ThrowsException_WhenExceptionOccurs()
        {
            // Arrange
            var mockConfig = new Mock<IOptions<MyConfiguration>>();
            mockConfig.Setup(x => x.Value).Returns(new MyConfiguration { Connectionstring = "test" });

            var controller = new ClinicalViewController(mockConfig.Object);
            // Leaving ControllerContext/HttpContext null will cause HttpContext.Session to throw NullReferenceException inside GetView

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => controller.Vitals());
        }
    }
}
