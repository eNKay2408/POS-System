using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSSystem.Converters;

namespace POSSystem.Tests.Converters
{
    [TestClass()]
    public class IntToStringConverterTests
    {
        private IntToStringConverter _converter;

        [TestInitialize]
        public void Setup()
        {
            _converter = new IntToStringConverter();
        }

        [TestMethod()]
        [DataRow(123, "123")]
        [DataRow(0, "0")]
        [DataRow(-123, "-123")]
        [DataRow(null, "")]
        public void Convert_ShouldReturnExpectedString(object input, object expected)
        {
            // Act
            var result = _converter.Convert(input, typeof(string), null, "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        [DataRow("123", 123)]
        [DataRow("0", 0)]
        [DataRow("-123", -123)]
        [DataRow("invalid", null)]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void ConvertBack_ShouldReturnExpectedInt(object input, object expected)
        {
            // Act
            var result = _converter.ConvertBack(input, typeof(int), null, "");

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}