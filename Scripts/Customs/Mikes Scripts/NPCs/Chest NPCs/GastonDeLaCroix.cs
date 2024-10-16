using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gaston de la Croix")]
    public class GastonDeLaCroix : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GastonDeLaCroix() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gaston de la Croix";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1057 });
            AddItem(new PlateLegs() { Hue = 1457 });
            AddItem(new PlateArms() { Hue = 1757 });
            AddItem(new PlateGloves() { Hue = 1857 });
            AddItem(new PlateHelm() { Hue = 2757 });
            AddItem(new GoldRing() { Name = "Royal Ring of France" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x2040); // Random hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2043, 0x2045); // Random facial hair styles

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
                Say("Greetings, I am Gaston de la Croix, the keeper of treasures. My role is to safeguard the treasures of the French monarchy.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you for asking. The treasures I guard keep me in good spirits.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect and maintain the vast wealth entrusted to me. Many seek these treasures, but few are worthy.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Indeed, I guard many treasures. They are hidden well and only revealed to those who prove their worth.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must demonstrate your knowledge of the great figures of French history. Tell me, what do you know of Louis?");
            }
            else if (speech.Contains("louis"))
            {
                Say("Louis is a name of great importance in French history. His legacy is both grand and mysterious. What can you tell me about his legacy?");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The legacy of Louis is one of power and grandeur. But there are other figures who played crucial roles. What do you know of Marie Antoinette?");
            }
            else if (speech.Contains("marie"))
            {
                Say("Marie Antoinette, a queen whose life was as tumultuous as it was illustrious. Her story is intertwined with the fate of France. Can you tell me more about her influence?");
            }
            else if (speech.Contains("influence"))
            {
                Say("Her influence extended far beyond her court. Her story is a part of the revolution. But enough about the past, what about the art of French warfare? What can you tell me about Napoleon?");
            }
            else if (speech.Contains("napoleon"))
            {
                Say("Napoleon, a general whose name is synonymous with French military prowess. His impact on France was immense. Have you heard of his major battles?");
            }
            else if (speech.Contains("battles"))
            {
                Say("The major battles of Napoleon's campaigns are legendary. But among them, one stands out. Do you know of his greatest victory?");
            }
            else if (speech.Contains("victory"))
            {
                Say("His greatest victory is often considered to be the Battle of Austerlitz. But victories are not just on the battlefield. How about the arts? What do you know of French art and culture?");
            }
            else if (speech.Contains("art"))
            {
                Say("French art is renowned worldwide. From the Renaissance to modern times, it has been influential. But what of the revolution? How did it impact French society?");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The French Revolution reshaped the country in profound ways. It ended the monarchy and set the stage for modern France. Now, considering all this history, what have you learned?");
            }
            else if (speech.Contains("learned"))
            {
                Say("If you have absorbed the history and understood its significance, you may be worthy of a reward. For your knowledge, accept this treasure chest as a token of appreciation.");
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new LouisTreasuryChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("I have no reward for you right now. Please return later.");
                }
            }
            else
            {
                Say("I'm afraid I cannot help with that query. Try speaking about names, history, or notable figures.");
            }

            base.OnSpeech(e);
        }

        public GastonDeLaCroix(Serial serial) : base(serial) { }

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
