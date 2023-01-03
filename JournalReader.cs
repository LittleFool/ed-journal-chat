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

        public static void RunWatcher()
        {
            if (!Directory.Exists(Config.JournalPath))
            {
                throw new DirectoryNotFoundException(Config.JournalPath);
            }

            if (Config.ActiveJournalFile == null)
            {
                throw new Exception("Config.ActiveJournalFile not set");
            }

            FileSystemWatcher watcher = new();
            watcher.Path = Config.JournalPath;
            watcher.NotifyFilter = NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite;
            watcher.Filter = Config.ActiveJournalFile.Name;
            watcher.Changed += ParseJournalFile;
            watcher.EnableRaisingEvents = true;
        }

        private static void ParseJournalFile(object sender, FileSystemEventArgs e)
        {
            ParseJournalFile(e.FullPath);
        }

        public static void ParseJournalFile(string? FullPath)
        {
            if (FullPath == null || !File.Exists(FullPath))
            {
                throw new FileNotFoundException();
            }

            var fs = new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                        }
                        else
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
                        case JournalFileheader:
                            DisplayOutput.Fileheader((JournalFileheader)value); break;

                        case JournalCommander:
                            DisplayOutput.Commander((JournalCommander)value); break;

                        case JournalSendText:
                            DisplayOutput.SendText((JournalSendText)value); break;

                        case JournalReceiveText:
                            DisplayOutput.ReceiveText((JournalReceiveText)value); break;

                        case JournalFriends:
                            DisplayOutput.Friends((JournalFriends)value); break;

                        case JournalWingInvite:
                            DisplayOutput.WingInvite((JournalWingInvite)value); break;

                        case JournalShutdown:
                            DisplayOutput.Shutdown((JournalShutdown)value); break;
                    }

                    line = sr.ReadLine();
                }

                ReadLinesCount = CurrentLine;
            }
        }

        public static void Read()
        {
            if (!Directory.Exists(Config.JournalPath))
            {
                throw new DirectoryNotFoundException(Config.JournalPath);
            }

            DateTime newestFileTime = DateTime.MinValue;
            string? newestFilePath = null;
            string[] fileEntries = Directory.GetFiles(Config.JournalPath, Config.JournalFileSearchPattern);
            foreach (string fileEntry in fileEntries)
            {
                DateTime currentFileTime = File.GetLastWriteTime(fileEntry);
                if (currentFileTime > newestFileTime)
                {
                    newestFileTime = currentFileTime;
                    newestFilePath= fileEntry;
                }
            }

            if (newestFileTime == DateTime.MinValue)
            {
                throw new Exception("No file matches the filter!");
            }

            ParseJournalFile(newestFilePath);
        }
    }
}
