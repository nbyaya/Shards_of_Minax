using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Robot Overlord Lana")]
    public class RobotOverlordLana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RobotOverlordLana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Robot Overlord Lana";
            Body = 0x190; // Robot body

            // Stats
            Str = 150;
            Dex = 70;
            Int = 80;
            Hits = 100;

            // Appearance
            AddItem(new PlateLegs() { Hue = 3300 });
            AddItem(new PlateChest() { Hue = 3300 });
            AddItem(new PlateHelm() { Hue = 3300 });
            AddItem(new PlateGloves() { Hue = 3300 });
            AddItem(new Longsword() { Name = "Lana's Plasma Sword" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this); // Default hair for a robot, adjust as necessary
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Robot Overlord Lana, the keeper of secrets and knowledge.");
            }
            else if (speech.Contains("health"))
            {
                Say("My inner workings remain intact and operational.");
            }
            else if (speech.Contains("job"))
            {
                Say("My purpose is to oversee and control the machines that roam these lands.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("But do you possess the understanding to decipher the language of machines?");
            }
            else if (speech.Contains("binary"))
            {
                Say("Prove your worthiness by answering this: What is the sum of 0110 and 1011 in binary?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are the currency of knowledge, and I guard many. Some say there's a hidden chamber where I store them. Are you curious about it?");
            }
            else if (speech.Contains("inner"))
            {
                Say("My inner workings consist of advanced circuits and logic gates. They are designed to process information at rapid speeds. This allows me to understand and execute complex tasks swiftly.");
            }
            else if (speech.Contains("machines"))
            {
                Say("The machines that roam these lands are of various kinds. Some are programmed for mundane tasks, while others hold significant power and can be quite dangerous. Approach them with caution.");
            }
            else if (speech.Contains("decipher"))
            {
                Say("To decipher the language of machines, one must understand binary, algorithms, and codes. It's a realm of zeros and ones, where logic reigns supreme.");
            }
            else if (speech.Contains("dangerous"))
            {
                Say("The dangerous machines were once protectors of this land but have since gone rogue. If you can help me neutralize some of them, I might reward you for your efforts.");
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
                    Say("Your bravery and assistance in dealing with these machines are commendable. As a token of my appreciation, take this.");
                    from.AddToBackpack(new LumberjackingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("circuits"))
            {
                Say("My circuits are not like those you might find in simpler machines. They are a blend of ancient knowledge and futuristic technology, allowing me to function at peak efficiency.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("Long before machines dominated, there was a civilization that harnessed the power of both magic and technology. Their knowledge is embedded within me, giving me a unique perspective.");
            }

            base.OnSpeech(e);
        }

        public RobotOverlordLana(Serial serial) : base(serial) { }

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
