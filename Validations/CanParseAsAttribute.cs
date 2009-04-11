using System;
using System.Globalization;
using System.Reflection;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CanParseAsAttribute : ValidationAttribute
    {
        public const string DefaultMessage = "Invalid format";

        private readonly MethodInfo parseMethod;

        public CanParseAsAttribute(Type type) : this(type, DefaultMessage)
        {
        }

        public CanParseAsAttribute(Type type, string message) : base(message)
        {
            parseMethod = type.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) });
            if (parseMethod == null)
                throw new ArgumentException("The given type must have a public static Parse method.");
        }

        protected override bool IsValid(object rawValue)
        {
            // Ignore empty values -- use RequiredAttribute for those
            if (IsMissing(rawValue))
                return true;

            try
            {
                parseMethod.Invoke(null, new[] { rawValue, CultureInfo.CurrentCulture });
                return true;
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine(ex.GetType().Name);
                return false;
            }
        }
    }
}
