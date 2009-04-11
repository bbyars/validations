using NUnit.Framework;
using Validations;

namespace Validations.Test
{
    [TestFixture]
    public class MatchesAttributeTest
    {
        public string NullString
        {
            get { return null; }
        }

        [Test]
        public void NullStringIsValid()
        {
            var attribute = new MatchesAttribute(@"\d{3}-\d{3}-\d{4}");
            ValidationAssert.IsValid(attribute, this, "NullString");
        }

        public string Invalid
        {
            get { return "22-22-222"; }
        }

        [Test]
        public void InvalidString()
        {
            var attribute = new MatchesAttribute(@"\d{3}-\d{3}-\d{4}");
            ValidationAssert.IsNotValid(attribute, this, "Invalid");
        }

        public int WrongType
        {
            get { return 1; }
        }

        [Test]
        public void WrongTypeIsNotValid()
        {
            var attribute = new MatchesAttribute(@"\d{3}-\d{3}-\d{4}");
            ValidationAssert.IsNotValid(attribute, this, "WrongType");
        }

        public string Valid
        {
            get { return "999-999-9999"; }
        }

        [Test]
        public void ValidStringIsValid()
        {
            var attribute = new MatchesAttribute(@"\d{3}-\d{3}-\d{4}");
            ValidationAssert.IsValid(attribute, this, "Valid");
        }
    }
}
