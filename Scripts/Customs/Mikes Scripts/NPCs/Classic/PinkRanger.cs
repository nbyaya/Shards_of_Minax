using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Pink Ranger")]
    public class PinkRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PinkRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pink Ranger";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 110;
            Int = 65;
            Hits = 75;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 24 });
            AddItem(new StuddedChest() { Hue = 24 });
            AddItem(new ChainCoif() { Hue = 24 });
            AddItem(new StuddedGloves() { Hue = 24 });
            AddItem(new Boots() { Hue = 24 });
            AddItem(new Bow() { Name = "Pink Ranger's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am the Pink Ranger, a guardian of Compassion!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, as Compassion heals all wounds.");
            }
            else if (speech.Contains("job"))
            {
                Say("I protect the innocent and spread Compassion throughout the land.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the truest form of strength. Do you understand its power?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then you must strive to show kindness and empathy in all your actions.");
            }
            else if (speech.Contains("pink"))
            {
                Say("The color pink signifies love and compassion. It's more than just a hue; it's a representation of my mission.");
            }
            else if (speech.Contains("perfect"))
            {
                Say("It's not just about physical health, but also mental and spiritual well-being. Compassion plays a vital role in all three.");
            }
            else if (speech.Contains("protect"))
            {
                Say("Protection is more than just physical safety. It's about ensuring that the spirit of compassion remains alive in people's hearts.");
            }
            else if (speech.Contains("power"))
            {
                Say("True power doesn't come from strength or might, but from understanding and empathy. Have you ever felt this kind of power?");
            }
            else if (speech.Contains("felt"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Feelings are a guiding force. When you truly feel for others, you can understand their pain and joy. That's where true compassion starts. And for your journey in understanding this, let me give you a reward to help you on your way.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey has its challenges and rewards. But remember, it's not the destination but the lessons learned along the way that matter.");
            }

            base.OnSpeech(e);
        }

        public PinkRanger(Serial serial) : base(serial) { }

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
