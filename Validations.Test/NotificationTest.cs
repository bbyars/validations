using NUnit.Framework;
using Validations;

namespace Validations.Test
{
	[TestFixture]
	public class NotificationTest
	{
		[Test]
		public void IsValidWhenEmpty()
		{
			Assert.IsTrue(new Notification().IsValid);
		}

		[Test]
		public void IsNotValidWhenNotEmpty()
		{
			var notification = new Notification();
			notification.Register("field", "message");
			Assert.IsFalse(notification.IsValid);
		}
        
		[Test]
		public void EmptyGetMessages()
		{
			var notification = new Notification();
			Assert.AreEqual(0, notification.GetMessagesFor("field").Length);
		}
        
		[Test]
		public void GetMessages()
		{
			var notification = new Notification();
			notification.Register("field1", "message1");
			notification.Register("field2", "message2");
			notification.Register("field1", "message3");
            
			var messages = notification.GetMessagesFor("field1");
			Assert.AreEqual(new[] {"message1", "message3"}, messages);
		}
        
		[Test]
		public void HasMessage()
		{
			var notification = new Notification();
			Assert.IsFalse(notification.HasMessage("field", "message"), "message should not exist yet");
			notification.Register("field", "message");
			Assert.IsTrue(notification.HasMessage("field", "message"), "message should exist");
		}
        
		[Test]
		public void GetJoinedMessage()
		{
			var notification = new Notification();
			notification.Register("field1", "message1");
			notification.Register("field1", "message2");
			Assert.AreEqual("message1, message2", notification.GetMessageFor("field1", ", "));
		}
        
		[Test]
		public void FieldsWithErrors()
		{
			var notification = new Notification();
			notification.Register("field1", "message1");
			notification.Register("field2", "message2");
			Assert.AreEqual(new[] {"field1", "field2"}, notification.FieldsWithMessages);
		}
	}
}