using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Don Diego del Plata")]
    public class DonDiegoDelPlata : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DonDiegoDelPlata() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Don Diego del Plata";
            Title = "the Conquistador";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });

            HairItemID = Utility.RandomList(0x203B, 0x203C); // Different styles of hair
            HairHue = Utility.RandomHairHue();

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
                Say("I am Don Diego del Plata, the noble Conquistador.");
            }
            else if (speech.Contains("job"))
            {
                Say("My mission is to guard the treasures of the New World and to ensure that only the worthy receive them.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you. The life of adventure keeps me fit.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! The true reward of the brave. Speak more if you wish to learn how to earn it.");
            }
            else if (speech.Contains("brave"))
            {
                Say("Only the brave can claim the treasure I guard. Show your courage through words and deeds.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is not the absence of fear, but the triumph over it. Your words show you have it.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait before I can bestow another reward.");
                }
                else
                {
                    Say("For your bravery and wisdom, I grant you the Conquistador's Hoard.");
                    from.AddToBackpack(new ConquistadorsHoard()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public DonDiegoDelPlata(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
