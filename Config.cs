﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class Config
    {
        public static readonly string JournalPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\Frontier Developments\\Elite Dangerous";
        public static readonly string JournalFileSearchPatternH4 = "Journal.????-??-??T??????.??.log";
        public static readonly string JournalFileSearchPatternH3 = "Journal.????????????.??.log";
        public static FileInfo? ActiveJournalFile { get; set; }
    }
}
