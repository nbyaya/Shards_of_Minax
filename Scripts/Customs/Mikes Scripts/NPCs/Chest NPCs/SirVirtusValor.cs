using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Virtus Valor")]
    public class SirVirtusValor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirVirtusValor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Virtus Valor";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
			
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2046); // Short and long hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x204B, 0x204E); // Beard styles

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
                Say("Greetings, I am Sir Virtus Valor, defender of the virtues. Do you seek knowledge?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge of virtues guides us. Ask me about 'honesty' or 'valor' to proceed.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the bedrock of trust. In the path of honesty, you must also understand 'compassion'.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is a virtue that binds us together. Reflect upon 'courage' as you ponder compassion.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to face adversity. With courage, you can explore the virtue of 'humility'.");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility teaches us to understand our place. To fully grasp humility, one must also understand 'truth'.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is the light that guides us. Knowing truth will lead you to seek 'valor' once more.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just bravery, but a testament to one's honor. Through valor, we find 'honor' and 'duty'.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the essence of our deeds. With honor, one must also fulfill their 'duty'.");
            }
            else if (speech.Contains("duty"))
            {
                Say("Duty is a call to action. As you fulfill your duty, you may reflect on 'sacrifice'.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Sacrifice is the ultimate expression of virtue. Consider how 'dedication' plays into sacrifice.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is key to perseverance. With dedication, you are ready to undertake a 'quest'.");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest for virtue is a noble pursuit. If you are prepared for the journey, you may now seek 'enlightenment'.");
            }
            else if (speech.Contains("enlightenment"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received a reward. Return later for further enlightenment.");
                }
                else
                {
                    Say("You have shown great understanding of the virtues. For your efforts, accept this chest, a symbol of our highest virtues.");
                    from.AddToBackpack(new VirtuesGuardianChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues are a guide to a noble life. Ask about 'honesty', 'compassion', 'courage', 'humility', 'truth', 'valor', 'honor', 'duty', 'sacrifice', or 'dedication' to learn more.");
            }

            base.OnSpeech(e);
        }

        public SirVirtusValor(Serial serial) : base(serial) { }

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
