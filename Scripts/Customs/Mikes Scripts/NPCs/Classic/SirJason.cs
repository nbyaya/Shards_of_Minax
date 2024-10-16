using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Jason")]
    public class SirJason : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirJason() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Jason";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 80;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 37 });
            AddItem(new PlateChest() { Hue = 37 });
            AddItem(new PlateHelm() { Hue = 37 });
            AddItem(new PlateGloves() { Hue = 37 });
            AddItem(new Boots() { Hue = 37 });
            AddItem(new BattleAxe() { Name = "Sir Jason's Axe" });

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
                Say("I am Sir Jason, the enigmatic wanderer of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("My existence is but a shadow, my health an illusion.");
            }
            else if (speech.Contains("job"))
            {
                Say("I wander through the mists, seeking knowledge and enlightenment.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Knowledge is the weapon I wield. Can you grasp the secrets I offer?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Your response is intriguing. What questions burn within you?");
            }
            else if (speech.Contains("enigmatic"))
            {
                Say("Once, I was a simple knight, but a fateful encounter in the Crystal Caves changed me forever.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("Long ago, a sorcerer's curse bound my essence to the twilight, causing me to exist between day and night.");
            }
            else if (speech.Contains("mists"))
            {
                Say("The mists hold many mysteries. I've seen things within them that few would believe: ancient cities, lost souls, and keys to forgotten realms.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("There are ancient scrolls scattered across the land, each holding a piece of the ultimate truth. Find them, and the path to enlightenment may open for you.");
            }
            else if (speech.Contains("questions"))
            {
                Say("The realm holds many secrets. Seek the Eternal Flame hidden within the Heart of the Forest. To those deemed worthy, I offer a reward for its discovery.");
            }
            else if (speech.Contains("crystal"))
            {
                Say("The caves are a labyrinth of wonder and danger. It's where I found an artifact of unimaginable power, which is now lost.");
            }
            else if (speech.Contains("sorcerer"))
            {
                Say("The sorcerer's name was Eldrin. Enraged by my defiance, he cast me into this half-existence. But there may be a way to break his enchantment.");
            }
            else if (speech.Contains("lost"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The souls are trapped, yearning for release. Should you find them, a benevolent spirit may grant you a boon. A sample for you.");
                    from.AddToBackpack(new ForensicEvaluationAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirJason(Serial serial) : base(serial) { }

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
