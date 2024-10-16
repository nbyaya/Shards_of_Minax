using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlHeavenlyStrike : XmlAttachment
    {
        private int m_BaseDamage = 40; // Base damage for the heavenly strike
        private int m_DamageRange = 20; // Random damage range
        private TimeSpan m_NextStrikeTime = TimeSpan.FromMinutes(1); // Time between strikes
        private DateTime m_NextStrike;

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaseDamage { get { return m_BaseDamage; } set { m_BaseDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageRange { get { return m_DamageRange; } set { m_DamageRange = value; } }

        public XmlHeavenlyStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlHeavenlyStrike() { }

        [Attachable]
        public XmlHeavenlyStrike(int baseDamage, int damageRange, double nextStrikeMinutes)
        {
            BaseDamage = baseDamage;
            DamageRange = damageRange;
            m_NextStrikeTime = TimeSpan.FromMinutes(nextStrikeMinutes);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_BaseDamage);
            writer.Write(m_DamageRange);
            writer.Write(m_NextStrikeTime);
            writer.WriteDeltaTime(m_NextStrike);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_BaseDamage = reader.ReadInt();
                m_DamageRange = reader.ReadInt();
                m_NextStrikeTime = reader.ReadTimeSpan();
                m_NextStrike = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Heavenly Strike: Deals {0} damage with an additional {1} random damage.", m_BaseDamage, m_DamageRange);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextStrike)
            {
                PerformHeavenlyStrike(attacker, defender);
                m_NextStrike = DateTime.UtcNow + m_NextStrikeTime;
            }
        }

        private void PerformHeavenlyStrike(Mobile attacker, Mobile target)
        {
            if (target != null && target.Alive)
            {
                // Send a light beam effect
                Effects.SendTargetParticles(target, 0x1F5, 16, 16, 0, EffectLayer.Waist, 0);
                
                // Calculate damage
                int totalDamage = m_BaseDamage + Utility.Random(m_DamageRange);
                target.Damage(totalDamage, attacker);
                
                // Send overhead message
                target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A beam of light strikes you from the heavens! *");
            }
        }
    }
}
