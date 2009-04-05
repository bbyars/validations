using NUnit.Framework;
using Validations;
using Validations.Test;

namespace Validations.Test
{
	[TestFixture]
	public class InAttributeTest
	{
		public string NullString
		{
			get { return null; }
		}

		[Test]
		public void NullsValid()
		{
			var attribute = new InAttribute("Jan", "Feb", "Mar");
			ValidationAssert.IsValid(attribute, this, "NullString");
		}

		public string InvalidMonth
		{
			get { return "January"; }
		}

		[Test]
		public void InvalidMonthNotValid()
		{
			var attribute = new InAttribute("Jan", "Feb", "Mar");
			ValidationAssert.IsNotValid(attribute, this, "InvalidMonth");
		}

		public string ValidMonth
		{
			get { return "Mar"; }
		}

		[Test]
		public void ValidMonthIsValid()
		{
			var attribute = new InAttribute("Jan", "Feb", "Mar");
			ValidationAssert.IsValid(attribute, this, "ValidMonth");
		}
	}
}