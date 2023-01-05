using System.Text;

namespace ed_journal_chat
{
    internal class Program
    {
        private static bool HelpMode = false;

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
                    case 'h':
                        PrintHelp(); break;
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

        private static void PrintHelp()
        {
            HelpMode = !HelpMode;

            if (HelpMode)
            {
                JournalReader.StopWatcher();
                Console.Clear();

                Console.WriteLine("'space'\tcopy target system to clipboard");
                Console.WriteLine("'s'\tcopy last sent message to clipboard");
                Console.WriteLine("'r'\tcopy last received message to clipboard");
                Console.WriteLine("'h'\topen and close this page");
            } else
            {
                Console.Clear();
                
                if (Config.ActiveJournalFile != null)
                {
                    JournalReader.ContinueWatcher();
                    JournalReader.ParseJournalFile(Config.ActiveJournalFile.FullName);
                }
            }

            
        }
    }
}
