using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Aura the Alchemist")]
    public class AuraTheAlchemist : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AuraTheAlchemist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Aura the Alchemist";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 40;
            Int = 120;
            Hits = 70;

            // Appearance
            AddItem(new Robe(1156)); // Kimono with hue 1156
            AddItem(new ThighBoots(38)); // Thigh boots with hue 38
            AddItem(new LeatherGloves() { Name = "Aura's Protective Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Aura the Alchemist, and you are?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Do I look like a healer to you?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I dabble in the mystic arts of alchemy, but what's it to you?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom comes from mastering the unknown. Do you seek knowledge?");
            }
            else if (speech.Contains("philosopher's stone"))
            {
                Say("Then prove it! Answer me this: What is the philosopher's stone?");
            }
            else if (speech.Contains("alchemy"))
            {
                Say("Alchemy is the art of transforming base materials into wondrous concoctions. Are you interested in learning some basic recipes?");
            }
            else if (speech.Contains("recipes"))
            {
                Say("There are many recipes, some simple and others complex. One of the most basic is the elixir of healing. Do you wish to learn how to make it?");
            }
            else if (speech.Contains("elixir"))
            {
                Say("Ah! The elixir of healing. To make it, you'd need fresh mandrake root and a vial of pure spring water. Bring me these, and I'll show you the way.");
            }
            else if (speech.Contains("mandrake"))
            {
                Say("Mandrake root is a rare herb, often found in marshy areas. It's quite potent. If you manage to find some for me, I might have a reward for you.");
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
                    Say("Ah, eager are we? If you bring me the mandrake root, I shall bestow upon you a special concoction of my own making. It's effects? Well, that's a surprise.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("unknown"))
            {
                Say("The unknown is where true discoveries lie. In my studies, I've often faced mysteries and puzzles. One that eludes me is the riddle of the moonstone. Ever heard of it?");
            }
            else if (speech.Contains("moonstone"))
            {
                Say("The moonstone is said to be a gem that captures the essence of moonlight. Legends say it's hidden deep within a forgotten cave. Find it, and its secrets could be yours.");
            }
            else if (speech.Contains("cave"))
            {
                Say("The cave of which I speak is said to be protected by elemental spirits. Tread carefully if you seek it, for its guardians are not to be trifled with.");
            }

            base.OnSpeech(e);
        }

        public AuraTheAlchemist(Serial serial) : base(serial) { }

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
