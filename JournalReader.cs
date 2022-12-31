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
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string? line = sr.ReadLine();
                int CurrentLine = 0;

                while (line != null)
                {
                    CurrentLine++;

                    // Skip the lines we already read, or empty lines
                    if (CurrentLine <= ReadLinesCount || line.Equals(""))
                    {
                        line = sr.ReadLine();
                        continue;
                    }

                    // move the event attribute to the front of the json string
                    string pattern = "\"event\":\"\\w+\",? ";
                    Match m = Regex.Match(line, pattern);
                    if (m.Success)
                    {
                        // remove the event attribute
                        line = line.Replace(m.Value, "");

                        if (m.Value.EndsWith(", "))
                        {
                            // add the event attribute at the front
                            line = line.Replace("{ ", "{ " + m.Value);
                        } else
                        {
                            // no attribute after event, we need to fix the ","
                            line = line.Replace("{ ", "{ " + m.Value.Trim() + ", ");
                            line = line.Replace(", }", " }");
                        }
                    }

                    // Deserialize the json string
                    var readOnlySpan = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(line));
                    var value = JsonSerializer.Deserialize<JournalBase>(readOnlySpan);

                    // determine which class we got
                    switch (value)
                    {
                        case JournalSendText:
                            DisplayOutput.SendText((JournalSendText)value); break;

                        case JournalReceiveText:
                            DisplayOutput.ReceiveText((JournalReceiveText)value); break;

                        case JournalShutdown:
                            Console.WriteLine(value.timestamp + " Game Closed"); break;
                    }

                    line = sr.ReadLine();
                }

                ReadLinesCount = CurrentLine;
            }
        }
    }
}
