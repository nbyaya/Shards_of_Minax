using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ravishing Rosa")]
    public class RavishingRosa : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RavishingRosa() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ravishing Rosa";
            Body = 0x191; // Human female body

            // Stats
            Str = 89;
            Dex = 73;
            Int = 52;
            Hits = 64;

            // Appearance
            AddItem(new LongPants() { Hue = 2967 });
            AddItem(new BodySash() { Hue = 2968 });
            AddItem(new Sandals() { Hue = 2969 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, kind traveler. I am Ravishing Rosa, a courtesan of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Ah, it's as robust as a rose in bloom.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession, you ask? I offer companionship and dance, weaving stories with the grace of my steps.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Life's virtues, they say, are like a delicate dance. Honesty, Compassion, Valor... which do you hold most dear?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Ah, a soul that values honesty. In my line of work, trust is the foundation of all things.");
            }
            else if (speech.Contains("ravishing"))
            {
                Say("Ah, you've heard of me, have you? There's a story behind every name, and mine has a tale of passion and mystery.");
            }
            else if (speech.Contains("rose"))
            {
                Say("The rose is a symbol of love and beauty, but it also has thorns. Life has taught me to appreciate the bloom while being wary of the thorns.");
            }
            else if (speech.Contains("dance"))
            {
                Say("Ah, dance! It's a language without words. Every move, every gesture tells a story. In fact, I learned a rare dance from an old seer. Would you like to see?");
            }
            else if (speech.Contains("compassion"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Compassion is a virtue that allows us to connect with others, to feel their pain and joy. It's the essence of my work. In gratitude for recognizing it, please take this token of my appreciation.");
                    from.AddToBackpack(new FireResistAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is hard to earn and easy to break. In my travels, I've encountered those who betrayed trust and others who upheld it at great cost. It's a delicate balance, isn't it?");
            }
            else if (speech.Contains("story"))
            {
                Say("Every story has its ups and downs. Mine began in a small village, overshadowed by a looming castle. That castle held many secrets, one of which changed my life forever.");
            }
            else if (speech.Contains("thorns"))
            {
                Say("Thorns remind us that even in beauty, there can be pain. I've had my share of encounters with thorny situations. One such encounter was with a bandit leader named Red Raven.");
            }
            else if (speech.Contains("seer"))
            {
                Say("The seer I met was an old woman with eyes that held the mysteries of the universe. She spoke of prophecies and visions, one of which involved a hero rising from the ashes.");
            }

            base.OnSpeech(e);
        }

        public RavishingRosa(Serial serial) : base(serial) { }

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
