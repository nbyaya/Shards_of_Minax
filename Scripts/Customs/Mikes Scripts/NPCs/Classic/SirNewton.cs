using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Newton")]
    public class SirNewton : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirNewton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Newton";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 75;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new LongPants() { Hue = 1122 });
            AddItem(new Tunic() { Hue = 1122 });
            AddItem(new Boots() { Hue = 1122 });
            AddItem(new Spellbook() { Name = "Principia Mathematica" });

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
                Say("I am Sir Newton, the great philosopher. What do you want, peasant?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Bah! My mind is my only concern.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job'? I am a thinker, not a common laborer!");
            }
            else if (speech.Contains("philosophy") || speech.Contains("wisdom") || speech.Contains("questioning"))
            {
                Say("True wisdom lies in questioning everything. Are you capable of it?");
            }
            else if (speech.Contains("ignorance") || speech.Contains("begone"))
            {
                Say("Your response reeks of ignorance, as expected. Begone!");
            }
            else if (speech.Contains("philosopher"))
            {
                Say("Ah, philosophy! The love of wisdom. It has been my lifelong pursuit to unravel the mysteries of existence.");
            }
            else if (speech.Contains("mind"))
            {
                Say("The mind is a powerful tool. I've always believed that a sharp mind is more valuable than the strongest sword.");
            }
            else if (speech.Contains("thinker"))
            {
                Say("To think is to live. While others toil away, I ponder the great questions of our age.");
            }
            else if (speech.Contains("questioning"))
            {
                // Reward logic
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Questioning is the path to enlightenment. If you ever find a golden apple, bring it to me, and I shall reward your curiosity.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("begone"))
            {
                Say("Before you leave, remember this: Ignorance can be cured with knowledge. Seek out the library of the ancients to better yourself.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Ah, you seek a reward for your efforts? Very well, but only if you can answer my riddle correctly.");
            }

            base.OnSpeech(e);
        }

        public SirNewton(Serial serial) : base(serial) { }

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
