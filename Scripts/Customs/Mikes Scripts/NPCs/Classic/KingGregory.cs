using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of King Gregory")]
    public class KingGregory : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KingGregory() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "King Gregory";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 110;
            Int = 80;
            Hits = 85;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1428 });
            AddItem(new StuddedChest() { Hue = 1428 });
            AddItem(new StuddedGloves() { Hue = 1428 });
            AddItem(new Bandana() { Hue = 1428 });
            AddItem(new Boots() { Hue = 1428 });
            AddItem(new Bow() { Name = "King Gregory's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, I am King Gregory, ruler of this land.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as my commitment to the virtues.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to uphold the virtues and ensure justice prevails in my kingdom.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues are the foundation of a just society. Do you understand their importance?");
            }
            else if (speech.Contains("yes"))
            {
                Say("The virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each is vital for a just and harmonious world.");
            }
            else if (speech.Contains("land"))
            {
                Say("This land, known as Britannia, has been under the rule of my lineage for generations. It's a place of magic and mystery, where heroes rise and legends are born.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("My commitment extends beyond just my health. It is to the prosperity and safety of all who reside within Britannia. An ancient artifact was stolen from our kingdom, which could threaten our peace.");
            }
            else if (speech.Contains("justice"))
            {
                Say("True justice can sometimes be a complicated matter. There are threats from dark forces, conspiracies, and challenges that test my judgment daily. Among them is the theft of the ancient artifact I mentioned.");
            }
            else if (speech.Contains("importance"))
            {
                Say("The importance of the virtues cannot be understated. They guide our moral compass and dictate how we interact with one another. To truly grasp their significance, one must strive to embody them.");
            }
            else if (speech.Contains("britannia"))
            {
                Say("Britannia has eight cities, each representing one of the virtues. Traveling through them will give you a deeper understanding of our values.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("The artifact, known as the Ankh of Virtues, has the power to amplify the virtues in one's heart or reveal their deepest vices. I fear it may be misused. If you can retrieve it, I shall reward you handsomely.");
            }
            else if (speech.Contains("dark"))
            {
                Say("These forces emerge from the Abyss, seeking to corrupt the land and its inhabitants. They are drawn to the power of the Ankh and may be behind its theft.");
            }
            else if (speech.Contains("embody"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To embody the virtues is to live by them daily. It's a path of challenges, but also of great rewards, both spiritually and in the material world. Take this!");
                    from.AddToBackpack(new LowerAttackAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
        }

        public KingGregory(Serial serial) : base(serial) { }

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
