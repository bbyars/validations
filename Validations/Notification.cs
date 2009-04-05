using System.Collections.Generic;
using System.Linq;

namespace Validations
{
	public class Notification
	{
		private readonly List<NotificationMessage> messages = new List<NotificationMessage>();
        
		public bool IsValid
		{
			get { return messages.Count == 0; }
		}
        
		public void Register(string fieldName, string message)
		{
			messages.Add(new NotificationMessage(fieldName, message));    
		}
        
		public string[] GetMessagesFor(string fieldName)
		{
			return messages.Where(message => message.FieldName == fieldName)
				.Select(message => message.Message).ToArray();
		}
        
		public string GetMessageFor(string fieldName, string messageSeparator)
		{
			return string.Join(messageSeparator, GetMessagesFor(fieldName));
		}
        
		public bool HasMessage(string fieldName, string message)
		{
			var expected = new NotificationMessage(fieldName, message);
			return messages.Exists(actual => actual.Equals(expected));
		}
        
		public string[] FieldsWithMessages
		{
			get { return messages.Select(message => message.FieldName).Distinct().ToArray(); }
		}
	}
}