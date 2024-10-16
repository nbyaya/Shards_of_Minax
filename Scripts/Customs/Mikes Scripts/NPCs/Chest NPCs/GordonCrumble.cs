using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class GordonCrumble : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GordonCrumble() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gordon Crumble";
            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();
            Title = "the Baker";

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomPinkHue() });
            AddItem(new LongPants() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Shoes() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Cap() { Hue = Utility.RandomBrightHue() });
            AddItem(new FullApron() { Hue = Utility.RandomBrightHue() });
            
            // Stats
            Str = 100;
            Dex = 70;
            Int = 90;
            Hits = 80;

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
                Say("Hello, I am Gordon Crumble, the master baker.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to create the finest pastries and breads. I delight in baking.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thanks to the joy of baking.");
            }
            else if (speech.Contains("baking"))
            {
                Say("Baking is both an art and a science. Each ingredient must be measured with precision.");
            }
            else if (speech.Contains("pastry"))
            {
                Say("Pastries are my specialty. A well-made pastry can bring joy to anyone.");
            }
            else if (speech.Contains("delight"))
            {
                Say("Ah, 'Baker's Delight' is not just a chest; it's a treasure trove of baking wonders.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait a little longer before you can receive another reward.");
                }
                else
                {
                    Say("For your inquisitiveness and patience, here is the 'Baker's Delight' chest as your reward!");
                    from.AddToBackpack(new BakersDolightChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public GordonCrumble(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
