using System.Text.RegularExpressions;

namespace Validations
{
	public class MatchesAttribute : ValidationAttribute
	{
		public const string MESSAGE = "Invalid format";
        
		private readonly Regex regex;
        
		public MatchesAttribute(string pattern) : this(pattern, RegexOptions.None)
		{
		}
        
		public MatchesAttribute(string pattern, RegexOptions options)
		{
			regex = new Regex(pattern, options);
		}
        
		protected override bool IsValid(object rawValue)
		{
			if (IsMissing(rawValue))
				return true;
            
			return regex.IsMatch(rawValue.ToString());
		}

		public override string Message
		{
			get { return MESSAGE; }
		}
	}
}