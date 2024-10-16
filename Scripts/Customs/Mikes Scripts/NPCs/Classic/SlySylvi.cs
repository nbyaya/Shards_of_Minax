using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sly Sylvi")]
    public class SlySylvi : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SlySylvi() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sly Sylvi";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 100;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Cloak() { Hue = 1109 });
            AddItem(new LeatherSkirt() { Hue = 1109 });
            AddItem(new LeatherBustierArms() { Hue = 1109 });
            AddItem(new Sandals() { Hue = 1109 });
            AddItem(new LeatherGloves() { Name = "Sylvi's Sneaky Gloves" });
            AddItem(new Dagger() { Name = "Sylvi's Stiletto" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Sly Sylvi, the rogue of these parts!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Well, let's just say I'm nimble enough to avoid most troubles.");
            }
            else if (speech.Contains("job"))
            {
                Say("My line of work, you ask? I tread the shadows, seeking treasures and secrets.");
            }
            else if (speech.Contains("treasures") || speech.Contains("secrets"))
            {
                Say("True rogues move with cunning and finesse. Do you have a quick wit, traveler?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If you value agility in your pursuits, then we may share common ground.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Remember, in the shadows, the eight virtues can guide one's path or obscure it.");
            }
            else if (speech.Contains("rogue"))
            {
                Say("The term 'rogue' might be associated with mischief, but we are seekers of truth and treasure. Do you not wish to uncover the hidden?");
            }
            else if (speech.Contains("nimble"))
            {
                Say("Being nimble isn't just about physical agility, it's about anticipating danger and understanding your surroundings. Ever heard of the Whispering Caverns?");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are more than just absence of light. They hold mysteries, tales, and sometimes, the past that wishes to stay hidden. Ever wondered about the Lost City?");
            }
            else if (speech.Contains("finesse"))
            {
                Say("Finesse is the art of treading without a trace, taking without being noticed. But, there's an old artifact I've been seeking. Perhaps, if you aid me, I might share its glory with you.");
            }
            else if (speech.Contains("agility"))
            {
                Say("Agility is a testament to one's adaptability. On my travels, I've discovered many relics. One in particular is the Moonlit Medallion. Seek it, and your endeavors might be rewarded.");
            }
            else if (speech.Contains("lost"))
            {
                Say("Ah, the Lost City of Elara! It's said to be a place of immeasurable wealth and ancient secrets. But, it's not the riches I seek but a particular gem. Aid me in its retrieval, and I shall reward you generously.");
            }
            else if (speech.Contains("artifact"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("This artifact is the Starlight Dagger, lost for centuries. It's said to possess a power beyond comprehension. Help me find it, and I'll ensure you are handsomely compensated.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SlySylvi(Serial serial) : base(serial) { }

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
