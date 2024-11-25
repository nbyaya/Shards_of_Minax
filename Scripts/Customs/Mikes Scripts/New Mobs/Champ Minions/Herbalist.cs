using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a BattleHerbalist")]
    public class BattleHerbalist : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(30.0); // time between area heals
        private DateTime m_NextHealTime;

        [Constructable]
        public BattleHerbalist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the BattleHerbalist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the BattleHerbalist";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomGreenHue();
            AddItem(robe);

            Item shoes = new Sandals();
            shoes.Hue = Utility.RandomNeutralHue();
            AddItem(shoes);

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

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 70.0, 90.0);
            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 50;

            m_NextHealTime = DateTime.Now + m_HealDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextHealTime)
            {
                PerformAreaHeal();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }

            base.OnThink();
        }

        private void PerformAreaHeal()
        {
            IPooledEnumerable eable = this.GetMobilesInRange(5);

            foreach (Mobile m in eable)
            {
                if (m != this && m is BaseCreature && ((BaseCreature)m).ControlMaster != null)
                {
                    m.Heal(Utility.RandomMinMax(20, 30), this);
                    m.SendMessage("You feel a warm energy as the BattleHerbalist heals you.");
                    CurePoison(m);
                }
            }

            eable.Free();
        }

        private void CurePoison(Mobile m)
        {
            if (m.Poisoned)
            {
                m.CurePoison(this);
                m.SendMessage("You feel a cleansing energy as the BattleHerbalist detoxifies you.");
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            PackItem(new GreaterHealPotion(Utility.RandomMinMax(2, 5)));
            PackItem(new CurePotion(Utility.RandomMinMax(2, 5)));
        }

        public BattleHerbalist(Serial serial) : base(serial)
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
