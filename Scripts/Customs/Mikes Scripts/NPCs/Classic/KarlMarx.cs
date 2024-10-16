using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Karl Marx")]
    public class KarlMarx : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KarlMarx() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Karl Marx";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new LongPants() { Hue = 1154 });
            AddItem(new FancyShirt() { Hue = 1109 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Spellbook() { Name = "Das Kapital" });

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
                Say("I am Karl Marx, a philosopher and economist.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is not relevant; what matters is the health of society.");
            }
            else if (speech.Contains("job"))
            {
                Say("My mission is to promote the ideals of communism.");
            }
            else if (speech.Contains("communism") || speech.Contains("equality"))
            {
                Say("Communism values the common good above all else. Do you believe in equality?");
            }
            else if (speech.Contains("yes") || speech.Contains("character") || speech.Contains("greater good"))
            {
                Say("Your answer reveals much about your character. Strive for the greater good, my friend.");
            }
            else if (speech.Contains("philosopher"))
            {
                Say("Philosophy is the pursuit of wisdom. It's a means of understanding the world around us, and our place in it.");
            }
            else if (speech.Contains("society"))
            {
                Say("Society is a complex web of relationships and structures. The true health of a society is measured by the well-being of its most vulnerable members.");
            }
            else if (speech.Contains("ideals"))
            {
                Say("The ideals I stand for aim at a classless society where resources and production are commonly owned. Do you understand the importance of shared ownership?");
            }
            else if (speech.Contains("common"))
            {
                Say("The common good emphasizes the community and its social health over individual gain. It is a principle that calls for shared sacrifices and benefits.");
            }
            else if (speech.Contains("character"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Character is the manifestation of one's values and beliefs. It is the compass that guides us through moral dilemmas. Here, take this token as a testament to our meaningful conversation.");
                    from.AddToBackpack(new CarpentryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("good"))
            {
                Say("Working for the greater good means sometimes setting aside personal desires for the benefit of all. It's a noble path, but not an easy one.");
            }

            base.OnSpeech(e);
        }

        public KarlMarx(Serial serial) : base(serial) { }

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
