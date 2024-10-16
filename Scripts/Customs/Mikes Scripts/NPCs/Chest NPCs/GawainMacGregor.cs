using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gawain MacGregor")]
    public class GawainMacGregor : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public GawainMacGregor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gawain MacGregor";
            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();
            Title = "the Bard";

            // Appearance
            AddItem(new Robe(Utility.RandomGreenHue())); // Celtic-themed robe
            AddItem(new Sandals());
            AddItem(new DeerMask()); // Custom headpiece, if available
            AddItem(new QuarterStaff() { Name = "Gawain's Mystical Staff", Hue = 2309 });

            // Stats
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Musicianship, 75.0, 100.0);
            SetSkill(SkillName.Parry, 75.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Gawain MacGregor, a humble bard and keeper of legends.");
            }
            else if (speech.Contains("job"))
            {
                Say("I wander the lands, sharing tales of old and safeguarding ancient relics of the Celtic lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you. The legends of old keep my spirit strong.");
            }
            else if (speech.Contains("celtic legends"))
            {
                Say("Ah, the Celtic Legends Chest! It is said to hold treasures from the mists of time, blessed by the ancient druids themselves.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("The treasures within the chest are a blend of magical artifacts and mystical relics, each with its own tale to tell.");
            }
            else if (speech.Contains("relics"))
            {
                Say("The relics are enchanted by the old gods, each holding a part of the ancient lore. Seek them with a pure heart.");
            }
            else if (speech.Contains("reward"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("You must prove your worth before receiving the treasure.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                CelticLegendsChest chest = new CelticLegendsChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have earned the respect of the legends. Take this Celtic Legends Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public GawainMacGregor(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
