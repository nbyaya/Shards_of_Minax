using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a forager")]
    public class Forager : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(10.0); // time between healing attempts
        public DateTime m_NextHealTime;

        [Constructable]
        public Forager() : base(AIType.AI_Healer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Forager";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Forager";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item sandals = new Sandals(Utility.RandomNeutralHue());
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(sandals);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(300, 500);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.EvalInt, 60.1, 80.0);
            SetSkill(SkillName.Healing, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 2000;
            Karma = 2000;

            VirtualArmor = 25;

            m_NextHealTime = DateTime.Now + m_HealDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextHealTime)
            {
                Mobile ally = FindAllyToHeal();
                if (ally != null)
                {
                    this.Say(true, "I will heal you, friend!");
                    this.DoBeneficial(ally);
                    ally.Hits += Utility.RandomMinMax(20, 50);
                    m_NextHealTime = DateTime.Now + m_HealDelay;
                }
            }
            base.OnThink();
        }

        private Mobile FindAllyToHeal()
        {
            foreach (Mobile m in this.GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.Hits < m.HitsMax && !m.IsDeadBondedPet && this.CanBeBeneficial(m, false, true))
                {
                    return m;
                }
            }
            return null;
        }

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            AddLoot(LootPack.Average);
            PackItem(new Ginseng(Utility.RandomMinMax(10, 20)));
        }

        public Forager(Serial serial) : base(serial)
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
