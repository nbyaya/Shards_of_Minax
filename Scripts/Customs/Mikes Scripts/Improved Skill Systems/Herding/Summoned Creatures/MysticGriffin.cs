using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic griffin corpse")]
    public class MysticGriffin : BaseCreature
    {
        private DateTime m_NextShield;
        private static readonly TimeSpan ShieldInterval = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan ShieldDuration = TimeSpan.FromSeconds(10);

        [Constructable]
        public MysticGriffin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic griffin";
            Body = 0x5;
            Hue = 1153; // Unique hue for the Mystic Griffin

            SetStr(150);
            SetDex(120);
            SetInt(200);

            SetHits(200);
            SetMana(300);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.0);
            SetSkill(SkillName.Magery, 90.0);
            SetSkill(SkillName.MagicResist, 85.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 80.0);

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextShield = DateTime.UtcNow + ShieldInterval;
        }

        public MysticGriffin(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextShield)
            {
                ShieldAllies();
                m_NextShield = DateTime.UtcNow + ShieldInterval;
            }
        }

        private void ShieldAllies()
        {
            Map map = this.Map;

            if (map == null)
                return;

            foreach (Mobile m in this.GetMobilesInRange(5))
            {
                if (m != this && IsAlly(m))
                {
                    m.SendMessage("You are shielded by the Mystic Griffin's magical energy!");
                    m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                    m.PlaySound(0x1ED);

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Protection, 1075812, ShieldDuration, m, "Protected by the Mystic Griffin"));
                }
            }
        }

        private bool IsAlly(Mobile m)
        {
            return (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == this.ControlMaster) || (m == this.ControlMaster);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextShield);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextShield = reader.ReadDateTime();
        }
    }
}
