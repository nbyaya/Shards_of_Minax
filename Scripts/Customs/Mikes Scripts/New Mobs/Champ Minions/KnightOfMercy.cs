using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a knight of mercy")]
    public class KnightOfMercy : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(10.0); // time between heals
        private DateTime m_NextHealTime;
        private bool m_HasResurrected;

        [Constructable]
        public KnightOfMercy() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = "Knight of Mercy";
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Name = NameList.RandomName("female");
            }
            else
            {
                Name = NameList.RandomName("male");
            }

            Item helm = new PlateHelm();
            Item chest = new PlateChest();
            Item legs = new PlateLegs();
            Item arms = new PlateArms();
            Item gloves = new PlateGloves();
            Item shield = new MetalKiteShield();
            Item sword = new Longsword();

            AddItem(helm);
            AddItem(chest);
            AddItem(legs);
            AddItem(arms);
            AddItem(gloves);
            AddItem(shield);
            AddItem(sword);

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 90.1, 100.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = 6000;

            VirtualArmor = 58;

            m_NextHealTime = DateTime.Now + m_HealDelay;
            m_HasResurrected = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextHealTime)
            {
                HealAlly();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }
        }

        private void HealAlly()
        {
            foreach (Mobile ally in this.GetMobilesInRange(10))
            {
                if (ally != this && ally.Alive && ally.Hits < ally.HitsMax && ally is BaseCreature && !(ally is BaseVendor))
                {
                    int toHeal = Utility.RandomMinMax(20, 40);
                    ally.Heal(toHeal);
                    this.Say("Be healed, friend!");
                    return;
                }
            }

            if (!m_HasResurrected)
            {
                foreach (Mobile ally in this.GetMobilesInRange(10))
                {
                    if (ally != this && !ally.Alive && ally is BaseCreature && !(ally is BaseVendor))
                    {
                        ally.Resurrect();
                        m_HasResurrected = true;
                        this.Say("Rise again, ally!");
                        return;
                    }
                }
            }
        }

        public KnightOfMercy(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_HasResurrected);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_HasResurrected = reader.ReadBool();
        }
    }
}
