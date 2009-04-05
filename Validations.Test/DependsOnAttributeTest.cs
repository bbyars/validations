using System;
using NUnit.Framework;

namespace Validations.Test
{
	[TestFixture]
	public class DependsOnAttributeTest
	{
		[Required]
		[DependsOn("ValueSelected")]
		public int Value { get; set; }

		public bool ValueSelected { get; set; }

		[Test]
		public void DependencySatisfiedIfNoDependsOnAttribute()
		{
			Assert.IsTrue(Validator.DependencySatisfied(this, "ValueSelected"));
		}

		[Test]
		public void DependencyNotSatisfied()
		{
			ValueSelected = false;
			Assert.IsFalse(Validator.DependencySatisfied(this, "Value"));
		}

		[Test]
		public void DependencySatisfiedIfDependentPropertyTrue()
		{
			ValueSelected = true;
			Assert.IsTrue(Validator.DependencySatisfied(this, "Value"));
		}

		[DependsOn("InvalidProperty")]
		public int OtherValue { get; set; }

		[Test]
		public void HelpfulExceptionIfInvalidProperty()
		{
			try
			{
				Validator.DependencySatisfied(this, "OtherValue");
				Assert.Fail("Should have thrown exception");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.Message.Contains("InvalidProperty"));
			}
		}

		[DependsOn("ValueSelected", false)]
		public int ValueOff { get; set; }

		[Test]
		public void FailedDependencyWithNonDefaultValue()
		{
			ValueSelected = true;
			Assert.IsFalse(Validator.DependencySatisfied(this, "ValueOff"));
		}

		[Test]
        public void PassedDependencyWithNonDefaultValue()
		{
			ValueSelected = false;
			Assert.IsTrue(Validator.DependencySatisfied(this, "ValueOff"));
		}
	}
}