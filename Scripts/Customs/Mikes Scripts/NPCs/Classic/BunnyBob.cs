using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bunny Bob")]
    public class BunnyBob : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BunnyBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bunny Bob";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 120;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new FloppyHat() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new ShepherdsCrook() { Name = "Bunny Bob's Crook" });

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
                Say("Greetings, traveler! I am Bunny Bob, the animal tamer!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in excellent health, thanks for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to tame and care for animals. I have a special bond with these creatures.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("The virtue of compassion guides my interactions with animals. It's about understanding and caring for their needs.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Can you name any of the eight virtues that guide our lives in Britannia?");
            }
            else if (speech.Contains("honesty") || speech.Contains("compassion") || speech.Contains("valor") || speech.Contains("justice") || speech.Contains("sacrifice") || speech.Contains("honor") || speech.Contains("spirituality") || speech.Contains("humility"))
            {
                Say("Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility - these virtues shape our lives in profound ways.");
            }
            else if (speech.Contains("bunny"))
            {
                Say("Ah, the name Bunny Bob has been passed down in my family for generations. Funny story how it came about, would you like to hear it?");
            }
            else if (speech.Contains("excellent"))
            {
                Say("Yes, being around animals keeps me active and happy. Ever tried any of the herbal remedies from the forest?");
            }
            else if (speech.Contains("animals"))
            {
                Say("Over the years, I've tamed a variety of animals, from the humble rabbit to the majestic griffin. Have you ever encountered a rare creature in your travels?");
            }
            else if (speech.Contains("story"))
            {
                Say("Well, many years ago, my great-great-grandfather found a rabbit with a twisted foot. Instead of hunting it, he cared for it. The village kids started calling him Bunny Bob, and the name stuck for future generations!");
            }
            else if (speech.Contains("remedies"))
            {
                Say("The forest around here is rich with herbs and plants that have healing properties. If you bring me some blueleaf herbs, I might reward you with something special.");
            }
            else if (speech.Contains("creature"))
            {
                Say("There are many rare creatures in the land of Britannia. I once had the chance to tame a phoenix! Do you have any tales of encounters with legendary beasts?");
            }
            else if (speech.Contains("blueleaf"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, blueleaf! It's a remarkable herb with azure leaves. Not only can it heal, but it can also rejuvenate the spirit. Here's a reward for your efforts in bringing it to me.");
                    from.AddToBackpack(new ImbuingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("phoenix"))
            {
                Say("The phoenix is a symbol of rebirth and renewal. Taming one requires patience and understanding. Their feathers are said to have magical properties. Ever seen a phoenix feather?");
            }

            base.OnSpeech(e);
        }

        public BunnyBob(Serial serial) : base(serial) { }

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
