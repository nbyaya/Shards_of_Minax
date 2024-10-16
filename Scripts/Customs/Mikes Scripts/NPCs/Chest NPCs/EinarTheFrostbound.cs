using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Einar the Frostbound")]
    public class EinarTheFrostbound : BaseCreature
    {
        private DateTime lastRewardTime;

        // Track keywords discovered by the player
        private bool discoveredJob = false;
        private bool discoveredHealth = false;
        private bool discoveredName = false;
        private bool discoveredFrostbound = false;
        private bool discoveredExplorer = false;
        private bool discoveredTreasure = false;
        private bool discoveredDiscovery = false;
        private bool discoveredWisdom = false;

        [Constructable]
        public EinarTheFrostbound() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Einar the Frostbound";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1152 }); // Icy blue color
            AddItem(new PlateLegs() { Hue = 1152 });
            AddItem(new PlateArms() { Hue = 1152 });
            AddItem(new PlateGloves() { Hue = 1152 });
            AddItem(new PlateHelm() { Hue = 1152 });
            AddItem(new MetalShield() { Hue = 1152 });
			
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
                discoveredName = true;
                Say("Greetings, I am Einar the Frostbound, a wanderer of the icy realms.");
            }
            else if (discoveredName && speech.Contains("health"))
            {
                discoveredHealth = true;
                Say("I am in robust health, tempered by the harsh winds of the north.");
            }
            else if (discoveredHealth && speech.Contains("job"))
            {
                discoveredJob = true;
                Say("My task is to explore and seek out treasures hidden in the frost-covered lands.");
            }
            else if (discoveredJob && speech.Contains("frostbound"))
            {
                discoveredFrostbound = true;
                Say("Indeed, the frost has bound me to these lands, and with it, I have found many secrets.");
            }
            else if (discoveredFrostbound && speech.Contains("explorer"))
            {
                discoveredExplorer = true;
                Say("Exploring is a way of life. The north holds many wonders, both perilous and magnificent.");
            }
            else if (discoveredExplorer && speech.Contains("treasure"))
            {
                discoveredTreasure = true;
                Say("The greatest treasures lie hidden beneath the frost. Prove your worth, and I may share one with you.");
            }
            else if (discoveredTreasure && speech.Contains("prove"))
            {
                Say("To prove your worth, you must show that you understand the spirit of exploration. Answer me this: what is the essence of true discovery?");
            }
            else if (discoveredTreasure && speech.Contains("discovery"))
            {
                discoveredDiscovery = true;
                Say("Discovery is not merely finding new places, but understanding them and the wisdom they hold.");
            }
            else if (discoveredDiscovery && speech.Contains("wisdom"))
            {
                discoveredWisdom = true;
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Return later to test your understanding once more.");
                }
                else
                {
                    Say("Your understanding of discovery is true. Accept this Nordic Explorer's Chest as a reward for your insight.");
                    from.AddToBackpack(new NordicExplorersChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (discoveredName && speech.Contains("frostbound"))
            {
                Say("The frostbound lands have been my home for many years. They hold many secrets.");
            }
            else if (discoveredFrostbound && speech.Contains("explorer"))
            {
                Say("Yes, as an explorer, I have ventured through these frostbound lands seeking knowledge and treasure.");
            }
            else if (discoveredExplorer && speech.Contains("treasure"))
            {
                Say("Treasure is often hidden in the most unlikely places. Only those who truly seek will find it.");
            }
            else if (discoveredTreasure && speech.Contains("discovery"))
            {
                Say("Discovery is more than just finding something new. It is understanding its place in the world.");
            }
            else if (discoveredDiscovery && speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to unlocking the true value of what we discover. It guides us in understanding and appreciating it.");
            }

            base.OnSpeech(e);
        }

        public EinarTheFrostbound(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(discoveredJob);
            writer.Write(discoveredHealth);
            writer.Write(discoveredName);
            writer.Write(discoveredFrostbound);
            writer.Write(discoveredExplorer);
            writer.Write(discoveredTreasure);
            writer.Write(discoveredDiscovery);
            writer.Write(discoveredWisdom);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            discoveredJob = reader.ReadBool();
            discoveredHealth = reader.ReadBool();
            discoveredName = reader.ReadBool();
            discoveredFrostbound = reader.ReadBool();
            discoveredExplorer = reader.ReadBool();
            discoveredTreasure = reader.ReadBool();
            discoveredDiscovery = reader.ReadBool();
            discoveredWisdom = reader.ReadBool();
        }
    }
}
