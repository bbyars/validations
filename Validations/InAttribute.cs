using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InAttribute : ValidationAttribute
    {
        public static string ErrorMessage(params object[] validValues)
        {
            var values = string.Join(", ", validValues.Select(value => PrettyPrint(value)).ToArray());
            return string.Format(CultureInfo.CurrentCulture, "Value must be one of [{0}]", values);
        }

        private static string PrettyPrint(object value)
        {
            if (value == null)
                return "<NULL>";

            if (value is string)
                return "\"" + value + "\"";

            return value.ToString();
        }

        private readonly ArrayList validValues;

        public InAttribute(params object[] validValues) : this(ErrorMessage(validValues), validValues)
        {
        }

        public InAttribute(string message, params object[] validValues) : base(message)
        {
            this.validValues = new ArrayList(validValues);
        }

        protected override bool IsValid(object rawValue)
        {
            // Ignore missing values -- use RequiredAttribute for those...
            if (IsMissing(rawValue))
                return true;

            return validValues.Contains(rawValue);
        }
    }
}
