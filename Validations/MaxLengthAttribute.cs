using Validations;

public class MaxLengthAttribute : ValidationAttribute
{
	private static string DefaultMessage(int maxLength)
	{
		return string.Format("cannot be more than {0} characters", maxLength);
	}

	private readonly int maxLength;

	public MaxLengthAttribute(int maxLength) : this(maxLength, DefaultMessage(maxLength))
	{
	}

	public MaxLengthAttribute(int maxLength, string message) : base(message)
	{
		this.maxLength = maxLength;
	}

	protected override bool IsValid(object rawValue)
	{
		if (IsMissing(rawValue))
			return true;

		return rawValue.ToString().Length <= maxLength;
	}
}