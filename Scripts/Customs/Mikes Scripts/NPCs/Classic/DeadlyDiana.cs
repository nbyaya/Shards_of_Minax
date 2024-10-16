using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Deadly Diana")]
    public class DeadlyDiana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DeadlyDiana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Deadly Diana";
            Body = 0x191; // Human female body

            // Stats
            Str = 150;
            Dex = 65;
            Int = 25;
            Hits = 110;

            // Appearance
            AddItem(new LeatherShorts() { Hue = 1175 });
            AddItem(new Skirt() { Hue = 1109 });
            AddItem(new ThighBoots() { Hue = 1175 });
            AddItem(new LeatherGloves() { Name = "Diana's Killing Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Deadly Diana, a shadow in the night.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern to thee.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is not for the faint-hearted. I bring death.");
            }
            else if (speech.Contains("justice"))
            {
                Say("The virtue of justice is but a facade in my world. Do you dare to seek justice?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your quest for justice is futile. In my world, justice is defined by the heart of the beholder.");
            }
            else if (speech.Contains("night"))
            {
                Say("The night is when I'm most alive, it's when secrets are whispered and shadows take form.");
            }
            else if (speech.Contains("concern"))
            {
                Say("Many have shown concern, and most met their end trying to pry too deep.");
            }
            else if (speech.Contains("death"))
            {
                Say("Death, to me, is a mercy I bestow upon those who are deemed deserving. Not all receive it.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are the currency of my world. For the right price, I may share one with you.");
            }
            else if (speech.Contains("end"))
            {
                Say("Every end is a new beginning. But remember, not all beginnings lead to good paths.");
            }
            else if (speech.Contains("mercy"))
            {
                Say("Once, I showed mercy to a beggar. In gratitude, he gave me a trinket. If you prove worthy, it could be yours.");
            }
            else if (speech.Contains("price"))
            {
                Say("Everything has a price. If you wish to know a secret, offer something of value.");
            }
            else if (speech.Contains("beginning"))
            {
                Say("My beginning was in the dark alleys of the city. That's where my legend was born.");
            }
            else if (speech.Contains("trinket"))
            {
                Say("The trinket is a token from another era, a sign of a promise once made. Prove your worth, and it's yours.");
            }
            else if (speech.Contains("offer"))
            {
                Say("Most offers are empty gestures, but I sense something genuine in you. What do you propose?");
            }
            else if (speech.Contains("alleys"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The alleys are where secrets and shadows blend. Be wary if you tread there. Take this.");
                    from.AddToBackpack(new AnimalLoreAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DeadlyDiana(Serial serial) : base(serial) { }

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
