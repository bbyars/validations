using System;
using System.Reflection;

namespace Validations
{
	public class CanParseAsAttribute : ValidationAttribute
	{
		public const string MESSAGE = "Invalid format";
        
		private readonly MethodInfo parseMethod;
        
		public CanParseAsAttribute(Type type)
		{
			parseMethod = type.GetMethod("Parse", new[] { typeof(string) });
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
				parseMethod.Invoke(null, new[] { rawValue });
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override string Message
		{
			get { return MESSAGE; }
		}
	}
}