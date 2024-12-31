using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSSystem.Converters;

namespace POSSystem.Tests.Converters
{
    [TestClass()]
    public class InverseBoolConverterTests
    {
        private InverseBoolConverter _converter;

        [TestInitialize]
        public void TestInitialize()
        {
            _converter = new InverseBoolConverter();
        }

        [TestMethod()]
        [DataRow(true, false)]
        [DataRow(false, true)]
        [DataRow(null, null)]
        public void Convert_ShouldReturnExpectedBool(object input, object expected)
        {
            // Act
            var result = _converter.Convert(input, typeof(bool), null, "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        [DataRow(true, false)]
        [DataRow(false, true)]
        [DataRow(null, null)]
        public void ConvertBack_ShouldReturnExpectedBool(object input, object expected)
        {
            // Act
            var result = _converter.ConvertBack(input, typeof(bool), null, "");

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}