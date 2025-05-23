using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Helpers;

namespace UnitTesting_CodeReuse_DateHelper_NurAhmadiAdityaNanda
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Test_FormatIndo_ShouldReturnCorrectFormat()
        {
            // Arrange
            DateTime inputDate = new DateTime(2025, 5, 23);

            // Act
            string formattedDate = DateHelper.FormatIndo(inputDate);

            // Assert
            Assert.AreEqual("23 Mei 2025", formattedDate);
        }

        [TestMethod]
        public void Test_FormatIndo_WithSingleDigitDayAndMonth()
        {
            // Arrange
            DateTime inputDate = new DateTime(2025, 1, 1);

            // Act
            string formattedDate = DateHelper.FormatIndo(inputDate);

            // Assert
            Assert.AreEqual("01 Januari 2025", formattedDate);
        }
    }
}
