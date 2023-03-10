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
            Console.WriteLine("Press 'h' for Help!");

            if (Config.ActiveJournalFile != null)
            {
                JournalReader.ParseJournalFile(Config.ActiveJournalFile.FullName);
                JournalReader.RunWatcher();
            }

            char? input = null;

            while (!input.Equals('q'))
            {
                input = Console.ReadKey().KeyChar;

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("   ");
                Console.SetCursorPosition(0, Console.CursorTop);

                switch (input)
                {
                    case ' ':
                        if (CMDR.FSDTarget != null)
                        { 
                            SetClipboard(CMDR.FSDTarget.Name);
                            DisplayOutput.FSDTarget(CMDR.FSDTarget);
                        } else
                        {
                            Console.WriteLine("No targeted system found!");
                        }
                        break;

                    case 's':
                        SetClipboard(CMDR.LastSentText); break;
                    case 'ы':
                        SetClipboard(CMDR.LastSentText); break;

                    case 'r':
                        SetClipboard(CMDR.LastReceivedText); break;
                    case 'к':
                        SetClipboard(CMDR.LastReceivedText); break;

                    case 'h':
                        PrintHelp(); break;
                }
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

                Console.WriteLine("'q' close the program");
                Console.WriteLine("'space'\tcopy target system to clipboard");
                Console.WriteLine("'s'\tcopy last sent message to clipboard");
                Console.WriteLine("'r'\tcopy last received message to clipboard");
                Console.WriteLine("'h'\topen and close this page");
                Console.WriteLine();

                Console.WriteLine("Chat colors are as follows:");
                foreach(KeyValuePair<string, ConsoleColor> entry in DisplayOutput.ChatColors)
                {
                    Console.ForegroundColor = entry.Value;
                    Console.WriteLine(DisplayOutput.ChannelTranslations[entry.Key]);
                }

                Console.ForegroundColor = ConsoleColor.Gray;
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
