using System;
using NUnit.Framework;
using Validations;
using Validations.Test;

namespace Validations.Test
{
    [TestFixture]
    public class CanParseAsAttributeTest
    {
        public string NullString
        {
            get { return null; }
        }

        [Test]
        public void NullValuesAreValid()
        {
            var attribute = new CanParseAsAttribute(typeof(int));
            ValidationAssert.IsValid(attribute, this, "NullString");
        }

        public string EmptyString
        {
            get { return ""; }
        }

        [Test]
        public void EmptyValuesAreValid()
        {
            var attribute = new CanParseAsAttribute(typeof(int));
            ValidationAssert.IsValid(attribute, this, "EmptyString");
        }

        public string InvalidNumber
        {
            get { return "thirtyFive"; }
        }

        [Test]
        public void InvalidNumberIsNotValid()
        {
            var attribute = new CanParseAsAttribute(typeof(int));
            ValidationAssert.IsNotValid(attribute, this, "InvalidNumber");
        }

        public string ValidNumber
        {
            get { return "35"; }
        }

        [Test]
        public void ValidNumberIsValid()
        {
            var attribute = new CanParseAsAttribute(typeof(int));
            ValidationAssert.IsValid(attribute, this, "ValidNumber");
        }

        public string ValidNumberWithSpaces
        {
            get { return " 35 "; }
        }

        [Test]
        public void ValidNumberWithSpacesIsValid()
        {
            var attribute = new CanParseAsAttribute(typeof(int));
            ValidationAssert.IsValid(attribute, this, "ValidNumberWithSpaces");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TypeWithNoParseMethodThrowsException()
        {
            var attribute = new CanParseAsAttribute(GetType());
            ValidationAssert.IsNotValid(attribute, this, "ValidNumber");
        }
    }
}
