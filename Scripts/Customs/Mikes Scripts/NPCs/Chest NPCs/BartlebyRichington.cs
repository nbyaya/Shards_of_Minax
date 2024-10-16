using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bartleby Richington")]
    public class BartlebyRichington : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BartlebyRichington() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bartleby Richington";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new Shoes() { Hue = 1153 });
            AddItem(new Cloak() { Hue = 1153 });
            AddItem(new Gold() { Amount = 1000 });

            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Short hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203D, 0x2040); // Short beards

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

            // Basic Responses
            if (speech.Contains("name"))
            {
                Say("Ah, greetings! I am Bartleby Richington, merchant extraordinaire! If you seek to know more, just ask.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as my collection of treasures, thank you for asking! But tell me, what else intrigues you?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to amass and trade the finest goods across the lands. A merchant's life is never dull! Would you like to know more about my wares?");
            }

            // Dialogues unlocked by basic responses
            else if (speech.Contains("wares") && speech.Contains("job"))
            {
                Say("I have an array of goods, from rare artifacts to valuable treasures. Perhaps you are interested in the lore behind them?");
            }
            else if (speech.Contains("lore") && speech.Contains("wares"))
            {
                Say("Each item has a tale. For example, the 'Jewel of Prosperity' is said to bring fortune to its bearer. Curious about any specific item?");
            }

            // Dialogues unlocked by lore-related responses
            else if (speech.Contains("jewel") && speech.Contains("lore"))
            {
                Say("Ah, the 'Jewel of Prosperity'! Legend says it was crafted by a famous jeweler who could turn even the direst of fortunes around.");
            }
            else if (speech.Contains("turn") && speech.Contains("jewel"))
            {
                Say("Indeed, it is said that the jewel has the power to influence luck and prosperity. Do you believe in such things?");
            }
            else if (speech.Contains("believe") && speech.Contains("turn"))
            {
                Say("Belief in such artifacts can often lead to unexpected changes. Some say faith in these treasures brings them to life.");
            }

            // Dialogues unlocked by belief-related responses
            else if (speech.Contains("faith") && speech.Contains("believe"))
            {
                Say("Faith in a treasure can sometimes be more powerful than the treasure itself. Have you ever experienced such a phenomenon?");
            }
            else if (speech.Contains("experience") && speech.Contains("faith"))
            {
                Say("Many have reported strange coincidences and fortune shifts. It seems belief and the power of a good charm go hand in hand.");
            }

            // Dialogues unlocked by experience-related responses
            else if (speech.Contains("charm") && speech.Contains("experience"))
            {
                Say("A charm, such as the 'Jewel of Prosperity,' is not just a trinket but a symbol of potential fortune. Do you wish to explore more about such charms?");
            }
            else if (speech.Contains("explore") && speech.Contains("charm"))
            {
                Say("Exploring these charms can lead to fascinating discoveries. And speaking of which, have you heard of the 'Merchant's Fortune' chest?");
            }

            // Dialogues unlocked by exploration-related responses
            else if (speech.Contains("chest") && speech.Contains("explore"))
            {
                Say("The 'Merchant's Fortune' chest is a collection of many rare items. It is a reward only for those who engage in true commerce and trade.");
            }
            else if (speech.Contains("reward") && speech.Contains("chest"))
            {
                Say("A reward indeed! But before you receive it, tell me, do you know the secret of fortune?");
            }

            // Dialogues unlocked by secret-related responses
            else if (speech.Contains("secret") && speech.Contains("reward"))
            {
                Say("The secret of fortune is not just in wealth, but in understanding and using it wisely. Are you ready to prove your worthiness?");
            }
            else if (speech.Contains("prove") && speech.Contains("secret"))
            {
                Say("Proving your worth involves engaging deeply with the merchant's craft. Your questions so far show promise. Now, a final test: can you answer why fortune favors the bold?");
            }

            // Final test before reward
            else if (speech.Contains("bold") && speech.Contains("fortune"))
            {
                Say("Indeed, fortune often favors those who take bold steps. Your journey has been commendable. Here is a special reward for your curiosity and engagement!");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new MerchantFortuneChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am intrigued by your queries. Continue asking, and perhaps more secrets will be revealed.");
            }

            base.OnSpeech(e);
        }

        public BartlebyRichington(Serial serial) : base(serial) { }

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
