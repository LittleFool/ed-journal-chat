using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class Reader
    {
        private static int ReadLinesCount = 0;
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\Frontier Developments\\Elite Dangerous";
        public static void RunWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite;
            watcher.Filter = "Journal.????-??-??T??????.??.log";
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                string? line = sr.ReadLine();
                int CurrentLine = 0;

                while (line != null)
                {
                    CurrentLine++;

                    if (CurrentLine <= ReadLinesCount || line.Equals(""))
                    {
                        line = sr.ReadLine();
                        continue;
                    }

                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }

                ReadLinesCount = CurrentLine;
            }
        }
    }
}
