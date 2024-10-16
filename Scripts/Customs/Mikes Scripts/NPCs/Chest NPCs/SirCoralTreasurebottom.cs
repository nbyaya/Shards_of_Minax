using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Coral Treasurebottom")]
    public class SirCoralTreasurebottom : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool spokeAboutSea = false;
        private bool spokeAboutExploration = false;
        private bool spokeAboutDiscovery = false;
        private bool spokeAboutCourage = false;
        private bool spokeAboutWisdom = false;
        private bool spokeAboutTreasures = false;

        [Constructable]
        public SirCoralTreasurebottom() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Coral Treasurebottom";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new LeatherChest() { Hue = 1153 }); // Sea-themed hue
            AddItem(new LeatherLegs() { Hue = 1153 });
            AddItem(new LeatherArms() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new FeatheredHat() { Hue = 1153 });

            // Hair and facial hair
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
                Say("Ahoy! I am Sir Coral Treasurebottom, guardian of the deep sea treasures.");
            }
            else if (speech.Contains("sea") && !spokeAboutSea)
            {
                Say("The sea holds many mysteries. Have you ever ventured into its depths?");
                spokeAboutSea = true;
            }
            else if (speech.Contains("venture") && spokeAboutSea && !spokeAboutExploration)
            {
                Say("Exploration of the sea can lead to great discoveries. What have you found on your journeys?");
                spokeAboutExploration = true;
            }
            else if (speech.Contains("discovery") && spokeAboutExploration && !spokeAboutDiscovery)
            {
                Say("Every discovery has its tale. Tell me, have you uncovered any ancient secrets?");
                spokeAboutDiscovery = true;
            }
            else if (speech.Contains("secrets") && spokeAboutDiscovery && !spokeAboutCourage)
            {
                Say("Uncovering secrets requires courage. What drives you to seek the unknown?");
                spokeAboutCourage = true;
            }
            else if (speech.Contains("courage") && spokeAboutCourage && !spokeAboutWisdom)
            {
                Say("Courage and wisdom often go hand in hand. Have you learned anything from your trials?");
                spokeAboutWisdom = true;
            }
            else if (speech.Contains("wisdom") && spokeAboutWisdom && !spokeAboutTreasures)
            {
                Say("Wisdom leads to understanding, and understanding reveals the true value of treasures. Are you in search of treasures?");
                spokeAboutTreasures = true;
            }
            else if (speech.Contains("treasures") && spokeAboutTreasures)
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Return when the tides are more favorable.");
                }
                else
                {
                    Say("You have proven your curiosity and valor. As a token of appreciation, take this Mermaid's Treasure Chest!");
                    from.AddToBackpack(new MermaidTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Ah, it seems you have not yet ventured deep enough into the mysteries of the sea.");
            }

            base.OnSpeech(e);
        }

        public SirCoralTreasurebottom(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(spokeAboutSea);
            writer.Write(spokeAboutExploration);
            writer.Write(spokeAboutDiscovery);
            writer.Write(spokeAboutCourage);
            writer.Write(spokeAboutWisdom);
            writer.Write(spokeAboutTreasures);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            spokeAboutSea = reader.ReadBool();
            spokeAboutExploration = reader.ReadBool();
            spokeAboutDiscovery = reader.ReadBool();
            spokeAboutCourage = reader.ReadBool();
            spokeAboutWisdom = reader.ReadBool();
            spokeAboutTreasures = reader.ReadBool();
        }
    }
}
