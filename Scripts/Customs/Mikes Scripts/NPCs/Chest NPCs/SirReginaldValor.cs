using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald Valor")]
    public class SirReginaldValor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginaldValor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Valor";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 85;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });
            AddItem(new Longsword() { Hue = Utility.RandomMetalHue() }); // For a touch of valor

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
                Say("Greetings, I am Sir Reginald Valor, a knight devoted to the virtues of chivalry.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as steadfast as my honor, good traveler.");
            }
            else if (speech.Contains("job"))
            {
                Say("My purpose is to uphold the ideals of chivalry and ensure justice and valor in all my deeds.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Ah, the virtues of chivalry are many. They guide us in our quest for honor and justice.");
            }
            else if (speech.Contains("chivalry"))
            {
                Say("Chivalry is a noble code that guides the actions of knights. It is built on honor, bravery, and courtesy.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the core of chivalry. It demands integrity and respect in all our dealings.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is the courage to stand firm in the face of danger. It is essential for any knight.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the willingness to face fear and adversity. It is a vital part of valor and honor.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just bravery but also the strength to act with honor and integrity.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice ensures that wrongs are righted and the innocent are protected. It is a sacred duty for all knights.");
            }
            else if (speech.Contains("duty"))
            {
                Say("Duty is the obligation to uphold the virtues and principles of knighthood. It guides our every action.");
            }
            else if (speech.Contains("principles"))
            {
                Say("The principles of knighthood are honor, bravery, and justice. They are the foundation of our code.");
            }
            else if (speech.Contains("code"))
            {
                Say("The code of chivalry is a set of ideals that guide the conduct of knights. It includes honor, bravery, and justice.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return when the time is right.");
                }
                else
                {
                    Say("For your dedication to the virtues and principles of chivalry, I present to you the Chest of Chivalry. May it aid you in your noble quests.");
                    from.AddToBackpack(new SpecialChivalryChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirReginaldValor(Serial serial) : base(serial) { }

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
