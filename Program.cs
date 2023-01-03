// See https://aka.ms/new-console-template for more information
using ed_journal_chat;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Options.SelectJournalFile();

if (Config.ActiveJournalFile != null)
{
    JournalReader.ParseJournalFile(Config.ActiveJournalFile.FullName);
    JournalReader.RunWatcher();
}

while(true)
{
    System.Threading.Thread.Sleep(1000);
}
