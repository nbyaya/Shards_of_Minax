using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a star reader")]
    public class StarReader : BaseCreature
    {
        private TimeSpan m_BuffDelay = TimeSpan.FromSeconds(15.0); // time between buffs
        public DateTime m_NextBuffTime;

        [Constructable]
        public StarReader() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Star Reader";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Star Reader";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBlueHue();
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(200, 300);
            SetDex(150, 200);
            SetInt(400, 500);

            SetHits(150, 200);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 80.5, 90.0);
            SetSkill(SkillName.Tactics, 50.1, 60.0);
            SetSkill(SkillName.Wrestling, 20.1, 30.0);

            Fame = 6000;
            Karma = 6000;

            VirtualArmor = 40;

            m_NextBuffTime = DateTime.Now + m_BuffDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBuffTime)
            {
                BuffAllies();
                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }

            base.OnThink();
        }

        public void BuffAllies()
        {
            IPooledEnumerable eable = this.GetMobilesInRange(5);
            foreach (Mobile m in eable)
            {
                if (m is BaseCreature && m != this && !m.Deleted)
                {
                    BaseCreature bc = (BaseCreature)m;
                    if (bc.Controlled && bc.ControlMaster == this.ControlMaster)
                    {
                        m.SendMessage("You feel the guidance of the stars!");
                        m.Hits += 10;
                        m.Dex += 5;
                    }
                }
            }
            eable.Free();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);
            PackItem(new StarSapphire());
        }

        public StarReader(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
