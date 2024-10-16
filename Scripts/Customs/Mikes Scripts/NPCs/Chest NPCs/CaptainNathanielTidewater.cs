using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Nathaniel Tidewater")]
    public class CaptainNathanielTidewater : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool spokeOfJob;
        private bool spokeOfTreasure;
        private bool spokeOfProve;
        private bool spokeOfRiddles;
        private bool spokeOfSeaOrAdventure;
        private bool spokeOfGoldOrJewels;

        [Constructable]
        public CaptainNathanielTidewater() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Nathaniel Tidewater";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new TricorneHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new Cloak() { Hue = Utility.RandomBlueHue() });
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldNecklace() { Hue = Utility.RandomMetalHue() });

            Hue = Race.RandomSkinHue(); // Hair and beard
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
            spokeOfJob = false;
            spokeOfTreasure = false;
            spokeOfProve = false;
            spokeOfRiddles = false;
            spokeOfSeaOrAdventure = false;
            spokeOfGoldOrJewels = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ahoy! I be Captain Nathaniel Tidewater, master of the seas. Have ye heard of me?");
            }
            else if (speech.Contains("heard"))
            {
                Say("Aye, I've sailed many waters and met many souls. What brings ye to me?");
            }
            else if (speech.Contains("job"))
            {
                Say("I be the captain of this here vessel, and me job is to guard me precious treasure.");
                spokeOfJob = true;
            }
            else if (speech.Contains("guard") && spokeOfJob)
            {
                Say("Aye, guardin' me treasure is no easy task. But I seek to find someone worthy of it.");
                spokeOfTreasure = true;
            }
            else if (speech.Contains("treasure") && spokeOfTreasure)
            {
                Say("To prove yer worthiness, ye must first solve me riddles about the sea.");
                spokeOfProve = true;
            }
            else if (speech.Contains("prove") && spokeOfProve)
            {
                Say("What be the most valuable thing a captain can possess? It's not gold or jewels.");
                spokeOfRiddles = true;
            }
            else if (speech.Contains("riddles") && spokeOfRiddles)
            {
                Say("Here's a hint: The true treasure lies beyond the horizon of mere material wealth.");
            }
            else if (speech.Contains("sea") && spokeOfRiddles)
            {
                Say("Ah, the sea! The greatest adventure of all. But adventure alone won't get ye the reward.");
                spokeOfSeaOrAdventure = true;
            }
            else if (speech.Contains("adventure") && spokeOfSeaOrAdventure)
            {
                Say("To earn me trust, ye must show courage and resilience. Can ye handle the trials of the deep?");
                spokeOfGoldOrJewels = true;
            }
            else if (speech.Contains("gold") && spokeOfSeaOrAdventure)
            {
                Say("Gold be shiny, but it fades. True courage and skill shine brighter.");
            }
            else if (speech.Contains("jewels") && spokeOfSeaOrAdventure)
            {
                Say("Jewels be precious, yet they are but baubles compared to the courage of a true sailor.");
            }
            else if (speech.Contains("courage") && spokeOfSeaOrAdventure)
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for ye right now. Return later, brave sailor.");
                }
                else
                {
                    Say("Ye've shown true courage and wisdom. For yer efforts, take this chest of treasures and secrets.");
                    from.AddToBackpack(new NavyCaptainsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                if (speech.Contains("sail") || speech.Contains("ocean"))
                {
                    Say("The ocean holds many secrets, but only the worthy will uncover them.");
                }
                else if (speech.Contains("secret"))
                {
                    Say("The secrets of the sea are vast and deep. Only those who earn my trust will uncover them.");
                }
                else if (speech.Contains("worthy"))
                {
                    Say("Worthy ye must be to earn the Captain's trust. Show me what ye’ve got!");
                }
            }

            base.OnSpeech(e);
        }

        public CaptainNathanielTidewater(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(spokeOfJob);
            writer.Write(spokeOfTreasure);
            writer.Write(spokeOfProve);
            writer.Write(spokeOfRiddles);
            writer.Write(spokeOfSeaOrAdventure);
            writer.Write(spokeOfGoldOrJewels);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            spokeOfJob = reader.ReadBool();
            spokeOfTreasure = reader.ReadBool();
            spokeOfProve = reader.ReadBool();
            spokeOfRiddles = reader.ReadBool();
            spokeOfSeaOrAdventure = reader.ReadBool();
            spokeOfGoldOrJewels = reader.ReadBool();
        }
    }
}
