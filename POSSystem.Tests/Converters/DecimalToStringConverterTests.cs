using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSSystem.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.Tests.Converters
{
    [TestClass()]
    public class DecimalToStringConverterTests
    {
        private DecimalToStringConverter _converter;

        [TestInitialize]
        public void Setup()
        {
            _converter = new DecimalToStringConverter();
        }

        [TestMethod()]
        [DataRow(1.11, "1.11")]
        [DataRow(0, "0")]
        [DataRow(-1.11, "-1.11")]
        [DataRow(null, "")]
        public void Convert_ShouldReturnExpectedString(object input, object expected)
        {
            // Act
            var result = _converter.Convert(input, typeof(string), null, "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        [DataRow("1.11", 1.11)]
        [DataRow("0", 0)]
        [DataRow("-1.11", -1.11)]
        [DataRow("invalid", null)]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void ConvertBack_ShouldReturnExpectedDecimal(object input, object expected)
        {
            // Act
            var result = _converter.ConvertBack(input, typeof(decimal), null, "");

            // Assert
            if (expected == null)
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.AreEqual(Convert.ToDecimal(expected), result);
            }
        }
    }
}
