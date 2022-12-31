using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ed_journal_chat.Journal
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "event", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType, IgnoreUnrecognizedTypeDiscriminators = true)]
    [JsonDerivedType(typeof(JournalBase), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(JournalSendText), typeDiscriminator: "SendText")]
    public class JournalBase
    {
        public DateTime timestamp { get; set; }

        [JsonPropertyName("event")]
        public string jsonEvent { get; set; }
    }
}
