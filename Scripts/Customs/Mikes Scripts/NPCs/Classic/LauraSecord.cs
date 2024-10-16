using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Laura Secord")]
    public class LauraSecord : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LauraSecord() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Laura Secord";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 100;
            Int = 85;
            Hits = 65;

            // Appearance
            AddItem(new PlainDress() { Hue = 1125 });
            AddItem(new Boots() { Hue = 1103 });
            AddItem(new Bonnet() { Hue = 1132 });

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
                Say("I am Laura Secord, hailing from the beautiful lands of France!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, merci!");
            }
            else if (speech.Contains("job"))
            {
                Say("I find solace in the art of baking delicious pastries!");
            }
            else if (speech.Contains("baking"))
            {
                Say("True compassion is found in the warmth of a freshly baked croissant, don't you think?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Would you like to try one of my delightful pastries?");
            }
            else if (speech.Contains("france"))
            {
                Say("France is a land of romance, art, and of course, scrumptious cuisine! I miss the lavender fields and the gentle hum of the Seine river.");
            }
            else if (speech.Contains("merci"))
            {
                Say("Ah, you recognize the tongue of my homeland! French is such a poetic language. It always brings warmth to my heart.");
            }
            else if (speech.Contains("pastries"))
            {
                Say("My pastries are a blend of traditional French techniques and local flavors. Have you ever tried a macaron with exotic fruits?");
            }
            else if (speech.Contains("romance"))
            {
                Say("There's nothing more romantic than sharing a delicate pastry under the Eiffel Tower. Such memories always make me smile.");
            }
            else if (speech.Contains("cuisine"))
            {
                Say("French cuisine is not just about taste; it's an experience. Every dish tells a story. For instance, the quiche has roots in medieval Germany but found its fame in France.");
            }
            else if (speech.Contains("french"))
            {
                Say("Speaking in French always reminds me of my childhood in the quaint town of Provence. The aromas, the bustling marketplaces... such fond memories!");
            }
            else if (speech.Contains("macaron"))
            {
                Say("Ah, the macaron! A delightful treat that is both crispy and creamy. Making them is an art. If you help me gather some ingredients, I might just reward you with a batch of your own.");
            }
            else if (speech.Contains("ingredients"))
            {
                Say("For a perfect batch of macarons, I'll need almond flour, egg whites, and some unique fruit extracts. Once you bring them to me, a special reward awaits you.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Your efforts won't go unnoticed. Please return later.");
                }
                else
                {
                    Say("Your efforts won't go unnoticed. For your help, you will receive a box of my finest pastries. A treat to delight your senses! Here is a sample for you!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LauraSecord(Serial serial) : base(serial) { }

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
