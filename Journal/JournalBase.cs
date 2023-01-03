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
    [JsonDerivedType(typeof(JournalFileheader), typeDiscriminator: "Fileheader")]
    [JsonDerivedType(typeof(JournalCommander), typeDiscriminator: "Commander")]
    [JsonDerivedType(typeof(JournalSendText), typeDiscriminator: "SendText")]
    [JsonDerivedType(typeof(JournalReceiveText), typeDiscriminator: "ReceiveText")]
    [JsonDerivedType(typeof(JournalFriends), typeDiscriminator: "Friends")]
    [JsonDerivedType(typeof(JournalWingInvite), typeDiscriminator: "WingInvite")]
    [JsonDerivedType(typeof(JournalFSDTarget), typeDiscriminator: "FSDTarget")]
    [JsonDerivedType(typeof(JournalShutdown), typeDiscriminator: "Shutdown")]
    public class JournalBase
    {
        public DateTime timestamp { get; set; }
    }
}
