using System.Text;

namespace ed_journal_chat
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Options.SelectJournalFile();

            if (Config.ActiveJournalFile != null)
            {
                JournalReader.ParseJournalFile(Config.ActiveJournalFile.FullName);
                JournalReader.RunWatcher();
            }

            char? input = null;

            while (!input.Equals('q'))
            {
                input = Console.ReadKey().KeyChar;

                switch (input)
                {
                    case ' ':
                        SetClipboard(CMDR.FSDTarget); break;
                    case 's':
                        SetClipboard(CMDR.LastSentText); break;
                    case 'r':
                        SetClipboard(CMDR.LastReceivedText); break;
                }

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("   ");
                Console.SetCursorPosition(0, Console.CursorTop);
            }

        }

        private static void SetClipboard(string? s)
        {
            if (s != null && s.Length > 0)
            {
                Clipboard.SetText(s);
            }
        }
    }
}
