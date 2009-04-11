using System;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredAttribute : ValidationAttribute
    {
        public const string DefaultMessage = "Required Field";

        public RequiredAttribute() : this(DefaultMessage)
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
