using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Francis Drake")]
    public class SirFrancisDrake : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirFrancisDrake() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Francis Drake";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 100;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1105 });
            AddItem(new LeatherChest() { Hue = 1105 });
            AddItem(new LeatherGloves() { Hue = 1105 });
            AddItem(new TricorneHat() { Hue = 1105 });
            AddItem(new Boots() { Hue = 1105 });
            AddItem(new Cutlass() { Name = "Sir Drake's Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Ahoy there, I be Sir Francis Drake!");
            }
            else if (speech.Contains("health"))
            {
                Say("Me health be as good as a scallywag's heart!");
            }
            else if (speech.Contains("job"))
            {
                Say("I be a pirate of fortune and misfortune, me job be seekin' treasures and avoidin' the hangman's noose!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Life be a cruel sea, matey. Have ye ever faced a storm with naught but a toothpick for a ship?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Aye, that be the spirit! But remember, even a pirate can find calm waters if he sails wisely.");
            }
            else if (speech.Contains("drake"))
            {
                Say("Named after the great English sea captain and pirate. Many tales are told of me exploits, but not all know of the map.");
            }
            else if (speech.Contains("scallywag"))
            {
                Say("Me heart may be of a scallywag, but it's got a fire that burns fierce! Once, it even led me to the cursed isle.");
            }
            else if (speech.Contains("hangman"))
            {
                Say("Aye, the hangman's noose has been close to me neck many times. But a quick wit and quicker blade always see me through. There was that one time in Tortuga though...");
            }
            else if (speech.Contains("map"))
            {
                Say("This map I speak of, it be leading to a hidden treasure I've yet to find. But beware, it's said to be guarded by the ghost of Blackbeard.");
            }
            else if (speech.Contains("cursed"))
            {
                Say("That cursed isle, filled with dangerous creatures and hidden traps. But the legend says there's a gem there that can control the seas. Seek it out if ye dare!");
            }
            else if (speech.Contains("tortuga"))
            {
                Say("Ah, Tortuga! A haven for pirates like me. I once won a mysterious amulet in a dice game there. If you help me with a small task, it's yours.");
            }
            else if (speech.Contains("blackbeard"))
            {
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    Say("The ghost of Blackbeard, a fierce adversary in life and even more so in death. To best him, ye need the enchanted saber hidden on Cutthroat Island. Use this!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("I have no more help for you right now. Return later for further assistance.");
                }
            }
            else if (speech.Contains("gem"))
            {
                Say("Legend speaks of its power to command the waves and calm the fiercest storms. But many have sought it and failed. Return it to me, and I'll grant you a piece of me own treasure.");
            }
            else if (speech.Contains("amulet"))
            {
                Say("This amulet, some say it can protect its wearer from the deadly sirens of the deep. I've no need for it now. Prove your worth, and it's yours!");
            }

            base.OnSpeech(e);
        }

        public SirFrancisDrake(Serial serial) : base(serial) { }

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
