using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlChillTouch : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for Chill Touch
        private double m_AttackSpeedReduction = 0.15; // 15% attack speed reduction
        private TimeSpan m_EffectDuration = TimeSpan.FromSeconds(15); // Duration of the effect
        private double m_TriggerChance = 0.15; // 15% chance to trigger on melee attack

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public double AttackSpeedReduction { get { return m_AttackSpeedReduction; } set { m_AttackSpeedReduction = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public double TriggerChance { get { return m_TriggerChance; } set { m_TriggerChance = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan EffectDuration { get { return m_EffectDuration; } set { m_EffectDuration = value; } }

        public XmlChillTouch(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlChillTouch() { }

        [Attachable]
        public XmlChillTouch(double triggerChance, int damage, double attackSpeedReduction, double effectDuration)
        {
            TriggerChance = triggerChance;
            Damage = damage;
            AttackSpeedReduction = attackSpeedReduction;
            EffectDuration = TimeSpan.FromSeconds(effectDuration);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_AttackSpeedReduction);
            writer.Write(m_TriggerChance);
            writer.Write(m_EffectDuration);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_AttackSpeedReduction = reader.ReadDouble();
                m_TriggerChance = reader.ReadDouble();
                m_EffectDuration = reader.ReadTimeSpan();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Chill Touch: {0}% chance to inflict {1} damage and reduce attack speed by {2}% for {3} seconds.", m_TriggerChance * 100, m_Damage, m_AttackSpeedReduction * 100, m_EffectDuration.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (Utility.RandomDouble() < m_TriggerChance)
            {
                defender.Damage(m_Damage, attacker);
                Effects.SendLocationParticles(EffectItem.Create(defender.Location, defender.Map, EffectItem.DefaultDuration), 0x374A, 10, 15, 5023); // Visual effect for Chill Touch
                defender.PlaySound(0x1FB); // Sound effect for Chill Touch
				attacker.Say("Chilled!");

                if (defender is BaseCreature)
                {
                    (defender as BaseCreature).CurrentSpeed *= (1 + m_AttackSpeedReduction); // Increasing by reduction amount, which slows the creature
                    Timer.DelayCall(m_EffectDuration, delegate { (defender as BaseCreature).CurrentSpeed /= (1 + m_AttackSpeedReduction); }); // Reset speed after duration
                }
                // Implement player speed reduction if applicable
            }
        }
    }
}
