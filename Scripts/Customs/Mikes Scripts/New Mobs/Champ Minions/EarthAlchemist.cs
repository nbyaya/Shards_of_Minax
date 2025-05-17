using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of an earth alchemist")]
    public class EarthAlchemist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between alchemist's phrases
        public DateTime m_NextSpeechTime;

        [Constructable]
        public EarthAlchemist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomRedHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Earth Alchemist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Earth Alchemist";
            }

            AddItem(new Robe(Utility.RandomNeutralHue())); // Earth-toned robe
            AddItem(new Sandals(Utility.RandomNeutralHue()));

            HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2048); // Various hairstyles
            HairHue = Utility.RandomHairHue();



            SetStr(300, 320);
            SetDex(80, 100);
            SetInt(300, 320);
            SetHits(250, 270);

            SetDamage(5, 10);

            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 95.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                int phrase = Utility.Random(3);
                switch (phrase)
                {
                    case 0: Say("Feel the weight of the earth!"); break;
                    case 1: Say("You cannot escape the ground's grasp!"); break;
                    case 2: Say("I will bury you!"); break;
                }
                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Potions);
        }

        public override int TreasureMapLevel { get { return 2; } }

        public EarthAlchemist(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
