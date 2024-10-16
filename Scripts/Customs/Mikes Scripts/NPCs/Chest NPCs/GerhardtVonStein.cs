using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gerhardt von Stein")]
    public class GerhardtVonStein : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GerhardtVonStein() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gerhardt von Stein";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("I am Gerhardt von Stein, a guardian of ancient relics. Do you seek the wisdom of the past?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from understanding the history and virtues of these relics. Have you heard of the relics?");
            }
            else if (speech.Contains("relics"))
            {
                Say("Indeed, the relics are powerful symbols of ancient valor. Each relic has its own story. Do you value history?");
            }
            else if (speech.Contains("history"))
            {
                Say("History shapes our present and guides our future. The relics are a testament to that. Do you appreciate valor?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the courage to stand for what is right. It is a trait seen in many great heroes. Have you ever proven your worth?");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth is to show true integrity and strength. Have you faced trials of endurance?");
            }
            else if (speech.Contains("endurance"))
            {
                Say("Endurance in the face of adversity reveals one's true character. Do you seek knowledge of ancient virtues?");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as honor, humility, and wisdom guide our actions. Which virtue speaks to you the most?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the foundation of true valor. It guides our actions and decisions. How do you define integrity?");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the steadfast adherence to moral principles. It is crucial for anyone seeking to uncover secrets. Have you considered the secrets of relics?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets held within relics are profound. Only those who show dedication may uncover them. Are you prepared to face a trial?");
            }
            else if (speech.Contains("trial"))
            {
                Say("A trial is a test of your worthiness. If you are ready to face it, you may prove yourself and earn the ultimate reward. Will you undertake this challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must return later to earn the reward.");
                }
                else
                {
                    Say("Your dedication and perseverance have proven your worth. Accept this Teutonic Relic Chest as a symbol of your valor.");
                    from.AddToBackpack(new TeutonicRelicChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the key to overcoming all obstacles. It reflects your true commitment. The relics you seek are a testament to such virtues.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment is essential for anyone seeking greatness. Your willingness to engage in this dialogue shows your dedication. Embrace this knowledge as your reward.");
            }

            base.OnSpeech(e);
        }

        public GerhardtVonStein(Serial serial) : base(serial) { }

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
