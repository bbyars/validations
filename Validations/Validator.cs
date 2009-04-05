using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Validations
{
	public static class Validator
	{
		private static BindingFlags Bindings
		{
			get { return BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance; }
		}

		public static Notification Validate(object obj)
		{
			var notification = new Notification();

			var properties = obj.GetType().GetProperties(Bindings);
			foreach (PropertyInfo property in properties)
			{
				ValidateProperty(obj, property, notification);
			}

			return notification;
		}

		public static Notification ValidateProperty(object obj, string propertyName)
		{
			var notification = new Notification();
			var property = obj.GetType().GetProperty(propertyName, Bindings);

			if (property == null)
				throw new ArgumentException("Invalid property name:" + propertyName);

			ValidateProperty(obj, property, notification);
			return notification;
		}

		public static bool DependencySatisfied(object obj, string propertyName)
		{
			var property = obj.GetType().GetProperty(propertyName, Bindings);
			return DependencySatisfied(obj, property);
		}

		public static bool DependencySatisfied(object obj, PropertyInfo property)
		{
			var attribute = Attribute.GetCustomAttribute(property, typeof(DependsOnAttribute)) as DependsOnAttribute;
			if (attribute == null)
				return true;

			try
			{
				var dependentProperty = obj.GetType().GetProperty(attribute.PropertyName);
				return Equals(attribute.MustEqual, dependentProperty.GetValue(obj, null));
			}
			catch (Exception ex)
			{
				throw new ArgumentException("DependsOnAttribute references invalid property: " + attribute.PropertyName, ex);
			}
		}

		private static void ValidateProperty(object obj, PropertyInfo property, Notification notification)
		{
			if (!DependencySatisfied(obj, property))
				return;

			foreach (ValidationAttribute validationRule in GetValidationAttributes(obj, property))
			{
				validationRule.Property = property;
				validationRule.Validate(obj, notification);
			}
		}

		private static IEnumerable GetValidationAttributes(object obj, PropertyInfo property)
		{
			var attributes = new List<Attribute>();
			AddIfNotAddedAlready(property, attributes);

			// For some reason, inherit=true doesn't include interfaces
			var interfaces = obj.GetType().FindInterfaces(delegate { return true; }, null);
			foreach (Type interfase in interfaces)
			{
				var interfaceProperty = interfase.GetProperty(property.Name, Bindings);
				if (interfaceProperty != null)
					AddIfNotAddedAlready(interfaceProperty, attributes);
			}
			return attributes;
		}

		private static void AddIfNotAddedAlready(PropertyInfo property, List<Attribute> attributes)
		{
			var validators = Attribute.GetCustomAttributes(property, typeof(ValidationAttribute), true);
			foreach (Attribute validator in validators)
			{
				var alreadyAdded = attributes.Find(match => match.GetType() == validator.GetType());

				if (alreadyAdded == null)
					attributes.Add(validator);
			}
		}
	}
}