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
            FileInfo[] H4Files = journalDir.GetFiles(Config.JournalFileSearchPatternH4).OrderByDescending(p => p.LastWriteTime).ToArray();
            FileInfo[] H3Files = journalDir.GetFiles(Config.JournalFileSearchPatternH3).OrderByDescending(p => p.LastWriteTime).ToArray();

            int i = 0;
            int j = 0;

            for (i = 0; i < H4Files.Length && i < 5; i++)
            {
                Console.WriteLine("[" + i + "] " + H4Files[i].Name);
            }

            Console.WriteLine(new String('-', 33));

            for (j = 0; j < H3Files.Length && j < 5; j++)
            {
                Console.WriteLine("[" + (i + j) + "] " + H3Files[j].Name);
            }

            Console.Write("Select the journal file to use: ");
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            int index = (int) Char.GetNumericValue(input);
            if (index < 0 || index > i+j)
            {
                SelectJournalFile();
                return;
            }

            if (index < i)
            {
                Config.ActiveJournalFile = H4Files[index];
            } else
            {
                Config.ActiveJournalFile = H3Files[index - i];
            }

            Console.Clear();
        }
    }
}
