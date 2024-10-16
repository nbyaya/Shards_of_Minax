using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Fancy Faye")]
    public class FancyFaye : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FancyFaye() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Fancy Faye";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new Skirt() { Hue = 38 });
            AddItem(new FemaleLeatherChest() { Hue = 38 });
            AddItem(new ThighBoots() { Hue = 38 });
            AddItem(new FeatheredHat() { Hue = 2126 });
            AddItem(new Cutlass() { Name = "Faye's Rapier" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("Arr, I be Fancy Faye, the most illustrious pirate ye ever laid eyes on!");
            }
            else if (speech.Contains("health"))
            {
                Say("Me health? Ha, a pirate's health be as good as the rum she drinks!");
            }
            else if (speech.Contains("job"))
            {
                Say("Me job? Well, I'm a pirate, ain't I? Stealing treasure and causing mayhem, that's what I do best!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Ye think ye can handle the life of a pirate, landlubber?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ah, ye've got a bit o' fire in ye! But can ye stand tall in a storm, or will ye cower like a scared seagull?");
            }
            else if (speech.Contains("illustrious"))
            {
                Say("Ah, ye've heard of me reputation, have ya? Many tales have been spun 'bout the legendary adventures of Fancy Faye. Ever heard of the Cursed Emerald of Calypso?");
            }
            else if (speech.Contains("rum"))
            {
                Say("Ah, rum! The lifeblood of any self-respectin' pirate. I've had many a variety, but none as exquisite as the Golden Goblet Rum from Tortuga's hidden distillery.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Aye, treasure is what we seek, but not just any shiny trinket. The most prized of them all is the Lost Medallion of Marauders' Cove. Many have tried to find it, but all have failed.");
            }
            else if (speech.Contains("storm"))
            {
                Say("Storms are the test of a true pirate's mettle! But there's one storm that even I avoid - the Tempest of the Forsaken Abyss. Sail there, and ye might not return.");
            }
            else if (speech.Contains("emerald"))
            {
                Say("That emerald, it's said to grant untold power to its possessor, but at a heavy cost. I've been searching for it, and if ye help me, there might be a reward in it for ye.");
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
                    Say("Very well, if ye prove to be helpful in me quest for the Cursed Emerald, I might part with some of me treasured doubloons or maybe even a piece of a treasure map. A sample for ye.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("goblet"))
            {
                Say("They say that rum is brewed in a secret location, guarded by the Ghostly Pirates of Tortuga. Anyone who tries to find it either joins their crew or never returns.");
            }
            else if (speech.Contains("medallion"))
            {
                Say("Legend has it that the medallion is hidden deep within Marauders' Cove, protected by puzzles and traps that have claimed many a pirate's life.");
            }

            base.OnSpeech(e);
        }

        public FancyFaye(Serial serial) : base(serial) { }

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
