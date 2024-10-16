using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lord Havoc")]
    public class LordHavoc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LordHavoc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lord Havoc";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 75;
            Int = 50;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1175 });
            AddItem(new PlateChest() { Hue = 1175 });
            AddItem(new PlateArms() { Hue = 1175 });
            AddItem(new PlateHelm() { Hue = 1175 });
            AddItem(new PlateGloves() { Hue = 1175 });
            AddItem(new ExecutionersAxe() { Name = "Lord Havoc's Axe" });

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
                Say("I am Lord Havoc, the Black Knight. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern to the likes of you. Do not waste my time!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I am a knight, or at least I was, before this wretched world turned against me.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is meaningless in a world filled with deceit and treachery. Are you a fool, like the rest?");
            }
            else if (speech.Contains("yes") && SpeechEntryIsTriggered(30))
            {
                Say("Ha! You amuse me with your words, but actions speak louder. Prove your worth, or be gone!");
            }
            else if (speech.Contains("havoc"))
            {
                Say("Havoc is not just a name, but a title. It was bestowed upon me after the great siege of Falter's Keep.");
            }
            else if (speech.Contains("concern"))
            {
                Say("Concern for others was my downfall. In my earlier days, I once let my guard down and paid dearly for it.");
            }
            else if (speech.Contains("knight"))
            {
                Say("I was once among the most respected knights in the land. But after the betrayal, I became a shadow of my former self.");
            }
            else if (speech.Contains("siege"))
            {
                Say("The siege of Falter's Keep was a bloodbath. I led my men bravely, but in the end, treachery from within led to our defeat.");
            }
            else if (speech.Contains("downfall"))
            {
                Say("Trusting those who seemed loyal was my greatest mistake. It's hard to trust anyone these days.");
            }
            else if (speech.Contains("betrayal"))
            {
                Say("The ones closest to me stabbed me in the back, both figuratively and literally. For that, I wandered the land seeking vengeance.");
            }
            else if (speech.Contains("vengeance"))
            {
                Say("My quest for vengeance took me to dark places. But if you truly wish to help, seek out the cursed amulet in the Whispering Caverns. Return it to me, and I shall reward you.");
            }
            else if (speech.Contains("amulet"))
            {
                Say("The cursed amulet is a symbol of the treachery that befell me. It is said to be imbued with dark magic. Handle with caution. Return it to me, and you shall be generously rewarded.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Your efforts won't be in vain. But I have no reward to give right now. Please return later.");
                }
                else
                {
                    Say("Your efforts won't be in vain. Bring the amulet to me, and I shall grant you something of great value from my personal collection.");
                    from.AddToBackpack(new AnatomyAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        private bool SpeechEntryIsTriggered(int entryNumber)
        {
            // This method is used to check if a certain speech entry should trigger a response
            // Placeholder implementation, adjust logic as necessary for your system
            return true; // For simplicity, always return true here
        }

        public LordHavoc(Serial serial) : base(serial) { }

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
