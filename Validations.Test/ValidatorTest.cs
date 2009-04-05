using System;
using NUnit.Framework;
using Validations;

namespace Validations.Test
{
	public class ValidatorTestClass
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

	public class ValidatorTestSubclass : ValidatorTestClass
	{
		[Required]
		public override string InheritedField
		{
			get { return base.InheritedField; }
			set { base.InheritedField = value; }
		}
	}

	public interface ValidatorTestInterface
	{
		[CanParseAs(typeof(int))]
		string InheritedField { get; set; }
	}

	public class ValidatorTestSubclassWithInterface : ValidatorTestSubclass, ValidatorTestInterface
	{
	}

	[TestFixture]
	public class ValidatorTest
	{
		[Test]
		public void MultipleFailures()
		{
			var obj = new ValidatorTestClass {InheritedField = "A"};
			var notification = Validator.Validate(obj);
			Assert.IsTrue(notification.HasMessage("InheritedField", BetweenAttribute.ErrorMessage(0, 9)), "missing between");
			Assert.IsTrue(notification.HasMessage("ZipCode", RequiredAttribute.MESSAGE), "missing required");
		}
        
		[Test]
		public virtual void NothingFails()
		{
			var obj = new ValidatorTestClass {InheritedField = "0", ZipCode = "75672"};
			var notification = Validator.Validate(obj);
			Assert.IsTrue(notification.IsValid);
		}
        
		[Test]
		public virtual void SubclassAttributesAlsoChecked()
		{
			var subclass = new ValidatorTestSubclass();
			var notification = Validator.Validate(subclass);
			Assert.IsTrue(notification.HasMessage("InheritedField", RequiredAttribute.MESSAGE));
		}
        
		[Test]
		public virtual void InterfaceAttributesAlsoChecked()
		{
			var subclass = new ValidatorTestSubclassWithInterface {InheritedField = "notANumber"};
			var notification = Validator.Validate(subclass);
			Assert.IsTrue(notification.HasMessage("InheritedField", CanParseAsAttribute.MESSAGE));
		}
        
		[Test]
		public virtual void ValidateProperty()
		{
			var obj = new ValidatorTestClass {InheritedField = "A"};
			var notification = Validator.ValidateProperty(obj, "InheritedField");
			Assert.IsTrue(notification.HasMessage("InheritedField", BetweenAttribute.ErrorMessage(0, 9)));
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
			Assert.IsTrue(notification.HasMessage("Field", BetweenAttribute.ErrorMessage(0, 9)));
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