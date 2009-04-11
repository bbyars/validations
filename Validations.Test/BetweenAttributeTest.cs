using System;
using NUnit.Framework;
using Validations;

namespace Validations.Test
{
    [TestFixture]
    public class BetweenAttributeTest
    {
        public string NullString
        {
            get { return null; }
        }

        [Test]
        public void NullsValid()
        {
            var attribute = new BetweenAttribute(0, 999);
            ValidationAssert.IsValid(attribute, this, "NullString");
        }

        public int TooLow
        {
            get { return -1; }
        }

        [Test]
        public void TooLowIsNotValid()
        {
            var attribute = new BetweenAttribute(0, 999);
            ValidationAssert.IsNotValid(attribute, this, "TooLow");
        }

        public int Start
        {
            get { return 0; }
        }

        [Test]
        public void StartIsValid()
        {
            var attribute = new BetweenAttribute(0, 999);
            ValidationAssert.IsValid(attribute, this, "Start");
        }

        public int End
        {
            get { return 999; }
        }

        [Test]
        public void EndIsValid()
        {
            var attribute = new BetweenAttribute(0, 999);
            ValidationAssert.IsValid(attribute, this, "End");
        }

        public int TooHigh
        {
            get { return 1000; }
        }

        [Test]
        public void TooHighIsNotValid()
        {
            var attribute = new BetweenAttribute(0, 999);
            ValidationAssert.IsNotValid(attribute, this, "TooHigh");
        }

        public DateTime Date
        {
            get { return new DateTime(2009, 1, 1); }
        }

        [Test]
        public void ParsesDates()
        {
            var attribute = new BetweenAttribute("1/1/2009", "1/1/2010", typeof(DateTime));
            ValidationAssert.IsValid(attribute, this, "Date");
        }

        [Test]
        public void UsesCustomMessage()
        {
            var attribute = new BetweenAttribute(0, 999, "Not in range");
            var notification = ValidationAssert.GetNotification(attribute, this, "TooHigh");
            Assert.AreEqual("Not in range", notification.GetMessageFor("TooHigh", ""));
        }
    }
}
