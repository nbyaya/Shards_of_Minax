using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Queen Elizabeth I")]
    public class QueenElizabethI : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public QueenElizabethI() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Queen Elizabeth I";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new FancyDress() { Hue = 1152 }); // Fancy dress with hue 1152
            AddItem(new GoldNecklace());
            AddItem(new Boots() { Hue = 1175 }); // Boots with hue 1175
            AddItem(new Spellbook() { Name = "Queen Elizabeth's Journal" });
            
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
                Say("I am Queen Elizabeth I of England, the sovereign of a mighty kingdom!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is strong, for a queen must be resilient.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty as a monarch is to rule wisely and justly over my people.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("The virtue of justice is of utmost importance in my reign. Without it, chaos would prevail.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Do you believe in the power of justice, noble traveler?");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("My kingdom of England has a rich history and has seen many challenges. Yet, with the unity of our people, we always persevere.");
            }
            else if (speech.Contains("resilient"))
            {
                Say("Resilience is a quality not just for a queen, but for all of England. We face adversities with courage and determination.");
            }
            else if (speech.Contains("monarch"))
            {
                Say("As a monarch, I hold the responsibility of ensuring prosperity for my land and safety for my subjects. It's a task I do not take lightly.");
            }
            else if (speech.Contains("chaos"))
            {
                Say("Chaos is a force that threatens the harmony of a realm. I strive to maintain order and balance in all things.");
            }
            else if (speech.Contains("traveler"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Many travelers come to England, each bringing tales of distant lands and adventures. For your dedication to the pursuit of justice, I reward you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public QueenElizabethI(Serial serial) : base(serial) { }

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
