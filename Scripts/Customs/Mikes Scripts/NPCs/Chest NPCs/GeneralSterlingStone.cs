using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of General Sterling Stone")]
    public class GeneralSterlingStone : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GeneralSterlingStone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "General Sterling Stone";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });
            
            AddItem(new Scimitar() { Name = "Hero's Scimitar", Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue(); // Skin color
            HairItemID = 0x203B; // Short hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x203E; // Mustache
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

            // Initial Dialogue
            if (speech.Contains("name"))
            {
                Say("Greetings, soldier. I am General Sterling Stone. Are you here to learn about valor?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the essence of courage in the face of danger. But bravery alone is not enough. Do you understand what makes a hero?");
            }
            else if (speech.Contains("hero"))
            {
                Say("A hero is defined by their actions and choices, not just their deeds. But do you know the true meaning of bravery?");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery means standing firm despite fear. It's about pushing forward even when you feel overwhelmed. Have you ever faced a challenge that tested your bravery?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges forge our character. Overcoming them is what builds true valor. But what about endurance? Do you value it?");
            }
            else if (speech.Contains("endurance"))
            {
                Say("Endurance is the strength to persist through adversity. It’s crucial for lasting victory. Do you believe in the power of perseverance?");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the steady persistence in a course of action. It’s vital for long-term success. But do you understand the importance of leadership?");
            }
            else if (speech.Contains("leadership"))
            {
                Say("Leadership is about guiding others with vision and strength. It encompasses all the qualities of valor, bravery, and endurance. Are you ready to prove yourself?");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove yourself, one must act with honor and dedication. Show me that you understand these values, and I might have something special for you.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the respect and esteem earned through noble actions. It’s the cornerstone of true bravery. What about loyalty?");
            }
            else if (speech.Contains("loyalty"))
            {
                Say("Loyalty is the unwavering allegiance to a cause or person. It reinforces the virtues of bravery and valor. Are you prepared for a test?");
            }
            else if (speech.Contains("test"))
            {
                Say("A test of character is the ultimate trial. If you are ready to face it, demonstrate your qualities of valor, bravery, and honor.");
            }
            else if (speech.Contains("demonstrate"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your understanding of these virtues is commendable. For your dedication and knowledge, accept this WWII Valor Chest as a token of our gratitude.");
                    from.AddToBackpack(new WWIIValorChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("thank"))
            {
                Say("You're welcome. May your path be guided by the virtues of valor and honor.");
            }

            base.OnSpeech(e);
        }

        public GeneralSterlingStone(Serial serial) : base(serial) { }

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
