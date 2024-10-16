using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Brewmaster Barlin")]
    public class BrewmasterBarlin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BrewmasterBarlin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Brewmaster Barlin";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 35;
            Int = 125;
            Hits = 75;

            // Appearance
            AddItem(new ShortPants() { Hue = 2126 });
            AddItem(new Surcoat() { Hue = 1446 });
            AddItem(new Sandals() { Hue = 1904 });

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
                Say("Welcome to my alchemy shop, traveler!");
            }
            else if (speech.Contains("health"))
            {
                Say("I brew potions that can heal wounds.");
            }
            else if (speech.Contains("job"))
            {
                Say("Alchemy is my profession.");
            }
            else if (speech.Contains("alchemy"))
            {
                Say("Alchemy requires patience and knowledge of the ingredients' properties. Do you have an interest in alchemy?");
            }
            else if (speech.Contains("yes") && speech.Contains("alchemy"))
            {
                Say("That's wonderful! Alchemy can reveal the hidden virtues of ingredients and create powerful elixirs.");
            }
            else if (speech.Contains("brewmaster"))
            {
                Say("Ah, you've heard of me? I've spent many years mastering the art of brewing potions in this region.");
            }
            else if (speech.Contains("heal"))
            {
                Say("Healing wounds is but one use of my potions. I also have elixirs that can boost strength and quicken reflexes.");
            }
            else if (speech.Contains("interest"))
            {
                Say("Many are intrigued by alchemy, but only a few truly dedicate themselves to its study. I sometimes offer apprenticeships to the worthy.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("Determining who is worthy is a challenge. However, if you bring me a rare ingredient, I might consider you for training.");
            }
            else if (speech.Contains("ingredient"))
            {
                Say("The ingredient I seek is the 'Mystic Moonflower'. It grows only under the full moon in the enchanted forest to the north.");
            }
            else if (speech.Contains("moonflower"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you've found it! As a token of my appreciation, here's something special for you. And if you're ever interested, my doors are open for apprenticeship.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BrewmasterBarlin(Serial serial) : base(serial) { }

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
