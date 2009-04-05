namespace Validations
{
	public class NotificationMessage
	{
		public NotificationMessage(string fieldName, string message)
		{
			FieldName = fieldName;
			Message = message;
		}

		public string FieldName { get; set; }
		public string Message { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as NotificationMessage;
			if (other == null) return false;
			return Equals(FieldName, other.FieldName) 
			       && Equals(Message, other.Message);
		}

		public override int GetHashCode()
		{
			return FieldName.GetHashCode() ^ Message.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Field {0}: {1}", FieldName, Message);
		}
	}
}