using System;
using System.Text.RegularExpressions;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MatchesAttribute : ValidationAttribute
    {
        public const string DefaultMessage = "Invalid format";

        private readonly Regex regex;

        public MatchesAttribute(string pattern) : this(pattern, DefaultMessage)
        {
        }

        public MatchesAttribute(string pattern, string message) : this(pattern, RegexOptions.None, message)
        {
        }

        public MatchesAttribute(string pattern, RegexOptions options) : this(pattern, options, DefaultMessage)
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
