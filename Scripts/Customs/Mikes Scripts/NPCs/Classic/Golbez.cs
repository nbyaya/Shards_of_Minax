using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Golbez")]
    public class Golbez : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Golbez() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Golbez";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 100;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 1109 }); // Robe with hue 1109
            AddItem(new Cloak() { Hue = 1109 }); // Cloak with hue 1109
            AddItem(new Sandals() { Hue = 1109 }); // Sandals with hue 1109
            AddItem(new SkullCap() { Hue = 1109 }); // SkullCap with hue 1109
            AddItem(new Spellbook() { Name = "Golbez's Grimoire" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Golbez, a seeker of power!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health!");
            }
            else if (speech.Contains("job"))
            {
                Say("I seek power and control over the elements!");
            }
            else if (speech.Contains("power") && speech.Contains("darkness"))
            {
                Say("True power lies in the mastery of one's inner darkness. Do you understand this concept?");
            }
            else if (speech.Contains("yes") && speech.Contains("darkness"))
            {
                Say("Then you have much to learn. Seek the balance between light and darkness.");
            }
            else if (speech.Contains("golbez"))
            {
                Say("Many fear my name, but few understand the depth of my quest. Have you encountered the Crystal of Shadows?");
            }
            else if (speech.Contains("perfect"))
            {
                Say("My health may be impeccable, but my spirit constantly battles between light and shadow. Are you aware of the Ritual of Ascension?");
            }
            else if (speech.Contains("control"))
            {
                Say("While I seek to control the elements, my true goal is the legendary Tome of the Ancients. Do you know of its location?");
            }
            else if (speech.Contains("power"))
            {
                Say("Power is not just about strength. It's about understanding, wisdom, and sacrifice. Ever heard of the Abyssal Chalice?");
            }
            else if (speech.Contains("darkness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Embracing one's darkness is a path to true enlightenment. As a token of our conversation, I grant you this small reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("yes"))
            {
                Say("Your acknowledgment is appreciated. Remember, the equilibrium between light and shadow is crucial. Seek the Oracle of Equilibrium for guidance.");
            }

            base.OnSpeech(e);
        }

        public Golbez(Serial serial) : base(serial) { }

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
