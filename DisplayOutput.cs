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
        public static readonly Dictionary<string, ConsoleColor> ChatColors = new();
        public static readonly Dictionary<string, string> ChannelTranslations = new();

        static DisplayOutput()
        {
            ChatColors.Add("local", ConsoleColor.Yellow);
            ChatColors.Add("player", ConsoleColor.DarkYellow);
            ChatColors.Add("wing", ConsoleColor.Cyan);
            ChatColors.Add("starsystem", ConsoleColor.DarkRed);
            ChatColors.Add("squadron", ConsoleColor.Green);
            ChatColors.Add("squadleaders", ConsoleColor.Green);
            ChatColors.Add("voicechat", ConsoleColor.Magenta);
            ChatColors.Add("chat", ConsoleColor.Blue);

            ChannelTranslations.Add("chat", "multicrew");
            ChannelTranslations.Add("local", "local");
            ChannelTranslations.Add("starsystem", "system");
            ChannelTranslations.Add("wing", "team");
            ChannelTranslations.Add("squadron", "squadron");
            ChannelTranslations.Add("squadleaders", "squad leaders");
            ChannelTranslations.Add("voicechat", "voicechat");
            ChannelTranslations.Add("player", "direct");
        }

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
            if (ChatColors.ContainsKey(journalObject.To))
            {
                Console.ForegroundColor = ChatColors[journalObject.To];
            } else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
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
            // null = MC chat
            if (journalObject.Channel == null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            } else if (ChatColors.ContainsKey(journalObject.Channel))
            {
                Console.ForegroundColor = ChatColors[journalObject.Channel];
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

        public static void FSDTarget(JournalFSDTarget journalFSDTarget)
        {
            Console.WriteLine("Copied system to clipboard: " + journalFSDTarget.Name);
        }

        public static void Shutdown(JournalShutdown journalShutdown)
        {
            Console.WriteLine(journalShutdown.timestamp.TimeOfDay + " - Game Closed");
        }
    }
}
