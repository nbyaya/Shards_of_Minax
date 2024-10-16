using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Morgana")]
    public class LadyMorgana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyMorgana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Morgana";
            Body = 0x191; // Human female body

            // Stats
            Str = 158;
            Dex = 70;
            Int = 32;
            Hits = 115;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1160 });
            AddItem(new NorseHelm() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1160 });
            AddItem(new ThighBoots() { Hue = 1160 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Lady Morgana, a knight of Valor!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to uphold the virtue of Valor by defending the weak and standing against injustice.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor is the strength to face danger with courage and resolve. Do you consider yourself valiant?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, facing adversity with courage is the path to true valor. How do you exhibit your courage in your daily life?");
            }
            else if (speech.Contains("knight"))
            {
                Say("Yes, I am a knight trained in the arts of combat and chivalry. I've journeyed far and wide, facing numerous challenges to uphold my oath.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Indeed, maintaining a strong physique is essential for a knight. Daily training and a disciplined lifestyle keep me in prime condition.");
            }
            else if (speech.Contains("injustice"))
            {
                Say("Injustice is a bane on our lands. I've encountered many who misuse their power, and I strive to correct their wrongdoings and protect the innocent.");
            }
            else if (speech.Contains("valiant"))
            {
                Say("Being valiant is not just about fighting battles. It's about standing up for what's right, even when it's hard. I've faced many decisions where the right path wasn't the easiest.");
            }
            else if (speech.Contains("decisions"))
            {
                Say("Every decision I make is guided by the virtues I uphold. Sometimes, it means making sacrifices for the greater good. For those who truly understand the essence of valor and make selfless choices, I have a reward.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you are interested in the reward! Here, take this. It's a token of appreciation for those who seek knowledge and show interest in the virtues of knighthood.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LadyMorgana(Serial serial) : base(serial) { }

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
