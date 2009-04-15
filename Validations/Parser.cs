using System;
using System.Globalization;
using System.Reflection;

namespace Validations
{
    public class Parser
    {
        public virtual object Parse(string value, Type type)
        {
            var useCultureInfo = true;
            var parseMethod = type.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) });
            if (parseMethod == null)
            {
                useCultureInfo = false;
                parseMethod = type.GetMethod("Parse", new[] { typeof(string) });
                if (parseMethod == null)
                    throw new ArgumentException(string.Format("Type {0} has no public static Parse method", type.Name));
            }

            try
            {
                return useCultureInfo
                    ? parseMethod.Invoke(null, Bindings, null, new object[] {value, CultureInfo.CurrentCulture}, null)
                    : parseMethod.Invoke(null, Bindings, null, new object[] {value}, null);
            }
            catch (TargetInvocationException ex)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Unable to parse <{0}> as a {1}.", value, type.Name);
                throw new FormatException(message, ex);
            }
        }

        private BindingFlags Bindings
        {
            get { return BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy; }
        }
    }
}