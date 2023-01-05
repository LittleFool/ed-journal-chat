using ed_journal_chat.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class DisplayOutput
    {
        public static void Fileheader(JournalFileheader journalFileheader)
        {
            string mode;
            if (journalFileheader.Odyssey)
            {
                mode = "Live";
                CMDR.Legacy = false;
            } else
            {
                mode = "Legacy";
                CMDR.Legacy = true;

                if (!JournalReader.LegacyModeOn)
                    return;
            }

            Console.WriteLine("This is " + mode + " version " + journalFileheader.gameversion + " build " + journalFileheader.build);
        }

        public static void Commander(JournalCommander journalCommander)
        {
            if (CMDR.Name == null)
            {
                Console.WriteLine("CMDR " + journalCommander.Name + " (" + journalCommander.FID + ")");
                CMDR.Name = journalCommander.Name;
            }
        }

        public static void SendText(JournalSendText journalObject)
        {
            switch (journalObject.To)
            {
                case "chat": Console.ForegroundColor = ConsoleColor.Blue; break;
                case "local": Console.ForegroundColor = ConsoleColor.Yellow; break;
                case "starsystem": Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case "wing": Console.ForegroundColor = ConsoleColor.Cyan; break;
                case "squadron": Console.ForegroundColor = ConsoleColor.Green; break;
                case "squadleaders": Console.ForegroundColor = ConsoleColor.Green; break;
                case "voicechat": Console.ForegroundColor = ConsoleColor.Magenta; break;
                default: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
            }

            string cmdrName;
            if (CMDR.Name == null)
                cmdrName = "Me";
            else
                cmdrName = CMDR.Name;

            if (journalObject.Sent)
            {
                Console.WriteLine(journalObject.timestamp.TimeOfDay + " - " + cmdrName + ":\t" + journalObject.Message);
                CMDR.LastSentText = journalObject.Message;
            }
                
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void ReceiveText(JournalReceiveText journalObject)
        {
            string? channel = journalObject.Channel;
            switch(journalObject.Channel)
            {
                case null: Console.ForegroundColor = ConsoleColor.Blue; break;
                case "player": Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case "local": Console.ForegroundColor = ConsoleColor.Yellow; break;
                case "starsystem": Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case "wing": Console.ForegroundColor = ConsoleColor.Cyan; break;
                case "squadron": Console.ForegroundColor = ConsoleColor.Green; break;
                case "squadleaders": Console.ForegroundColor = ConsoleColor.Green; break;
                case "voicechat": Console.ForegroundColor = ConsoleColor.Magenta; break;
            }

            // dont display NPC messages e.g. Cruise Ship, Entering System notifications and so on
            if (journalObject.Channel != null && journalObject.Channel.Equals("npc"))
                return;

            Console.WriteLine(journalObject.timestamp.TimeOfDay + " - " + journalObject.From + ":\t" + journalObject.Message);
            CMDR.LastReceivedText = journalObject.Message;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Friends(JournalFriends journalFriends)
        {
            switch(journalFriends.Status)
            {
                case "Requested":
                    Console.WriteLine(journalFriends.timestamp.TimeOfDay + " - friend request from " + journalFriends.Name); break;

                case "Added":
                    Console.WriteLine(journalFriends.timestamp.TimeOfDay + " - now friends with " + journalFriends.Name); break;
            }
        }

        public static void WingInvite(JournalWingInvite journalWingInvite)
        {
            Console.WriteLine(journalWingInvite.timestamp.TimeOfDay + " - team invite from " + journalWingInvite.Name);
        }

        public static void Shutdown(JournalShutdown journalShutdown)
        {
            Console.WriteLine(journalShutdown.timestamp.TimeOfDay + " - Game Closed");
        }
    }
}
