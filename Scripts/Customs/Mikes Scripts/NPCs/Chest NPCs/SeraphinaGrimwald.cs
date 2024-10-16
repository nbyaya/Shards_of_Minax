using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Seraphina Grimwald")]
    public class SeraphinaGrimwald : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SeraphinaGrimwald() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Seraphina Grimwald";
            Title = "the Enigmatic Seer";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 75;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new HoodedShroudOfShadows() { Hue = 1150 });
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new Spellbook() { Name = "Seraphina's Tome of Shadows" });

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = Race.RandomHairHue();

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
                Say("I am Seraphina Grimwald, the Enigmatic Seer of the coven.");
            }
            else if (speech.Contains("health"))
            {
                Say("My spirit is well, though the weight of ancient secrets can be heavy.");
            }
            else if (speech.Contains("job"))
            {
                Say("I weave spells and guard the secrets of the coven. My role is to protect our mystical heritage.");
            }
            else if (speech.Contains("coven"))
            {
                Say("The coven is a place of great power and mystery. Many seek our knowledge, but few are truly worthy.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like shadows. They can be unveiled, but they always remain elusive. Seek with intent.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries of the arcane are best approached with caution. Only the pure of heart may uncover their truths.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The time for reward has not yet come. Return after some time.");
                }
                else
                {
                    Say("Your inquisitiveness has earned you a reward. May the coven's treasures aid you in your journey.");
                    from.AddToBackpack(new CovenTreasuresChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public SeraphinaGrimwald(Serial serial) : base(serial) { }

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
