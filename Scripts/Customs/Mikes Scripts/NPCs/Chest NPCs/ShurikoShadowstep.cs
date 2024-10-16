using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shuriko Shadowstep")]
    public class ShurikoShadowstep : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool hasMentionedSecrets;
        private bool hasMentionedStealth;
        private bool hasMentionedDiscipline;
        private bool hasMentionedPreparation;

        [Constructable]
        public ShurikoShadowstep() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shuriko Shadowstep";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 75;
            Int = 90;
            Hits = 75;

            // Appearance
            AddItem(new LeatherNinjaHood() { Hue = 1109 });
            AddItem(new LeatherNinjaJacket() { Hue = 1109 });
            AddItem(new LeatherLegs() { Hue = 1109 });
            AddItem(new Sandals() { Hue = 1109 });
            AddItem(new BlackStaff() { Hue = 1109 });

            Hue = Utility.RandomSkinHue(); // Random skin color
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Dark hair
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize flags
            lastRewardTime = DateTime.MinValue;
            hasMentionedSecrets = false;
            hasMentionedStealth = false;
            hasMentionedDiscipline = false;
            hasMentionedPreparation = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Shuriko Shadowstep, master of the shadows.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in peak condition, thanks to my rigorous training.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard the secrets of the ancient ninja code.");
            }
            else if (speech.Contains("secrets"))
            {
                if (!hasMentionedSecrets)
                {
                    Say("The secrets I guard are many. To reveal more, you must first understand the essence of stealth.");
                    hasMentionedSecrets = true;
                }
                else if (hasMentionedStealth && !hasMentionedDiscipline)
                {
                    Say("Stealth is just one part of the equation. Discipline is equally important. Only with both will you grasp the full code.");
                }
                else if (hasMentionedDiscipline && !hasMentionedPreparation)
                {
                    Say("Discipline is key, but preparation is crucial. Have you prepared yourself for the path of the ninja?");
                    hasMentionedPreparation = true;
                }
                else if (hasMentionedPreparation)
                {
                    Say("You have come far in understanding the code. What else do you seek?");
                }
            }
            else if (speech.Contains("stealth"))
            {
                if (hasMentionedSecrets && !hasMentionedStealth)
                {
                    Say("Stealth is an art. It requires patience, precision, and a deep understanding of one's surroundings.");
                    hasMentionedStealth = true;
                }
                else if (hasMentionedStealth && !hasMentionedDiscipline)
                {
                    Say("Your grasp of stealth is impressive. To advance further, you must learn about discipline.");
                }
                else if (hasMentionedDiscipline && !hasMentionedPreparation)
                {
                    Say("You are progressing well. Have you prepared yourself fully for the trials ahead?");
                }
            }
            else if (speech.Contains("discipline"))
            {
                if (hasMentionedStealth && !hasMentionedDiscipline)
                {
                    Say("Discipline is the key to mastering any skill. Without it, even the greatest talents are wasted.");
                    hasMentionedDiscipline = true;
                }
                else if (hasMentionedDiscipline && !hasMentionedPreparation)
                {
                    Say("Now that you understand discipline, are you prepared for the final challenge?");
                    hasMentionedPreparation = true;
                }
            }
            else if (speech.Contains("preparation"))
            {
                if (hasMentionedPreparation)
                {
                    Say("Preparation is essential for the path of the ninja. Your journey has led you to the brink of understanding.");
                }
            }
            else if (speech.Contains("key"))
            {
                if (hasMentionedPreparation)
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(15);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward for you at the moment. Please return later.");
                    }
                    else
                    {
                        Say("Your understanding of stealth, discipline, and preparation is commendable. As a reward for your insight, accept this chest of ninja treasures.");
                        from.AddToBackpack(new NinjaChest()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("You must prove your understanding of all the aspects of the ninja code before receiving the reward.");
                }
            }

            base.OnSpeech(e);
        }

        public ShurikoShadowstep(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasMentionedSecrets);
            writer.Write(hasMentionedStealth);
            writer.Write(hasMentionedDiscipline);
            writer.Write(hasMentionedPreparation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasMentionedSecrets = reader.ReadBool();
            hasMentionedStealth = reader.ReadBool();
            hasMentionedDiscipline = reader.ReadBool();
            hasMentionedPreparation = reader.ReadBool();
        }
    }
}
