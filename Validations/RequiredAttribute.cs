namespace Validations
{
	public class RequiredAttribute : ValidationAttribute
	{
		public const string MESSAGE = "Required Field";

		public RequiredAttribute() : this(MESSAGE)
		{
		}

		public RequiredAttribute(string message) : base(message)
		{
		}

		protected override bool IsValid(object rawValue)
		{
			return !IsMissing(rawValue);
		}
	}
}