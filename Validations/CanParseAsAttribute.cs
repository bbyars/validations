using System;
using System.Reflection;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CanParseAsAttribute : ValidationAttribute
    {
        public const string DefaultMessage = "Invalid format";

        private readonly Type type;

        public CanParseAsAttribute(Type type) : this(type, DefaultMessage)
        {
        }

        public CanParseAsAttribute(Type type, string message) : base(message)
        {
            this.type = type;
        }

        protected override bool IsValid(object rawValue)
        {
            // Ignore empty values -- use RequiredAttribute for those
            if (IsMissing(rawValue))
                return true;

            try
            {
                new Parser().Parse(rawValue.ToString(), type);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
