using MudBlazor;

namespace tcgct_mud.Data
{
	public class Message
	{
        public Guid ID { get; private set; }
        public Severity Severity { get; set; }
        public string Text { get; set; }
		public Message(Severity severity, string text)
		{
			ID = Guid.NewGuid();
			Severity = severity;
			Text = text;
		}
	}
}
