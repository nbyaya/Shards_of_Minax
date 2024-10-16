using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Oldman Omric")]
    public class OldmanOmric : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OldmanOmric() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Oldman Omric";
            Body = 0x190; // Human male body

            // Stats
            Str = 78;
            Dex = 45;
            Int = 115;
            Hits = 68;

            // Appearance
            AddItem(new LongPants() { Hue = 38 });
            AddItem(new Tunic() { Hue = 1447 });
            AddItem(new Shoes() { Hue = 2126 });

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
                Say("Greetings, traveler. I am Oldman Omric, the alchemist.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, for an old man.");
            }
            else if (speech.Contains("job"))
            {
                Say("My life's work is the pursuit of alchemical knowledge.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Alchemy teaches us the virtue of Sacrifice. To transform one substance into another often requires sacrifice.");
            }
            else if (speech.Contains("alchemy"))
            {
                Say("Do you seek knowledge of the alchemical arts?");
            }
            else if (speech.Contains("omric"))
            {
                Say("Ah, you've heard of me? I've been in these lands for many years. I've seen kingdoms rise and fall, all while perfecting my craft.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, the secret to my longevity lies in the elixirs I brew. They're not just for show, after all!");
            }
            else if (speech.Contains("alchemical"))
            {
                Say("Throughout my years, I have collected rare and mystical ingredients from all corners of the realm. These ingredients are essential for advanced alchemical practices.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("True Sacrifice goes beyond alchemy. It's the essence of selflessness, giving without expecting return. In my youth, I once sacrificed something dear for the greater good.");
            }
            else if (speech.Contains("arts"))
            {
                Say("The arts are not simply mixing potions. It's understanding the world around us, how every element interacts, and harnessing that knowledge. If you are truly interested, I may have a special task for you. Complete it, and you shall be rewarded.");
            }
            else if (speech.Contains("kingdoms"))
            {
                Say("There were many great rulers and wars that shaped our history. But through it all, alchemy remained, adapting and evolving with time.");
            }
            else if (speech.Contains("elixirs"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Would you like a sample? This particular elixir can restore vitality and mend minor wounds. Consider it a gift from an old man.");
                    from.AddToBackpack(new ManaDrainAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("ingredients"))
            {
                Say("Some of these are dangerous in the wrong hands. From mandrake roots to phoenix feathers, each has its unique properties.");
            }
            else if (speech.Contains("youth"))
            {
                Say("Ah, those were the days! Adventures, close encounters, and the thrill of discovery. I once traveled with a band of heroes seeking the lost city.");
            }
            else if (speech.Contains("task"))
            {
                Say("I need a rare herb, known as the Moonlit Fern, which blooms only under a full moon. Retrieve it for me, and I shall grant you an unspecified reward.");
            }

            base.OnSpeech(e);
        }

        public OldmanOmric(Serial serial) : base(serial) { }

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
