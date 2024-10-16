using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Joan of Arc")]
    public class LadyJoanOfArc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyJoanOfArc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Joan of Arc";
            Body = 0x191; // Human female body

            // Stats
            Str = 140;
            Dex = 40;
            Int = 40;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 38 });
            AddItem(new PlateChest() { Hue = 38 });
            AddItem(new PlateHelm() { Hue = 38 });
            AddItem(new PlateGloves() { Hue = 38 });
            AddItem(new PlateArms() { Hue = 38 });
            AddItem(new PlateGorget() { Hue = 38 });
            AddItem(new Boots() { Hue = 38 });
            AddItem(new Halberd() { Name = "Lady Joan of Arc's Halberd" });

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
                Say("Greetings, traveler. I am Lady Joan of Arc.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is quite robust, for I strive to maintain a body and spirit in harmony.");
            }
            else if (speech.Contains("job"))
            {
                Say("My noble calling is that of a protector. I defend the weak and uphold justice.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is a virtue of great importance. It is the strength of one's heart that shines in times of adversity.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Do you possess the strength of valor within you?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your response speaks volumes. Remember, true valor is not recklessness but tempered strength.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is not just a state of body, but of the soul. It's achieved when we align our actions with our beliefs. Do you strive for such a balance in your life?");
            }
            else if (speech.Contains("protector"))
            {
                Say("Being a protector is more than just wielding a sword. It's about standing up for what's right, even if you stand alone. Have you ever taken such a stand?");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is a beacon, guiding us through the murkiness of human conflicts. But justice is only as strong as those who uphold it. Have you ever sought to bring justice to a situation?");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity tests our spirit, revealing our true character. In facing challenges, we find out who we truly are. Have you faced any recent challenges?");
            }
            else if (speech.Contains("recklessness"))
            {
                Say("Recklessness is the blind charge forward, while valor is the calculated risk. Wisdom is knowing the difference. Do you believe wisdom is necessary for a warrior?");
            }
            else if (speech.Contains("wisdom"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Wisdom is the compass that guides us through the labyrinth of life. It is earned through experiences, both good and bad. As a token of my appreciation for seeking wisdom, please accept this gift.");
                    from.AddToBackpack(new PeacemakingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("life"))
            {
                Say("Life is the greatest journey we will ever embark upon. Every step, every decision molds our path. Tell me, traveler, what drives you forward in this journey?");
            }

            base.OnSpeech(e);
        }

        public LadyJoanOfArc(Serial serial) : base(serial) { }

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
