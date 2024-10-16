using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Bounty McRae")]
    public class CaptainBountyMcRae : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public CaptainBountyMcRae() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Bounty McRae";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Bounty Hunter";

            AddItem(new StuddedChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Bandana() { Hue = Utility.RandomMetalHue() });

            // Equip weapon
            AddItem(new Crossbow() { Name = "Bounty Hunter's Crossbow" });

            SetSkill(SkillName.Swords, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Parry, 70.0, 90.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ahoy, I be Captain Bounty McRae, the finest bounty hunter this side of the realm!");
            }
            else if (speech.Contains("health"))
            {
                Say("I’m as fit as a fiddle and ready for any challenge that comes my way!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To track down the scoundrels and seekers of treasure who cross my path. And perhaps to help those who prove themselves worthy.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure, ye say? Aye, I’ve got a special chest that holds many a valuable item. Prove yer worth, and it could be yours!");
            }
            else if (speech.Contains("worth"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("Prove yer worth by answering more questions or showing true valor. Only then will the treasure be yours.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for specific conditions, can be enhanced based on server needs
            return !m_RewardGiven;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                BountyHuntersCache chest = new BountyHuntersCache();
                from.AddToBackpack(chest);
                from.SendMessage("Congratulations! Ye have proven yerself worthy. Here’s the Bounty Hunter's Cache, full o' treasures!");
                m_RewardGiven = true;
            }
        }

        public CaptainBountyMcRae(Serial serial) : base(serial) { }

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
