using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Prospector Pat Goldrush")]
    public class ProspectorPatGoldrush : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProspectorPatGoldrush() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Prospector Pat Goldrush";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Boots() { Hue = 0x8A }); // Dark brown boots
            AddItem(new LongPants() { Hue = 0x8A }); // Dark brown pants
            AddItem(new FancyShirt() { Hue = 0xB4 }); // Light brown shirt
            AddItem(new LeatherChest() { Hue = 0x8A }); // Dark brown chest armor
            AddItem(new TricorneHat() { Hue = 0x3F }); // Light brown hat
            AddItem(new Pickaxe() { Hue = 0x3F }); // Mining pickaxe as a prop

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x203B); // Long hair or a short hairstyle
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Howdy, partner! I'm Prospector Pat Goldrush, at your service.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as fit as a fiddle, just like the day I struck gold!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to sift through the sands for gold and share my finds with deserving adventurers.");
            }
            else if (speech.Contains("gold"))
            {
                Say("Ah, gold! It's a treasure worth its weight. But it takes perseverance to find it. Ask me more if you're curious!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("There's treasure aplenty if you know where to look. Care to test your luck and skill?");
            }
            else if (speech.Contains("test"))
            {
                Say("To find your fortune, you must show patience and skill. Prove your worth, and a grand reward awaits!");
            }
            else if (speech.Contains("worth"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've already given out all the rewards I can for now. Come back later!");
                }
                else
                {
                    Say("You've proven your worth with your questions. For your perseverance, take this Gold Rush Bounty Chest as a token of my esteem.");
                    from.AddToBackpack(new GoldRushBountyChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("adventure"))
            {
                Say("An adventure always promises excitement! Whether it's hunting for gold or exploring new lands, there's always a story to tell.");
            }
            else if (speech.Contains("story"))
            {
                Say("I've got a tale or two about the wild gold rush days. It was a time of great fortune and fierce competition.");
            }
            else if (speech.Contains("competition"))
            {
                Say("Indeed! Many a prospector faced fierce competition. But with persistence, one could strike it rich.");
            }
            else if (speech.Contains("persistence"))
            {
                Say("Persistence is key! Even when the gold eludes you, you must keep searching and never give up.");
            }
            else if (speech.Contains("search"))
            {
                Say("Searching for gold isn't just about digging; it's about knowing where to look. Keep your eyes peeled and your wits sharp!");
            }
            else if (speech.Contains("wits"))
            {
                Say("Yes, your wits are crucial. The clever prospector uses both tools and intellect to find the best spots.");
            }
            else if (speech.Contains("tools"))
            {
                Say("A good pickaxe and a trusty lantern are essential tools for any prospector. They light the way to riches.");
            }
            else if (speech.Contains("lantern"))
            {
                Say("A lantern can be a lifesaver in dark mines. It helps you see what others might miss and guides you through your journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey is filled with challenges and discoveries. Embrace them, and you'll find more than just gold.");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("Ah, discoveries! They can be just as valuable as gold. New places, hidden secrets, and forgotten lore await those who seek.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets of the land, treasures hidden in plain sight, and ancient legends—all part of the thrill of the hunt.");
            }
            else if (speech.Contains("legends"))
            {
                Say("Legends tell of mighty treasures and epic adventures. They inspire many to seek the glory and gold of the old days.");
            }
            else if (speech.Contains("glory"))
            {
                Say("Glory comes not just from wealth, but from the journey and the stories you gather along the way.");
            }
            else if (speech.Contains("stories"))
            {
                Say("Each story adds to the rich tapestry of a prospector's life. Share your own stories, and you'll find camaraderie with fellow adventurers.");
            }
            else if (speech.Contains("camaraderie"))
            {
                Say("Camaraderie among adventurers is valuable. It’s the bond that helps you through tough times and celebrates victories together.");
            }

            base.OnSpeech(e);
        }

        public ProspectorPatGoldrush(Serial serial) : base(serial) { }

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
