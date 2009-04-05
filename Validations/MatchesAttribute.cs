using System.Text.RegularExpressions;

namespace Validations
{
	public class MatchesAttribute : ValidationAttribute
	{
		public const string MESSAGE = "Invalid format";
        
		private readonly Regex regex;
        
		public MatchesAttribute(string pattern) : this(pattern, MESSAGE)
		{
		}

		public MatchesAttribute(string pattern, string message) : this(pattern, RegexOptions.None, message)
		{
		}
        
		public MatchesAttribute(string pattern, RegexOptions options) : this(pattern, options, MESSAGE)
		{
		}

		public MatchesAttribute(string pattern, RegexOptions options, string message) : base(message)
		{
			regex = new Regex(pattern, options);
		}
 
		protected override bool IsValid(object rawValue)
		{
			if (IsMissing(rawValue))
				return true;
            
			return regex.IsMatch(rawValue.ToString());
		}
	}
}