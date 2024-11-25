using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of George 'The General' Valor")]
    public class GeorgeTheGeneralValor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GeorgeTheGeneralValor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "George 'The General' Valor";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomNeutralHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomNeutralHue() });

            // Hair and Facial Hair
            HairItemID = Utility.RandomList(0x2040, 0x2041); // Short hair options
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2043, 0x2044); // Beard options
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
                Say("Greetings, brave soul. I am George 'The General' Valor. To understand my valor, you must first learn about honor.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a cornerstone of valor. It reflects our character. Do you seek to understand bravery?");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery means facing challenges with courage. To prove your bravery, one must first grasp the essence of valor.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is more than bravery. It is an unwavering commitment to righteousness. Can you show me your understanding of commitment?");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment involves dedication and sacrifice. Have you ever faced a true test of your dedication?");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication requires perseverance. Show me how you persevere in your quests, and we may discuss the essence of perseverance.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance means continuing despite obstacles. It's a crucial aspect of valor. Have you considered the role of courage in this process?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage fuels perseverance and valor. To reward your understanding of courage, we must discuss the nature of challenges.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges test our courage and commitment. If you can overcome these tests, you will prove your worthiness for the treasure.");
            }
            else if (speech.Contains("overcome"))
            {
                Say("Overcoming challenges shows true valor. If you have faced such challenges, tell me about them, and you may receive a special reward.");
            }
            else if (speech.Contains("special"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return later to prove your valor once again.");
                }
                else
                {
                    Say("Your journey through valor and bravery has impressed me. Accept this chest of great treasure as a reward for your courage and dedication.");
                    from.AddToBackpack(new SpecialWoodenChestWashington()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("journey"))
            {
                Say("A journey through valor and bravery is one of the highest honors. If you have completed such a journey, you are worthy of the treasure.");
            }

            base.OnSpeech(e);
        }

        public GeorgeTheGeneralValor(Serial serial) : base(serial) { }

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
