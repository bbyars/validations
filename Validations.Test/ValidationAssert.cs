using System;
using NUnit.Framework;
using Validations;

namespace Validations.Test
{
    public static class ValidationAssert
    {
        public static void IsNotValid(ValidationAttribute attribute, object caller, string propertyName)
        {
            AssertValidation(attribute, caller, propertyName, Assert.IsTrue);
        }

        public static void IsValid(ValidationAttribute attribute, object caller, string propertyName)
        {
            AssertValidation(attribute, caller, propertyName, Assert.IsFalse);
        }

        public static Notification GetNotification(ValidationAttribute attribute, object caller, string propertyName)
        {
            var notification = new Notification();
            attribute.Property = caller.GetType().GetProperty(propertyName);
            attribute.Validate(caller, notification);
            return notification;
        }

        private static void AssertValidation(ValidationAttribute attribute, object caller, 
            string propertyName, Action<bool> asserter)
        {
            var notification = GetNotification(attribute, caller, propertyName);
            asserter(notification.HasMessage(propertyName, attribute.Message));
        }
    }
}
