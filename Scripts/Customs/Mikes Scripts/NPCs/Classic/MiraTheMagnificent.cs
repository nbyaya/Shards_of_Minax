using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mira the Magnificent")]
    public class MiraTheMagnificent : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MiraTheMagnificent() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mira the Magnificent";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 70;
            Int = 70;
            Hits = 65;

            // Appearance
            AddItem(new FancyDress() { Hue = 2126 });
            AddItem(new Sandals() { Hue = 2126 });
            AddItem(new GoldNecklace() { Name = "Mira's Necklace" });
            AddItem(new GoldBracelet() { Name = "Mira's Bracelet" });

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
                Say("I am Mira the Magnificent, darling.");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care, darling? My health is none of your business.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job,' as you put it, is to make the world a more charming place. Isn't that magnificent?");
            }
            else if (speech.Contains("battles"))
            {
                Say("Oh, you're interested in my 'battles'? How amusing! Do you even know what true battles are, darling?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, darling, you're precious. If you think you're valiant, you've already lost the battle.");
            }
            else if (speech.Contains("magnificent"))
            {
                Say("Of course, darling, I earned that title after facing many challenges and winning the hearts of countless admirers. It's not easy being this magnificent.");
            }
            else if (speech.Contains("business"))
            {
                Say("While my health is my own business, I'll have you know I've faced many trials that would've broken the average person. But I'm no ordinary woman.");
            }
            else if (speech.Contains("charming"))
            {
                Say("Yes, I bring charm and elegance wherever I go. You'd be surprised how many doors a dash of charm can open.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Ah, the challenges I've faced! From treacherous terrains to cunning adversaries, every victory has only added to my magnificent reputation.");
            }
            else if (speech.Contains("trials"))
            {
                Say("My trials were not just physical, darling, but mental and emotional too. And through them all, I emerged stronger and more resolute.");
            }
            else if (speech.Contains("elegance"))
            {
                Say("True elegance is not just in appearance, but in character and actions. And if you prove yourself, perhaps I might reward you with a token of my appreciation.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("I admire those who show genuine interest. Here, take this as a sign of my favor. Use it wisely, darling.");
                    from.AddToBackpack(new BagOfJuice()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MiraTheMagnificent(Serial serial) : base(serial) { }

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
