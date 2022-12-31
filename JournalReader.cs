using ed_journal_chat.Journal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class JournalReader
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

                    string pattern = "\"event\":\"\\w+\", ";
                    Match m = Regex.Match(line, pattern);
                    if (m.Success)
                    {
                        line = line.Replace(m.Value, "");
                        line = line.Replace("{ ", "{ " + m.Value); 
                    }

                    var value = JsonSerializer.Deserialize<JournalBase>(line);

                    if (value is JournalSendText sendText)
                    {
                        if (sendText.Sent)
                            Console.WriteLine("Me to " + sendText.To + ": " + sendText.Message);
                    }

                    line = sr.ReadLine();
                }

                ReadLinesCount = CurrentLine;
            }
        }
    }
}
