using System;
using System.Globalization;
using Validations;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxLengthAttribute : ValidationAttribute
    {
        private static string DefaultMessage(int maxLength)
        {
            return string.Format(CultureInfo.CurrentCulture, "cannot be more than {0} characters", maxLength);
        }

        private readonly int maxLength;

        public MaxLengthAttribute(int maxLength)
            : this(maxLength, DefaultMessage(maxLength))
        {
        }

        public MaxLengthAttribute(int maxLength, string message)
            : base(message)
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
}