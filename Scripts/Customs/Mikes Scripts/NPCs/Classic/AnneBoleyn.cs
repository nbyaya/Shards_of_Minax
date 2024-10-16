using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Anne Boleyn")]
    public class AnneBoleyn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AnneBoleyn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Anne Boleyn";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyDress(1156)); // Clothing item with hue 1156
            AddItem(new GoldNecklace());
            AddItem(new Boots(1174)); // Boots with hue 1174
            AddItem(new Spellbook() { Name = "Anne's Diary" });
			
			Hue = Race.RandomSkinHue();
			HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, traveler. I am Anne Boleyn.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in fine health, thank you.");
            }
            else if (speech.Contains("job"))
            {
                Say("My daily duty is to tend to the gardens of this fine estate.");
            }
            else if (speech.Contains("virtues humility"))
            {
                Say("The virtue of humility is a noble one. We should all strive to be humble and kind.");
            }
            else if (speech.Contains("virtues ponder"))
            {
                Say("Do you ponder the virtues, traveler?");
            }
            else if (speech.Contains("anne"))
            {
                Say("Aye, once a queen of England, my fate was sealed at the Tower of London. It's a tale of romance, ambition, and betrayal.");
            }
            else if (speech.Contains("fine"))
            {
                Say("Thankfully, the serenity of these gardens has kept my spirit in a state of tranquility. I find nature to be healing.");
            }
            else if (speech.Contains("gardens"))
            {
                Say("These gardens are home to many rare and mystical herbs. Some even say there's an ancient herb with powerful healing abilities hidden here.");
            }
            else if (speech.Contains("humility"))
            {
                Say("True humility is understanding our place in the vastness of the universe. It teaches us to be compassionate to others.");
            }
            else if (speech.Contains("ponder"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new BeltSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("serenity"))
            {
                Say("Finding serenity amidst chaos is a virtue in itself. It's a sense of peace that eludes many, but can be found in nature's embrace.");
            }
            else if (speech.Contains("herbs"))
            {
                Say("These herbs carry the wisdom of ages. If you're keen to learn, seek the elder druid in the forest. He knows their secrets.");
            }

            base.OnSpeech(e);
        }

        public AnneBoleyn(Serial serial) : base(serial) { }

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
