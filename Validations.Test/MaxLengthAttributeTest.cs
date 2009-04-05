using NUnit.Framework;
using Validations.Test;

[TestFixture]
public class MaxLengthAttributeTest
{
	public string NullString
	{
		get { return null; }
	}

	[Test]
	public void NullIsValid()
	{
		var attribute = new MaxLengthAttribute(10);
		ValidationAssert.IsValid(attribute, this, "NullString");
	}

	public string FourLetterWord
	{
		get { return "four"; }
	}

	[Test]
	public void TooLong()
	{
		var attribute = new MaxLengthAttribute(3);
		ValidationAssert.IsNotValid(attribute, this, "FourLetterWord");
	}

	[Test]
	public void AtMaxLength()
	{
		var attribute = new MaxLengthAttribute(4);
		ValidationAssert.IsValid(attribute, this, "FourLetterWord");
	}
}