using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Feral Fred")]
    public class FeralFred : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FeralFred() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Feral Fred";
            Body = 0x190; // Human male body

            // Stats
            Str = 95;
            Dex = 60;
            Int = 70;
            Hits = 95;

            // Appearance
            AddItem(new LongPants() { Hue = 2967 });
            AddItem(new PlainDress() { Hue = 2965 });
            AddItem(new ThighBoots() { Hue = 1154 });
            AddItem(new ShepherdsCrook() { Name = "Fred's Taming Stick" });

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
                Say("I am Feral Fred, the animal tamer!");
            }
            else if (speech.Contains("health"))
            {
                Say("I have a few scratches, but I'm alright.");
            }
            else if (speech.Contains("job"))
            {
                Say("I tame and care for animals.");
            }
            else if (speech.Contains("animals"))
            {
                Say("Animals teach us compassion and loyalty. Do you understand?");
            }
            else if (speech.Contains("yes") && speech.Contains("animals"))
            {
                Say("Indeed, animals show us the way of compassion. Do you have a favorite animal?");
            }
            else if (speech.Contains("scratches"))
            {
                Say("Oh, these? Got them while taming a particularly wild griffin. Have you ever seen a griffin?");
            }
            else if (speech.Contains("care"))
            {
                Say("Caring for animals requires patience, love, and sometimes a little bravery. There are rare beasts I've encountered that need special attention. Ever heard of the Silver Lynx?");
            }
            else if (speech.Contains("loyalty"))
            {
                Say("Yes, loyalty is an admirable trait found in many animals. The Dire Wolf, for instance, will stay by its master's side even in the face of danger.");
            }
            else if (speech.Contains("griffin"))
            {
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    Say("Ah, the majestic griffin! A creature of the skies and the land. I once tamed one, and in gratitude, it gifted me a rare feather. Would you like it as a token of our conversation?");
                    from.AddToBackpack(new VeterinaryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("You must wait a while before receiving another reward.");
                }
            }
            else if (speech.Contains("lynx"))
            {
                Say("The Silver Lynx is a mystical creature, its fur shimmering under the moonlight. They say its purr can heal wounds. I've been fortunate to have seen one up close.");
            }
            else if (speech.Contains("wolf"))
            {
                Say("Dire Wolves are known for their fierce loyalty and unmatched strength. They make for great allies in battle but require understanding and respect.");
            }
            else if (speech.Contains("feral"))
            {
                Say("Ah, you've heard of me? I've been taming creatures in this land for many years. Most of the animals you see around were once my companions.");
            }

            base.OnSpeech(e);
        }

        public FeralFred(Serial serial) : base(serial) { }

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
