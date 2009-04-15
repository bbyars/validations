using System;
using NUnit.Framework;
using Validations;

namespace Validations.Test
{
    [TestFixture]
    public class ValidatorTest
    {
        private class ValidatorTestClass
        {
            [Between("0", "9")]
            public virtual string InheritedField { get; set; }

            [Required, Matches(@"\d{5}")]
            public virtual string ZipCode { get; set; }

            public virtual bool ValidateField { get; set; }

            [Between("0", "9")]
            [DependsOn("ValidateField")]
            public virtual string Field { get; set; }
        }

        private class ValidatorTestSubclass : ValidatorTestClass
        {
            [Required]
            public override string InheritedField
            {
                get { return base.InheritedField; }
                set { base.InheritedField = value; }
            }
        }

        private interface IValidatorTestInterface
        {
            [CanParseAs(typeof(int))]
            string InheritedField { get; set; }
        }

        private class ValidatorTestSubclassWithInterface : ValidatorTestSubclass, IValidatorTestInterface
        {
        }

        [Test]
        public void MultipleFailures()
        {
            var obj = new ValidatorTestClass { InheritedField = "A" };
            var notification = Validator.Validate(obj);
            Assert.IsTrue(notification.HasMessage("InheritedField", BetweenAttribute.DefaultErrorMessage(0, 9)), "missing between");
            Assert.IsTrue(notification.HasMessage("ZipCode", RequiredAttribute.DefaultMessage), "missing required");
        }

        [Test]
        public virtual void NothingFails()
        {
            var obj = new ValidatorTestClass { InheritedField = "0", ZipCode = "75672" };
            var notification = Validator.Validate(obj);
            Assert.IsTrue(notification.IsValid);
        }

        [Test]
        public virtual void SubclassAttributesAlsoChecked()
        {
            var subclass = new ValidatorTestSubclass();
            var notification = Validator.Validate(subclass);
            Assert.IsTrue(notification.HasMessage("InheritedField", RequiredAttribute.DefaultMessage));
        }

        [Test]
        public virtual void InterfaceAttributesAlsoChecked()
        {
            var subclass = new ValidatorTestSubclassWithInterface { InheritedField = "notANumber" };
            var notification = Validator.Validate(subclass);
            Assert.IsTrue(notification.HasMessage("InheritedField", CanParseAsAttribute.DefaultMessage));
        }

        [Test]
        public virtual void ValidateProperty()
        {
            var obj = new ValidatorTestClass { InheritedField = "A" };
            var notification = Validator.ValidateProperty(obj, "InheritedField");
            Assert.IsTrue(notification.HasMessage("InheritedField", BetweenAttribute.DefaultErrorMessage(0, 9)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void ValidateInvalidPropertyName()
        {
            Validator.ValidateProperty(this, "blarf");
        }

        [Test]
        public void FieldNotValidatedIfDependencyNotMet()
        {
            var obj = new ValidatorTestClass { ValidateField = false, Field = "A" };
            var notification = Validator.ValidateProperty(obj, "Field");
            Assert.IsTrue(notification.IsValid);
        }

        [Test]
        public void FieldValidatedIfDependencyMet()
        {
            var obj = new ValidatorTestClass { ValidateField = true, Field = "A" };
            var notification = Validator.ValidateProperty(obj, "Field");
            Assert.IsTrue(notification.HasMessage("Field", BetweenAttribute.DefaultErrorMessage(0, 9)));
        }

        [Test]
        public void PassedValidationWithDependency()
        {
            var obj = new ValidatorTestClass { ValidateField = true, Field = "0" };
            var notification = Validator.ValidateProperty(obj, "Field");
            Assert.IsTrue(notification.IsValid);
        }
    }
}
