using System;
using System.Globalization;
using System.Reflection;
using Validations;

namespace Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BetweenAttribute : ValidationAttribute 
    {
        public static string DefaultErrorMessage(object start, object end)
        {
            return string.Format(CultureInfo.CurrentCulture, "Value must be between {0} and {1}", start, end);
        }

        private static object Parse(string value, Type type)
        {
            return new Parser().Parse(value, type);
        }
        
        private readonly IComparable start;
        private readonly IComparable end;

        public BetweenAttribute(object start, object end) : this(start, end, DefaultErrorMessage(start, end))
        {
        }

        public BetweenAttribute(object start, object end, string message) : base(message)
        {
            this.start = start as IComparable;
            this.end = end as IComparable;
        }
        
        public BetweenAttribute(string start, string end, Type type) 
            : this(start, end, type, DefaultErrorMessage(start, end))
        {
        }

        public BetweenAttribute(string start, string end, Type type, string message)
            : this(Parse(start, type), Parse(end, type), message)
        {
        }

        protected override bool IsValid(object rawValue)
        {
            if (start == null || end == null)
                throw new ArgumentException("The given type must implement IComparable");

            // Ignore missing values -- use RequiredAttribute for those...
            if (IsMissing(rawValue))
                return true;

            try
            {
                var value = rawValue as IComparable;
                if ((value is string) && !(start is string))
                    value = Parse(value.ToString(), start.GetType()) as IComparable;

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
