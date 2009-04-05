namespace Validations
{
	public class RequiredAttribute : ValidationAttribute
	{
		public const string MESSAGE = "Required Field";
        
		protected override bool IsValid(object rawValue)
		{
			return !IsMissing(rawValue);
		}

		public override string Message
		{
			get { return MESSAGE; }
		}
	}
}