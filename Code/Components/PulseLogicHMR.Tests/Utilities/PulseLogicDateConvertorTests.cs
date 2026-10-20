using System;
using PulseLogicHMR.Core.Utilities;
using Xunit;

namespace PulseLogicHMR.Tests.Utilities
{
    public class PulseLogicDateConvertorTests
    {
        [Theory]
        [InlineData(2078, 1, 1, 0, 0, "AM", "2021-04-14 00:00:00")]
        [InlineData(2078, 1, 1, 1, 30, "AM", "2021-04-14 01:30:00")]
        [InlineData(2078, 1, 1, 12, 0, "PM", "2021-04-14 12:00:00")]
        [InlineData(2078, 1, 1, 1, 30, "PM", "2021-04-14 13:30:00")]
        [InlineData(2078, 1, 1, 12, 30, "AM", "2021-04-14 00:30:00")]
        public void ConvertNepToEngDate_ValidDate_ReturnsCorrectEnglishDate(
            int year, int month, int day, int hours, int minutes, string ampm, string expectedDateTimeStr)
        {
            // Arrange
            var nepaliDate = new NepaliDateType
            {
                Year = year,
                Month = month,
                Day = day,
                Hours = hours,
                Minutes = minutes,
                AMPM = ampm
            };
            var expectedDateTime = DateTime.Parse(expectedDateTimeStr);

            // Act
            var result = PulseLogicDateConvertor.ConvertNepToEngDate(nepaliDate);

            // Assert
            Assert.Equal(expectedDateTime, result);
        }

        [Fact]
        public void ConvertNepToEngDate_EmptyDate_ReturnsDefaultDateTime()
        {
            // Arrange
            var nepaliDate = new NepaliDateType { Year = 0, Month = 0, Day = 0 };

            // Act
            var result = PulseLogicDateConvertor.ConvertNepToEngDate(nepaliDate);

            // Assert
            Assert.Equal(default(DateTime), result);
        }

        [Fact]
        public void ConvertNepToEngDate_NullNepaliDate_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => PulseLogicDateConvertor.ConvertNepToEngDate(null));
        }

        [Fact]
        public void ConvertNepToEngDate_InvalidYear_ThrowsException()
        {
            // Arrange
            var nepaliDate = new NepaliDateType { Year = 1800, Month = 1, Day = 1 }; // Year not in dictionary

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => PulseLogicDateConvertor.ConvertNepToEngDate(nepaliDate));
        }

        [Fact]
        public void ConvertNepToEngDate_ValidDateEndOfYear_ReturnsCorrectEnglishDate()
        {
            // Arrange
            var nepaliDate = new NepaliDateType { Year = 2077, Month = 12, Day = 31, Hours = 11, Minutes = 59, AMPM = "PM" };

            // Act
            var result = PulseLogicDateConvertor.ConvertNepToEngDate(nepaliDate);

            // Assert
            Assert.Equal(DateTime.Parse("2021-04-13 23:59:00"), result);
        }
    }
}
