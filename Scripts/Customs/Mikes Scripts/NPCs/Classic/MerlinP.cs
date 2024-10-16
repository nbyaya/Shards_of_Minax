using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of MerlinP")]
    public class MerlinP : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MerlinP() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "MerlinP";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1109 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Spellbook() { Name = "MerlinP's Spellbook" });
            AddItem(new GnarledStaff() { Name = "MerlinP's Staff" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am MerlinP the Wizard.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the currents of time.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role in this world is to unravel the mysteries of magic.");
            }
            else if (speech.Contains("magic") || speech.Contains("arcane arts"))
            {
                Say("Powerful magic can change the course of destiny. Are you drawn to the arcane arts?");
            }
            else if (speech.Contains("yes") || speech.Contains("knowledge") || speech.Contains("wield wisely"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Then seek knowledge and wield it wisely, for magic is a double-edged sword.");
                    // Give a reward (example)
                    from.AddToBackpack(new Gold(1000)); // Replace with actual reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MerlinP(Serial serial) : base(serial) { }

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
