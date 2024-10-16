using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Archer Quinn")]
    public class ArcherQuinn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ArcherQuinn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Archer Quinn";
            Body = 0x191; // Human male body

            // Stats
            Str = 90;
            Dex = 80;
            Int = 70;
            Hits = 80;

            // Appearance
            AddItem(new LeatherCap() { Hue = 134 }); // The color could be adjusted
            AddItem(new LeatherChest() { Hue = 134 });
            AddItem(new LeatherLegs() { Hue = 134 });
            AddItem(new LeatherGloves() { Hue = 134 });
            AddItem(new ThighBoots() { Hue = 134 });
            AddItem(new CompositeBow() { Name = "Quinn's Bow" });
            
            // Speech Hue
            SpeechHue = 1152; // Choose an appropriate speech hue
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
                Say("Greetings, I am Archer Quinn, master of the bow.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in peak condition, ready for any challenge.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to perfect my archery skills and aid fellow archers.");
            }
            else if (speech.Contains("archery"))
            {
                Say("Ah, the art of archery! It requires precision and patience.");
            }
            else if (speech.Contains("bow"))
            {
                Say("The bow is not just a weapon but a companion. Treat it well.");
            }
            else if (speech.Contains("precision"))
            {
                Say("Precision is key in archery. Only with it can you hit your target accurately.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Return later.");
                }
                else
                {
                    Say("Your understanding of archery has impressed me. Take this chest, it holds great items for an archer like yourself.");
                    from.AddToBackpack(new ArcheryBonusChest()); // Give the custom chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am a humble archer, skilled in the ways of the bow and arrow. If you wish to discuss archery, just ask.");
            }

            base.OnSpeech(e);
        }

        public ArcherQuinn(Serial serial) : base(serial) { }

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
