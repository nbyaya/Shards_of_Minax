using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Admiral Quantum Stellar")]
    public class AdmiralQuantumStellar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AdmiralQuantumStellar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Admiral Quantum Stellar";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Cloak() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Starfleet Command Manual" });

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
                Say("Greetings, traveler. I am Admiral Quantum Stellar, at your service.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, ready to navigate the stars.");
            }
            else if (speech.Contains("job"))
            {
                Say("My mission is to oversee the safeguarding of the Starfleet's most valuable artifacts.");
            }
            else if (speech.Contains("vault"))
            {
                Say("The Starfleet's Vault holds treasures of great importance to our mission. It is a repository of knowledge and artifacts.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Indeed, the treasures within the vault are diverse and valuable. Only those with a keen mind can unlock them.");
            }
            else if (speech.Contains("unlock"))
            {
                Say("To unlock the vault's secrets, one must demonstrate understanding and insight. Prove your worth, and you shall be rewarded.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Your perseverance has earned you a special reward. Accept this Starfleet's Vault as a token of our appreciation.");
                from.AddToBackpack(new StarfleetsVault()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
            else if (speech.Contains("starfleet"))
            {
                Say("Starfleet stands as a beacon of exploration and peace. Our mission is to seek out new worlds and new civilizations.");
            }
            else if (speech.Contains("exploration"))
            {
                Say("Exploration is the essence of discovery. It drives us to push beyond our limits and uncover the mysteries of the universe.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery often leads to new understanding. It is a cornerstone of our mission, allowing us to make new advancements.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding the universe requires patience and wisdom. We must listen and learn from all we encounter.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is acquired through experience and reflection. It guides us in making the right choices and decisions.");
            }
            else if (speech.Contains("choices"))
            {
                Say("Every choice we make has an impact. Choosing wisely is essential for success in any mission.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is achieved through determination and effort. It is the result of overcoming challenges and persevering.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges are opportunities for growth. Facing them head-on builds strength and resilience.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth comes from facing and overcoming challenges. It is an essential part of our journey and development.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Our journey is one of constant learning and evolution. It shapes us and prepares us for future endeavors.");
            }
            else if (speech.Contains("evolution"))
            {
                Say("Evolution is a process of gradual change and improvement. It helps us adapt and thrive in a dynamic universe.");
            }
            else if (speech.Contains("adapt"))
            {
                Say("To adapt is to respond to new conditions effectively. It ensures survival and success in ever-changing environments.");
            }
            else if (speech.Contains("survival"))
            {
                Say("Survival often requires resourcefulness and resilience. We must be prepared to face unexpected challenges.");
            }
            else if (speech.Contains("resourcefulness"))
            {
                Say("Resourcefulness involves finding creative solutions with available resources. It is a key trait for overcoming obstacles.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Obstacles are hurdles that test our resolve. Conquering them strengthens our abilities and fortifies our spirit.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the determination to pursue a goal despite difficulties. It drives us to persist and succeed.");
            }
            else if (speech.Contains("persistence"))
            {
                Say("Persistence is the continued effort to achieve something despite difficulties. It is crucial for overcoming challenges.");
            }
            else if (speech.Contains("effort"))
            {
                Say("Effort is the energy and determination put into achieving a goal. It often determines the outcome of our endeavors.");
            }
            else if (speech.Contains("endurance"))
            {
                Say("Endurance is the ability to withstand prolonged challenges. It is essential for maintaining progress in our missions.");
            }

            base.OnSpeech(e);
        }

        public AdmiralQuantumStellar(Serial serial) : base(serial) { }

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
