using System;

namespace Validations
{
	public class BetweenAttribute : ValidationAttribute 
	{
		public static string ErrorMessage(object start, object end)
		{
			return string.Format("Value must be between {0} and {1}", start, end);
		}
        
		private readonly IComparable start;
		private readonly IComparable end;

		public BetweenAttribute(object start, object end)
		{
			this.start = start as IComparable;
			this.end = end as IComparable;

			if (start == null || end == null)
				throw new ArgumentException("The given type must implement IComparable");
		}
        
		public BetweenAttribute(string start, string end, Type type)
		{
			this.start = Parse(start, type);
			this.end = Parse(end, type);

			if (start == null || end == null)
				throw new ArgumentException("The given type must implement IComparable");
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
			catch
			{
				return false;
			}
		}

		private IComparable Parse(string value, Type type)
		{
			var parseMethod = type.GetMethod("Parse", new[] { typeof(string) });
			if (parseMethod == null)
				throw new ArgumentException(string.Format("{0} does not have a public static Parse method.", type.Name));

			var result = parseMethod.Invoke(null, new object[] { value }) as IComparable;
			if (result == null)
				throw new ArgumentException(string.Format("{0} does not implement IComparable", type.Name));
            
			return result;
		}
        
		public override string Message
		{
			get { return ErrorMessage(start, end); }
		}
	}
}