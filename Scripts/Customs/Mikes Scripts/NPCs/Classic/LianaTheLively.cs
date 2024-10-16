using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Liana the Lively")]
    public class LianaTheLively : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LianaTheLively() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Liana the Lively";
            Body = 0x191; // Human female body

            // Stats
            Str = 82;
            Dex = 42;
            Int = 120;
            Hits = 72;

            // Appearance
            AddItem(new Robe() { Hue = 64 });
            AddItem(new Boots() { Hue = 2126 });
            AddItem(new LeatherGloves() { Name = "Liana's Mixing Mitts" });

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
                Say("I am Liana the Lively, the alchemist. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What's it to you? I'm not your nurse.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm an alchemist, not that it's any of your business.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Knowledge is power, and I have plenty of it. Do you seek wisdom?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Wisdom, huh? You don't strike me as the wise type.");
            }
            else if (speech.Contains("liana"))
            {
                Say("Ah, you've heard of me? Not surprising, I'm quite renowned in these parts. Especially for a special potion I once crafted.");
            }
            else if (speech.Contains("nurse"))
            {
                Say("Hmph. Though I'm not a nurse, I've brewed concoctions that have healed many. But, I require rare ingredients for them.");
            }
            else if (speech.Contains("alchemist"))
            {
                Say("Indeed, being an alchemist is more than just mixing potions. It requires a deep connection to nature and its secrets.");
            }
            else if (speech.Contains("potion"))
            {
                Say("Ah, the legendary Elixir of Life. A potion that can heal any ailment, but the ingredients are hard to come by. If you fetch them for me, there might be a reward in it for you.");
            }
            else if (speech.Contains("ingredients"))
            {
                Say("I need the petal of a Moonflower, essence of a phoenix, and a vial of water from the Whispering Spring. Not easy to come by, but the reward will be worth it.");
            }
            else if (speech.Contains("nature"))
            {
                Say("Nature is a powerful force. She whispers her secrets to those who listen. In return, I respect and protect her.");
            }
            else if (speech.Contains("protect"))
            {
                Say("Over the years, I've saved many creatures and plants from harm. In gratitude, the land often bestows gifts upon me.");
            }
            else if (speech.Contains("gifts"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("From rare herbs to mystical artifacts, the land has given me many treasures. And for those who help me, I often share these gifts. Here have a sample.");
                    from.AddToBackpack(new HealingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LianaTheLively(Serial serial) : base(serial) { }

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
