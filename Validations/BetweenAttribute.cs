using System;
using System.Globalization;
using System.Reflection;
using Validations;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BetweenAttribute : ValidationAttribute 
    {
        public static string ErrorMessage(object start, object end)
        {
            return string.Format(CultureInfo.CurrentCulture, "Value must be between {0} and {1}", start, end);
        }

        private static IComparable Parse(string value, Type type)
        {
            var parseMethod = type.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) });
            if (parseMethod == null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} does not have a public static Parse method.", type.Name));

            var result = parseMethod.Invoke(null, new object[] { value, CultureInfo.CurrentCulture }) as IComparable;
            if (result == null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} does not implement IComparable", type.Name));

            return result;
        }
        
        private readonly IComparable start;
        private readonly IComparable end;

        public BetweenAttribute(object start, object end) : this(start, end, ErrorMessage(start, end))
        {
        }

        public BetweenAttribute(object start, object end, string message) : base(message)
        {
            this.start = start as IComparable;
            this.end = end as IComparable;

            if (start == null || end == null)
                throw new ArgumentException("The given type must implement IComparable");
        }
        
        public BetweenAttribute(string start, string end, Type type) 
            : this(start, end, type, ErrorMessage(start, end))
        {
        }

        public BetweenAttribute(string start, string end, Type type, string message)
            : this(Parse(start, type), Parse(end, type), message)
        {
        }

        protected override bool IsValid(object rawValue)
        {
            // Ignore missing values -- use RequiredAttribute for those...
            if (IsMissing(rawValue))
                return true;

            try
            {
                var value = rawValue as IComparable;
                if ((value is string) && !(start is string))
                    value = Parse(value.ToString(), start.GetType());

                if (value == null)
                    return false;
                
                return value.CompareTo(start) >= 0 && value.CompareTo(end) <= 0;
            }
            catch (TargetInvocationException)
            {
                return false;
            }
        }
    }
}
