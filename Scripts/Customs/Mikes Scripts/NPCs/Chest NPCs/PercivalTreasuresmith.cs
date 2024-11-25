using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Percival Treasuresmith")]
    public class PercivalTreasuresmith : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PercivalTreasuresmith() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Percival Treasuresmith";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });
            
            // Set hair and facial hair
            HairItemID = 0x203B; // Long hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2042; // Beard
            FacialHairHue = Utility.RandomHairHue();

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
                Say("Ah, greetings! I am Percival Treasuresmith, the keeper of ancient secrets and hidden treasures. What brings you to seek knowledge?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like buried treasures; they require effort and understanding to uncover. Tell me, what do you know of treasures?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures are not merely material wealth, but also wisdom and experience. How do you seek to find them?");
            }
            else if (speech.Contains("find"))
            {
                Say("To find something valuable, one must first understand what they seek. What do you believe is the most valuable?");
            }
            else if (speech.Contains("valuable"))
            {
                Say("Value is often a reflection of effort and knowledge. Tell me, what efforts have you made in your quest?");
            }
            else if (speech.Contains("efforts"))
            {
                Say("Efforts are measured by perseverance and wisdom. Have you encountered challenges on your path?");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges shape our journey and our understanding. What have you learned from the challenges you faced?");
            }
            else if (speech.Contains("learned"))
            {
                Say("Learning comes from reflection and experience. Have you reflected on your experiences deeply?");
            }
            else if (speech.Contains("reflected"))
            {
                Say("Reflection leads to deeper insights. Based on your reflections, what do you believe is the essence of true reward?");
            }
            else if (speech.Contains("essence"))
            {
                Say("A true reward is not merely a physical object but the culmination of one's journey and understanding. Are you ready for a token of appreciation?");
            }
            else if (speech.Contains("token"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your journey and understanding have proven your worth. Accept this Trailblazer's Trove as a reward for your dedication.");
                    from.AddToBackpack(new TrailblazersTrove()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am here to share knowledge and reward those who prove their worth. If you seek something specific, please ask.");
            }

            base.OnSpeech(e);
        }

        public PercivalTreasuresmith(Serial serial) : base(serial) { }

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
