using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lily Verdant")]
    public class LilyVerdant : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilyVerdant() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lily Verdant";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Cap() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherChest() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomGreenHue() });
            AddItem(new Sandals() { Hue = Utility.RandomNeutralHue() });

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
                Say("Hello there! I am Lily Verdant, keeper of these lush gardens.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am thriving amidst the beauty of these gardens. Nature keeps me well.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to nurture these plants and ensure our garden flourishes.");
            }
            else if (speech.Contains("garden"))
            {
                Say("The garden holds many secrets. Perhaps you'd like to know more about its treasures?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, you seek treasures! In these gardens, nature provides us with wonders, but only those who truly understand its whispers will be rewarded.");
            }
            else if (speech.Contains("understand"))
            {
                Say("Understanding nature requires patience and a keen eye. If you are sincere, I may have a special reward for you.");
            }
            else if (speech.Contains("sincere"))
            {
                Say("Sincerity is a rare gem in today's world. Your willingness to understand is commendable. Tell me, do you appreciate the delicate balance of nature?");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is crucial for the harmony of all things. In the garden, every plant, every creature plays a role. Do you seek harmony in your own life?");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Finding harmony is a personal journey. The garden teaches us about patience and the flow of life. Have you learned from these teachings?");
            }
            else if (speech.Contains("teachings"))
            {
                Say("The teachings of the garden are many. It speaks through the rustling leaves and blooming flowers. Have you felt its whispers?");
            }
            else if (speech.Contains("whispers"))
            {
                Say("The whispers of the garden guide those who listen. They speak of hidden truths and ancient secrets. Do you seek these secrets?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are often guarded closely. To uncover them, one must be persistent and thoughtful. Are you prepared for such a journey?");
            }
            else if (speech.Contains("journey"))
            {
                Say("A journey in the garden is both physical and spiritual. If you have proven your dedication, a reward awaits you. But first, tell me, do you value the small things?");
            }
            else if (speech.Contains("small things"))
            {
                Say("The small things often hold great significance. In the garden, the tiniest bud can become a magnificent bloom. Have you noticed these details?");
            }
            else if (speech.Contains("details"))
            {
                Say("Details make the difference between a thriving garden and a barren one. Pay close attention, and you shall find what you seek. If you have been attentive, you may now be worthy of a reward.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Please return later.");
                }
                else
                {
                    Say("Your dedication and attentiveness are admirable. For your efforts, accept this special chest from the heart of the garden.");
                    from.AddToBackpack(new GardenersParadiseChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("appreciate"))
            {
                Say("Appreciation for nature’s beauty is a mark of wisdom. The garden reveals its treasures to those who truly cherish it. Have you found joy in its splendor?");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy comes from within and is reflected in our surroundings. The garden flourishes as we do. If you have found joy, then you have understood much.");
            }
            else if (speech.Contains("flourishes"))
            {
                Say("Just as the garden flourishes with care, so does our spirit. If you have truly cared for the garden's lessons, you may be deserving of a reward.");
            }
            
            base.OnSpeech(e);
        }

        public LilyVerdant(Serial serial) : base(serial) { }

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
