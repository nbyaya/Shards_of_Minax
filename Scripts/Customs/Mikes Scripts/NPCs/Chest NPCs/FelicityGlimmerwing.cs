using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class FelicityGlimmerwing : BaseCreature
    {
        private bool m_RewardGiven;
        private DateTime lastRewardTime;

        [Constructable]
        public FelicityGlimmerwing() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Felicity Glimmerwing";
            Title = "the Enchanted Faerie";
            Body = 0x191; // Female body
            Hue = Utility.RandomSkinHue();

            // Appearance
            AddItem(new Robe(Utility.RandomPinkHue()));
            AddItem(new Sandals());
            AddItem(new FeatheredHat(Utility.RandomBrightHue()));

            // Stats
            Str = 70;
            Dex = 80;
            Int = 90;
            Hits = 60;

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    Say("Greetings, traveler! I am Felicity Glimmerwing, the Enchanted Faerie of this forest.");
                }
                else if (speech.Contains("job"))
                {
                    Say("I dance among the stars and weave enchantments to protect the magical realms.");
                }
                else if (speech.Contains("health"))
                {
                    Say("I am full of sparkling energy, thanks to the magic of the forest.");
                }
                else if (speech.Contains("fairy dust"))
                {
                    Say("Ah, fairy dust! It’s the essence of magic and wonder. It’s what makes our world shine!");
                }
                else if (speech.Contains("enchantment"))
                {
                    Say("Enchantments are woven into the very fabric of our world. They bring joy and mystery to those who seek it.");
                }
                else if (speech.Contains("magic"))
                {
                    Say("Magic is the heart of our world, bringing life to every corner of the realm.");
                }
                else if (speech.Contains("realm"))
                {
                    Say("The realm is filled with wonders beyond imagination. But not all is as it seems.");
                }
                else if (speech.Contains("wonders"))
                {
                    Say("Yes, wonders are hidden in plain sight. Look closely and you might discover something extraordinary.");
                }
                else if (speech.Contains("hidden"))
                {
                    Say("Hidden things often require a keen eye and a curious heart to be revealed.");
                }
                else if (speech.Contains("revealed"))
                {
                    Say("When something is revealed, it often leads to new adventures and secrets.");
                }
                else if (speech.Contains("adventures"))
                {
                    Say("Adventures are the essence of life. They challenge us and make our journeys worthwhile.");
                }
                else if (speech.Contains("secrets"))
                {
                    Say("Secrets are like hidden treasures, waiting to be uncovered by those who are worthy.");
                }
                else if (speech.Contains("treasures"))
                {
                    Say("Ah, treasures! They come in many forms. Some are material, while others are of the heart and mind.");
                }
                else if (speech.Contains("heart"))
                {
                    Say("The heart is the source of our true strength. It guides us through trials and triumphs.");
                }
                else if (speech.Contains("strength"))
                {
                    Say("Strength comes not only from physical power but from courage and wisdom.");
                }
                else if (speech.Contains("courage"))
                {
                    Say("Courage is facing fears and challenges head-on, despite the odds.");
                }
                else if (speech.Contains("wisdom"))
                {
                    Say("Wisdom is gained through experience and reflection, often illuminating the path forward.");
                }
                else if (speech.Contains("path"))
                {
                    Say("The path we walk is shaped by our choices and experiences, leading us to our destiny.");
                }
                else if (speech.Contains("destiny"))
                {
                    Say("Destiny is the culmination of our actions and decisions. It is shaped by both chance and choice.");
                }
                else if (speech.Contains("chance"))
                {
                    Say("Chance can lead to unexpected opportunities, but it is up to us to seize them.");
                }
                else if (speech.Contains("opportunities"))
                {
                    Say("Opportunities are gifts that come our way, offering us the chance to grow and succeed.");
                }
                else if (speech.Contains("success"))
                {
                    Say("Success is the result of perseverance and effort. It is achieved through dedication.");
                }
                else if (speech.Contains("dedication"))
                {
                    Say("Dedication to a goal ensures that we remain focused and driven, even in the face of adversity.");
                }
                else if (speech.Contains("adversity"))
                {
                    Say("Adversity is a test of our resolve. It challenges us but also strengthens us.");
                }
                else if (speech.Contains("test"))
                {
                    Say("Tests are opportunities for growth. They reveal our strengths and areas for improvement.");
                }
                else if (speech.Contains("reveal"))
                {
                    Say("To reveal something, one must often look beyond the surface and explore deeper truths.");
                }
                else if (speech.Contains("truths"))
                {
                    Say("Truths are often hidden beneath layers of complexity. Seek them with an open mind.");
                }
                else if (speech.Contains("mind"))
                {
                    Say("An open mind allows us to explore new ideas and perspectives, enriching our understanding.");
                }
                else if (speech.Contains("understanding"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("You must prove your worthiness to receive the magical reward.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                return false;
            }
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                FairyDustChest chest = new FairyDustChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven your worthiness! Here is the Fairy Dust Chest, filled with magical treasures.");
                m_RewardGiven = true;
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
        }

        public FelicityGlimmerwing(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
