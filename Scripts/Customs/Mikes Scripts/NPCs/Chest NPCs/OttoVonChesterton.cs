using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Otto von Chesterton")]
    public class OttoVonChesterton : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OttoVonChesterton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Otto von Chesterton";
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
			
            Hue = 2267; // Skin hue
            HairItemID = 0x203B; // Specific hair style
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x203B; // Facial hair style
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
                Say("Greetings, I am Otto von Chesterton, guardian of this treasure.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the unity of the German states!");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to safeguard this historical chest and reward those worthy.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Ah, the unity of Germany—a remarkable achievement indeed.");
            }
            else if (speech.Contains("chest"))
            {
                Say("This chest holds treasures from the time of German unification. Only the most deserving may claim its contents.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The treasure you seek is hidden within this very chest. Prove your worthiness!");
            }
            else if (speech.Contains("prove"))
            {
                Say("Demonstrate your knowledge and commitment, and the chest shall be yours.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("To show commitment, you must first understand the history. Ask me about the German states.");
            }
            else if (speech.Contains("states"))
            {
                Say("Germany was once divided into many states. Ask me about Otto von Bismarck.");
            }
            else if (speech.Contains("bismarck"))
            {
                Say("Otto von Bismarck was the Iron Chancellor who unified Germany. Ask me about his policies.");
            }
            else if (speech.Contains("policies"))
            {
                Say("Bismarck's policies were pivotal in German unification. Ask me about the Franco-Prussian War.");
            }
            else if (speech.Contains("franco-prussian war"))
            {
                Say("The Franco-Prussian War was a significant conflict leading to German unification. Ask me about its consequences.");
            }
            else if (speech.Contains("consequences"))
            {
                Say("The war led to the consolidation of German states. Ask me about the role of the Prussian Kingdom.");
            }
            else if (speech.Contains("prussian kingdom"))
            {
                Say("Prussia played a central role in unifying Germany. Ask me about the Treaty of Versailles.");
            }
            else if (speech.Contains("treaty of versailles"))
            {
                Say("The Treaty of Versailles was significant but came after the unification. Ask me about the German Empire.");
            }
            else if (speech.Contains("german empire"))
            {
                Say("The German Empire was established after unification. Ask me about the Kaiser.");
            }
            else if (speech.Contains("kaiser"))
            {
                Say("The Kaiser was the Emperor of the German Empire. Ask me about his impact on German history.");
            }
            else if (speech.Contains("impact"))
            {
                Say("The Kaiser's impact was profound. Ask me about the role of the German people.");
            }
            else if (speech.Contains("people"))
            {
                Say("The German people played a crucial role in the unification. Ask me about the chest to prove your knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Your knowledge of German history is impressive. To receive your reward, demonstrate your commitment once more.");
            }
            else if (speech.Contains("commitment"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The chest remains closed for now. Return later to prove your worth.");
                }
                else
                {
                    Say("Your dedication has earned you the chest. Take it and may it serve you well.");
                    from.AddToBackpack(new GermanUnificationChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public OttoVonChesterton(Serial serial) : base(serial) { }

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
