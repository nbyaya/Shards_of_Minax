using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Mace-a-Lot")]
    public class SirMaceaLot : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool talkedAboutMace;
        private bool talkedAboutStrength;
        private bool talkedAboutValor;

        [Constructable]
        public SirMaceaLot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Mace-a-Lot";
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
            AddItem(new Mace() { Hue = Utility.RandomMetalHue() });

            // Random appearance tweaks
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize variables
            lastRewardTime = DateTime.MinValue;
            talkedAboutMace = false;
            talkedAboutStrength = false;
            talkedAboutValor = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Sir Mace-a-Lot, the guardian of the mighty mace!");
                talkedAboutMace = true;
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as a steel mace. I am ready for any challenge!");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the secrets of the Mace Master's treasure.");
            }
            else if (speech.Contains("mace"))
            {
                if (talkedAboutMace)
                {
                    Say("Ah, the mighty mace! A weapon of great power and strength.");
                    talkedAboutStrength = true;
                }
                else
                {
                    Say("You need to first ask me about my name.");
                }
            }
            else if (speech.Contains("strength"))
            {
                if (talkedAboutMace && talkedAboutStrength)
                {
                    Say("The strength of a mace lies not just in its heft but in the wielder's heart. Show me your valor, and the reward shall be yours.");
                    talkedAboutValor = true;
                }
                else
                {
                    Say("You must first discuss the mace with me.");
                }
            }
            else if (speech.Contains("valor"))
            {
                if (talkedAboutStrength && talkedAboutValor)
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward for you right now. Please return later.");
                    }
                    else
                    {
                        Say("Your valor is commendable. For your courage and resolve, accept this chest as a token of our appreciation.");
                        from.AddToBackpack(new MacingBonusChest()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("You must first prove your strength.");
                }
            }
            else if (speech.Contains("treasure"))
            {
                if (talkedAboutMace)
                {
                    Say("The treasure of the Mace Master is a grand prize indeed. But you must prove your worth first.");
                }
                else
                {
                    Say("You must first ask me about my name.");
                }
            }
            else if (speech.Contains("worth"))
            {
                if (talkedAboutStrength)
                {
                    Say("To prove your worth, answer me this: What is the true strength of a mace?");
                }
                else
                {
                    Say("You must first discuss the mace with me.");
                }
            }
            else if (speech.Contains("strength"))
            {
                if (talkedAboutStrength)
                {
                    Say("The strength of a mace lies not just in its heft but in the wielder's heart. Show me your valor, and the reward shall be yours.");
                }
                else
                {
                    Say("You must first discuss the mace with me.");
                }
            }
            else if (speech.Contains("valor"))
            {
                if (talkedAboutStrength)
                {
                    Say("Your valor is commendable. Prove it, and the reward shall be yours.");
                }
                else
                {
                    Say("You must first prove your strength.");
                }
            }

            base.OnSpeech(e);
        }

        public SirMaceaLot(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(talkedAboutMace);
            writer.Write(talkedAboutStrength);
            writer.Write(talkedAboutValor);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            talkedAboutMace = reader.ReadBool();
            talkedAboutStrength = reader.ReadBool();
            talkedAboutValor = reader.ReadBool();
        }
    }
}
