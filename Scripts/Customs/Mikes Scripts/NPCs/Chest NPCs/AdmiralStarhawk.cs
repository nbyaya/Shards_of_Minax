using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Admiral Starhawk")]
    public class AdmiralStarhawk : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AdmiralStarhawk() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Admiral Starhawk";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            AddItem(new FancyShirt() { Name = "Starfleet Uniform" });

            Hue = 2157;
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Random hair
            HairHue = 2843;

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
                Say("Greetings, I am Admiral Starhawk, commander of the Galactic Fleet.");
            }
            else if (speech.Contains("admiral"))
            {
                Say("Indeed, as Admiral, my role involves overseeing the fleet's operations and maintaining galactic peace.");
            }
            else if (speech.Contains("fleet"))
            {
                Say("The fleet is a grand assembly of starships and warriors dedicated to protecting the galaxy.");
            }
            else if (speech.Contains("galaxy"))
            {
                Say("Our galaxy is vast and filled with countless wonders and dangers. We strive to keep it in balance.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Maintaining balance is crucial. It ensures that no single force becomes too dominant, preserving harmony.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony within the galaxy means peace among planets and civilizations. It is a delicate state to uphold.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace is the ultimate goal. It allows civilizations to thrive and explore the stars without conflict.");
            }
            else if (speech.Contains("explore"))
            {
                Say("Exploration is a key aspect of our mission. Discovering new worlds and cultures enriches our understanding of the universe.");
            }
            else if (speech.Contains("universe"))
            {
                Say("The universe is an endless expanse, full of mysteries yet to be unraveled. Our quest is to explore and understand it.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries drive us forward. Each discovery adds to our knowledge and brings us closer to understanding the cosmos.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Each discovery opens new pathways and possibilities. It is through exploration that we advance our knowledge.");
            }
            else if (speech.Contains("pathways"))
            {
                Say("Pathways in the galaxy can lead to unexpected encounters and adventures. They are integral to our mission.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Adventures are the lifeblood of exploration. They challenge us and reveal new facets of the galaxy.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are opportunities to grow and improve. Each one teaches us valuable lessons about ourselves and the universe.");
            }
            else if (speech.Contains("lessons"))
            {
                Say("Lessons learned from challenges shape our future decisions and actions, guiding us on our path.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path of exploration is filled with trials and triumphs. It requires courage and determination.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is essential for facing the unknown. It allows us to venture into the depths of space with confidence.");
            }
            else if (speech.Contains("confidence"))
            {
                Say("Confidence in our abilities ensures that we can tackle any obstacle that comes our way.");
            }
            else if (speech.Contains("obstacle"))
            {
                Say("Obstacles test our resolve and resourcefulness. Overcoming them makes us stronger and more capable.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is what drives us to persevere through difficulties. It is the heart of our quest.");
            }
            else if (speech.Contains("quest"))
            {
                Say("Our quest is to seek out new knowledge, maintain peace, and explore the galaxy's wonders.");
            }
            else if (speech.Contains("wonders"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your dedication and insight, accept this Galactic Relics Chest. It contains many treasures and artifacts from across the galaxy.");
                    from.AddToBackpack(new GalacticRelicsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public AdmiralStarhawk(Serial serial) : base(serial) { }

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
