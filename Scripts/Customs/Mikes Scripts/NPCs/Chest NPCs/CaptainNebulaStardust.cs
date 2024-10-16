using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Nebula Stardust")]
    public class CaptainNebulaStardust : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainNebulaStardust() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Nebula Stardust";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1157 });
            AddItem(new StuddedChest() { Hue = 1157 });
            AddItem(new ChainCoif() { Hue = 1157 });
            AddItem(new StuddedGloves() { Hue = 1157 });
            AddItem(new Boots() { Hue = 1157 });
            AddItem(new Spellbook() { Name = "Nebula's Logbook" });
			
            Hue = 2222;
            HairItemID = 0x203B; // Spiky Hair
            HairHue = 1264;

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
                Say("Greetings, spacefarer! I am Captain Nebula Stardust, explorer of the cosmos.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is stellar, thanks to the wonders of the galaxy.");
            }
            else if (speech.Contains("job"))
            {
                Say("I chart the unknown regions of space and seek out hidden treasures across the stars.");
            }
            else if (speech.Contains("explore"))
            {
                Say("Exploration is the essence of adventure! The galaxy holds countless mysteries.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Indeed, mysteries abound. Do you seek to uncover them?");
            }
            else if (speech.Contains("uncover"))
            {
                Say("To uncover is to reveal what is hidden. What do you wish to reveal?");
            }
            else if (speech.Contains("reveal"))
            {
                Say("To reveal something requires dedication. Have you the resolve to seek deeper truths?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is essential for any explorer. It drives you to face the unknown.");
            }
            else if (speech.Contains("unknown"))
            {
                Say("The unknown can be daunting, but it is also where the greatest discoveries lie.");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("Discoveries can change the course of one's journey. What have you found on your travels?");
            }
            else if (speech.Contains("travels"))
            {
                Say("Travels are what shape an explorer's destiny. Each journey leaves a mark upon us.");
            }
            else if (speech.Contains("journey"))
            {
                Say("A journey through the stars is a wondrous thing. It changes you forever.");
            }
            else if (speech.Contains("forever"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Please return later.");
                }
                else
                {
                    Say("Your quest for knowledge and adventure is admirable. As a token of appreciation, take this Galactic Explorer's Trove!");
                    from.AddToBackpack(new GalacticExplorersTrove()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! The reward for daring adventurers. Do you seek something special?");
            }
            else if (speech.Contains("special"))
            {
                Say("The special things are often the most rewarding. What makes something special to you?");
            }
            else if (speech.Contains("you"))
            {
                Say("I am but a humble explorer, seeking the wonders of the cosmos. And you?");
            }
            else if (speech.Contains("wonders"))
            {
                Say("The cosmos is full of wonders waiting to be discovered. Do you seek to understand them?");
            }
            else if (speech.Contains("understand"))
            {
                Say("Understanding comes with patience and curiosity. What have you learned on your path?");
            }
            else if (speech.Contains("path"))
            {
                Say("The path of an explorer is paved with challenges and rewards. How do you navigate yours?");
            }
            else if (speech.Contains("navigate"))
            {
                Say("Navigating through the stars requires skill and wisdom. How skilled are you in your explorations?");
            }
            else if (speech.Contains("skilled"))
            {
                Say("Skill is honed through experience and persistence. Have you faced any great challenges recently?");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges are what test our resolve and courage. What challenges have you encountered?");
            }
            else if (speech.Contains("encountered"))
            {
                Say("Encounters with the unknown can be both thrilling and daunting. Share your tales with me.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Tales of adventure are the heart of every explorer's journey. Tell me, what is your greatest tale?");
            }

            base.OnSpeech(e);
        }

        public CaptainNebulaStardust(Serial serial) : base(serial) { }

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
