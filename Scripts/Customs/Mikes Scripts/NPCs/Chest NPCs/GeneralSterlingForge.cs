using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of General Sterling Forge")]
    public class GeneralSterlingForge : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GeneralSterlingForge() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "General Sterling Forge";
            Body = 0x190; // Male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Longsword() { Name = "Commander’s Saber" });

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
                Say("I am General Sterling Forge, commander of the civil forces.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as the iron of my armor.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the treasures and secrets of our past battles.");
            }
            else if (speech.Contains("forge"))
            {
                Say("The forge of battle tempers the steel of a soldier’s will. Only the worthiest may claim its rewards.");
            }
            else if (speech.Contains("battle"))
            {
                Say("The echoes of battle remind us of the sacrifices made and the victories won. Do you seek such glory?");
            }
            else if (speech.Contains("glory"))
            {
                Say("Glory is earned through valor and honor, not given. Prove your worth and you may find yourself rewarded.");
            }
            else if (speech.Contains("prove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Come back later.");
                }
                else
                {
                    Say("You have shown the spirit of a true warrior. Accept this Civil War Cache as your reward.");
                    from.AddToBackpack(new CivilWarCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("treasure"))
            {
                Say("The true treasure lies in the valor of one's actions and the lessons learned from history.");
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public GeneralSterlingForge(Serial serial) : base(serial)
        {
        }

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
