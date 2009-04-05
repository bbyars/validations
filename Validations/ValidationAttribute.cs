using System;
using System.Reflection;

namespace Validations
{
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class ValidationAttribute : Attribute
	{
		protected abstract bool IsValid(object rawValue);
		public abstract string Message { get; }

		public PropertyInfo Property { get; set; }

		public void Validate(object target, Notification notification)
		{
			var rawValue = Property.GetValue(target, null);
			if (!IsValid(rawValue))
				notification.Register(Property.Name, Message);                
		}

		protected bool IsMissing(object rawValue)
		{
			return rawValue == null || rawValue.ToString().Trim() == "";
		}
	}
}