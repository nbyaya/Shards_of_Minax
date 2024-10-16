using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Guardian Angel")]
    public class GuardianAngel : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GuardianAngel() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Guardian Angel";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 60;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new WizardsHat() { Hue = 1153 });

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
                Say("I am the Guardian Angel, protector of sacred treasures.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard and bestow the blessings of the angels upon the worthy.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in divine health, thanks to the celestial energies.");
            }
            else if (speech.Contains("blessing"))
            {
                Say("A blessing from the angels is a rare gift indeed. To earn it, you must prove your worthiness.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove yourself, you must speak of virtues. Share your thoughts on kindness and courage.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Kindness is the light that guides us through the darkest times. It heals and comforts those around us.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to face our fears and challenges. It fuels our determination and fortitude.");
            }
            else if (speech.Contains("blessings"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait a little longer before you can receive another blessing.");
                }
                else
                {
                    Say("You have shown great wisdom and virtue. Accept this Angel’s Blessing Chest as a reward for your deeds.");
                    from.AddToBackpack(new AngelBlessingChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public GuardianAngel(Serial serial) : base(serial) { }

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
