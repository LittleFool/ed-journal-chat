using System.Text.Json.Serialization;

namespace ed_journal_chat.Journal
{
    public class JournalReceiveText : JournalBase
    {
        public string From { get; set; }
        public string Message { get; set; }
        public string? Channel { get; set; }
    }
}
