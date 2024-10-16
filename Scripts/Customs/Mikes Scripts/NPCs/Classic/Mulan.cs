using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mulan")]
    public class Mulan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Mulan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mulan";
            Body = 0x190; // Human female body

            // Stats
            Str = 90;
            Dex = 90;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new ChainChest() { Hue = 1197 });
            AddItem(new ChainLegs() { Hue = 1197 });
            AddItem(new ChainCoif() { Hue = 1197 });
            AddItem(new PlateGloves() { Hue = 1197 });
            AddItem(new Boots() { Hue = 1197 });
            AddItem(new Bow() { Name = "Mulan's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public Mulan(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Mulan, a traveler from distant lands. How may I assist you today?");

            greeting.AddOption("Tell me about your travels.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Ah, my travels have taken me through many wondrous lands! I have seen lush valleys, treacherous mountains, and enchanted forests. Each place holds its own mysteries and stories.")));
                });

            greeting.AddOption("What do you know about alchemy?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Alchemy is a fascinating blend of science and magic! It involves transforming matter and understanding the properties of various elements. I’ve learned a few things along the way, though I am not an expert. Would you like to know something specific?")));
                });

            greeting.AddOption("Can you share a story?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Certainly! There was a time when I crossed paths with a wise old sage who spoke of the stars and their influence on our lives. He taught me that our fates are intertwined with the cosmos, guiding us toward our destinies. It was a humbling experience.")));
                });

            greeting.AddOption("What do you do for fun?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("In my free time, I love to gather stories from travelers like yourself. I enjoy writing and crafting tales of heroism and adventure, sometimes even performing them for others around a warm fire.")));
                });

            greeting.AddOption("Do you know any local legends?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Indeed! One popular legend speaks of a hidden treasure guarded by a fierce dragon in the Misty Mountains. Many have tried to find it, but none have returned. It’s said that only the pure of heart can pass through the dragon’s defenses.")));
                });

            greeting.AddOption("What do you think of this land?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("This land is rich in culture and history. The people are resilient, and the landscapes are breathtaking. However, I sense a tension brewing beneath the surface, a struggle that could change everything.")));
                });

            greeting.AddOption("Can you offer me wisdom?",
                player => true,
                player =>
                {
                    if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later for more wisdom.")));
                    }
                    else
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("May your heart remain true, and may you always follow the path that brings you joy. Here, take this small token of my appreciation.")));
                        player.AddToBackpack(new Gold(1000)); // Example reward, adjust as needed
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                });

            greeting.AddOption("Tell me about your favorite adventure.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("One of my favorite adventures was when I traversed the Whispering Forest at dawn. The trees whispered secrets, and the light danced on the leaves. It felt as if the forest was alive, guiding me to an ancient shrine long forgotten by time.")));
                });

            greeting.AddOption("What do you know about the local wildlife?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("The wildlife here is as varied as the landscapes. From the gentle deer that roam the fields to the fierce wolves that hunt in the shadows, each creature plays a role in this delicate ecosystem. Respect them, and they will respect you.")));
                });

            greeting.AddOption("What advice would you give to a traveler?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Always be prepared, both mentally and physically. Keep an open mind, for you never know when a chance encounter might change your life. And remember, kindness often opens doors that strength cannot.")));
                });

            greeting.AddOption("Tell me about your family.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I come from a small village far from here. My family is dear to me, and they taught me the values of honor and courage. Though I may be far from home, their lessons guide me wherever I go.")));
                });

            greeting.AddOption("Do you have any hobbies?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I enjoy storytelling and crafting tales that inspire others. I also collect small trinkets from my travels, each with its own story to tell. It reminds me of the adventures I've had and the friends I've made along the way.")));
                });

            greeting.AddOption("Tell me about the stars.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("The stars have always been a source of wonder for me. They guide us in the darkest of nights and tell tales of ancient heroes. I often find myself gazing at them, pondering what lies beyond our world.")));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Safe travels, dear friend. May your path be filled with light and adventure!")));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
