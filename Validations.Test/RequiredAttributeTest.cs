using NUnit.Framework;
using Validations;
using Validations.Test;

namespace Validations.Test
{
	[TestFixture]
	public class RequiredAttributeTest
	{
		public string NullString
		{
			get { return null; }
		}

		[Test]
		public void NullStringFailsValidation()
		{
			ValidationAssert.IsNotValid(new RequiredAttribute(), this, "NullString");
		}

		public string EmptyString
		{
			get { return ""; }
		}

		[Test]
		public void EmptyStringFailsValidation()
		{
			ValidationAssert.IsNotValid(new RequiredAttribute(), this, "EmptyString");
		}
        
		public string OneCharacterString
		{
			get { return "x"; }
		}
        
		[Test]
		public void OneCharacterStringIsValid()
		{
			ValidationAssert.IsValid(new RequiredAttribute(), this, "OneCharacterString");
		}
	}
}