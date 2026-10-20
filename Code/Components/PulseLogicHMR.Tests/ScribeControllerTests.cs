using System;
using System.Dynamic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Controllers.Clinical;

namespace PulseLogicHMR.Tests.Clinical
{
    public class ScribeControllerTests
    {
        [Fact]
        public void Transcribe_Fever_ReturnsFeverICD10()
        {
            var mockConfig = new Mock<IOptions<MyConfiguration>>();
            mockConfig.Setup(c => c.Value).Returns(new MyConfiguration());

            var controller = new ScribeController(mockConfig.Object);

            dynamic payload = new ExpandoObject();
            payload.text = "I have a fever";

            var result = controller.Transcribe(payload);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            var valueType = okResult!.Value!.GetType();
            var resultsProperty = valueType.GetProperty("Results");
            var resultsValue = resultsProperty!.GetValue(okResult.Value);

            var resultsType = resultsValue!.GetType();
            var icd10CodeProperty = resultsType.GetProperty("ICD10Code");
            var icd10CodeValue = icd10CodeProperty!.GetValue(resultsValue);

            Assert.Equal("R50.9", icd10CodeValue);
        }

        [Fact]
        public void Transcribe_SoreThroat_ReturnsPharyngitisICD10()
        {
            var mockConfig = new Mock<IOptions<MyConfiguration>>();
            mockConfig.Setup(c => c.Value).Returns(new MyConfiguration());

            var controller = new ScribeController(mockConfig.Object);

            dynamic payload = new ExpandoObject();
            payload.text = "I have a sore throat";

            var result = controller.Transcribe(payload);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            var valueType = okResult!.Value!.GetType();
            var resultsProperty = valueType.GetProperty("Results");
            var resultsValue = resultsProperty!.GetValue(okResult.Value);

            var resultsType = resultsValue!.GetType();
            var icd10CodeProperty = resultsType.GetProperty("ICD10Code");
            var icd10CodeValue = icd10CodeProperty!.GetValue(resultsValue);

            Assert.Equal("J02.9", icd10CodeValue);
        }

        [Fact]
        public void Transcribe_NullPayload_ReturnsError()
        {
            var mockConfig = new Mock<IOptions<MyConfiguration>>();
            mockConfig.Setup(c => c.Value).Returns(new MyConfiguration());

            var controller = new ScribeController(mockConfig.Object);

            // Passing null or payload without text should trigger catch block in controller
            dynamic payload = new ExpandoObject();

            var result = controller.Transcribe(payload);

            // Expected failure: Microsoft.CSharp.RuntimeBinder.RuntimeBinderException when accessing payload.text
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult!.StatusCode);
        }
    }
}
