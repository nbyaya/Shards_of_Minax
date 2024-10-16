using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Qi Gong healer")]
    public class QiGongHealer : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(10.0); // time between healing actions
        public DateTime m_NextHealTime;

        [Constructable]
        public QiGongHealer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Qi Gong Healer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Qi Gong Healer";
            }

            Item robe = new Robe();
            robe.Hue = 0x48E; // Blue color
            AddItem(robe);

            Item sandals = new Sandals();
            sandals.Hue = 0x48E; // Matching blue color
            AddItem(sandals);

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

            SetStr(400, 600);
            SetDex(200, 300);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Healing, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, 90.1, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);

            Tamable = false;
            ControlSlots = 2;
            MinTameSkill = 0;

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 40;

            m_NextHealTime = DateTime.Now + m_HealDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextHealTime)
            {
                HealAllies();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }
        }

        private void HealAllies()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                {
                    if (m.Hits < m.HitsMax)
                    {
                        m.Heal(Utility.RandomMinMax(10, 20));
                        m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                        m.PlaySound(0x1F2);
                        this.Say("Be healed, my friend!");
                    }
                }
            }
        }

        public QiGongHealer(Serial serial) : base(serial)
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
