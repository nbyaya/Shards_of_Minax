using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the charred remains of Ignatius Emberbrand")]
    public class IgnatiusEmberbrand : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public IgnatiusEmberbrand() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ignatius Emberbrand";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            AddItem(new Cloak() { Hue = 1358, Name = "Cloak of Flames" });

            Hue = Race.RandomSkinHue(); // Fire-themed hue
            HairItemID = 0x2040; // Fiery red hair
            HairHue = 0x64; // Fiery red hair color
            FacialHairItemID = 0; // No facial hair

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
                Say("I am Ignatius Emberbrand, keeper of the infernal secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as fiery as the inferno itself, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the secrets of the Infernal Plane Chest, a treasure beyond compare.");
            }
            else if (speech.Contains("infernal"))
            {
                Say("The Infernal Plane Chest holds great power. Those who seek it must prove their worth.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must answer the following: What is the essence of fire?");
            }
            else if (speech.Contains("essence"))
            {
                Say("Correct! The essence of fire is destruction and rebirth. Speak of rebirth, and I shall reveal more.");
            }
            else if (speech.Contains("rebirth"))
            {
                Say("Rebirth is the cycle through which the old is consumed to give way to the new. The fire consumes to renew. Speak of destruction, and you will be closer to your reward.");
            }
            else if (speech.Contains("destruction"))
            {
                Say("Destruction clears the path for renewal, burning away the old to create the new. Speak of treasures, and you shall be rewarded.");
            }
            else if (speech.Contains("treasures"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your wisdom and perseverance, accept this chest of infernal treasures. May it serve you well.");
                    from.AddToBackpack(new InfernalPlaneChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("The flames of knowledge burn brightly. Seek the answers in the heart of the inferno.");
            }

            base.OnSpeech(e);
        }

        public IgnatiusEmberbrand(Serial serial) : base(serial) { }

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
