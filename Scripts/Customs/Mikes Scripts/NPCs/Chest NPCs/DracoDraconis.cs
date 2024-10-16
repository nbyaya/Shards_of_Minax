using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Draco Draconis")]
    public class DracoDraconis : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DracoDraconis() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Draco Draconis";
            Body = 0x190; // Human male body
            Title = "the Guardian";

            // Stats
            Str = 100;
            Dex = 70;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new Cloak() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1157 });

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
                Say("Greetings, traveler. I am Draco Draconis, the guardian of the Dragon's Hoard.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as strong as the dragons I guard.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to protect the Dragon Guardian's treasures.");
            }
            else if (speech.Contains("dragon"))
            {
                Say("The dragons are wise and powerful. They have entrusted me with their treasures.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The treasures I guard are not for the faint-hearted. Only the worthy may claim them.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have proven yourself worthy, but the treasures are still gathering their magic. Please return later.");
                }
                else
                {
                    Say("You have proven your worthiness. Accept this Dragon Guardian's Hoard as your reward.");
                    from.AddToBackpack(new DragonGuardiansHoardChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DracoDraconis(Serial serial) : base(serial) { }

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
