using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class Options
    {
        public static void SelectJournalFile()
        {
            Console.Clear();
            DirectoryInfo journalDir = new DirectoryInfo(Config.JournalPath);
            FileInfo[] files = journalDir.GetFiles(Config.JournalFileSearchPattern).OrderByDescending(p => p.LastWriteTime).ToArray();

            for (int i = 0; i < files.Length && i < 5; i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + files[i].Name);
            }

            Console.Write("Select the journal file to use: ");
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            int index = (int) Char.GetNumericValue(input);
            if (index == -1)
            {
                SelectJournalFile();
                return;
            }

            Config.ActiveJournalFIle = files[index - 1].FullName;
            Console.Clear();
        }
    }
}
