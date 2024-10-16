using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Thomas Edisonfield")]
    public class ThomasEdison : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public ThomasEdison() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Thomas Edison";
            Title = "the Inventor";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomBlueHue()));

            SetSkill(SkillName.Inscribe, 80.0, 100.0);
            SetSkill(SkillName.Tinkering, 80.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings! I am Thomas Edison, the master of inventions.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to experiment with new ideas and create marvelous inventions.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thanks to my relentless pursuit of discovery!");
            }
            else if (speech.Contains("inventions"))
            {
                Say("Ah, inventions! They are the heart of progress. Do you have any questions about my creations?");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery is the journey of a lifetime. It opens doors to the unknown and reveals hidden wonders.");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress is driven by innovation and creativity. Each step forward is a triumph of human ingenuity.");
            }
            else if (speech.Contains("reward"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("You must show a keen interest in inventions before I can reward you.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions. This could be more complex based on your server's requirements.
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                EdisonsTreasureChest chest = new EdisonsTreasureChest();
                from.AddToBackpack(chest);
                Say("You have demonstrated a genuine curiosity and appreciation for inventions. Here is Edison's Treasure Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public ThomasEdison(Serial serial) : base(serial)
        {
        }

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
