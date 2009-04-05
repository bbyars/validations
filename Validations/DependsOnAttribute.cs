using System;

namespace Validations
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DependsOnAttribute : Attribute
	{
		private readonly string propertyName;
		private readonly object mustEqual;

		public DependsOnAttribute(string propertyName) : this(propertyName, true)
		{
		}

		public DependsOnAttribute(string propertyName, object mustEqual)
		{
			this.propertyName = propertyName;
			this.mustEqual = mustEqual;
		}

		public object MustEqual
		{
			get { return mustEqual; }
		}

		public string PropertyName
		{
			get { return propertyName; }
		}
	}
}