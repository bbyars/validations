using System;
using NUnit.Framework;

namespace Validations.Test
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void ParsesInt()
        {
            Assert.AreEqual(1, new Parser().Parse("1", typeof(int)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotParseTypeWithNoParseMethod()
        {
            new Parser().Parse("", typeof(string));
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void CannotParseInvalidInput()
        {
            new Parser().Parse("thirtyFive", typeof(int));
        }

        [Test]
        public void CanParseWithExtraWhitespace()
        {
            Assert.AreEqual(1, new Parser().Parse(" 1 ", typeof(int)));
        }

        [Test]
        public void CanParseWithFormatProvider()
        {
            var parsedValue = new Parser().Parse("", typeof(TypeWithFormatProvider));
            Assert.IsInstanceOfType(typeof(TypeWithFormatProvider), parsedValue);
        }

        [Test]
        public void CanParseWithoutFormatProvider()
        {
            var parsedValue = new Parser().Parse("", typeof(TypeWithoutFormatProvider));
            Assert.IsInstanceOfType(typeof(TypeWithoutFormatProvider), parsedValue);
        }

        private class TypeWithFormatProvider
        {
            public static TypeWithFormatProvider Parse(string value, IFormatProvider formatProvider)
            {
                return new TypeWithFormatProvider();
            }
        }
        
        private class TypeWithoutFormatProvider
        {
            public static TypeWithoutFormatProvider Parse(string value)
            {
                return new TypeWithoutFormatProvider();
            }
        }
    }
}