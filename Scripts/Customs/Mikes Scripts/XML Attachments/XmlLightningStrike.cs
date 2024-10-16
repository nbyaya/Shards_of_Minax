using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlLightningStrike : XmlAttachment
    {
        private int m_MinDamage = 30; // Minimum damage of the lightning strike
        private int m_MaxDamage = 50; // Maximum damage of the lightning strike
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextLightningStrike;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlLightningStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlLightningStrike() { }

        [Attachable]
        public XmlLightningStrike(int minDamage, int maxDamage, double refractory)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextLightningStrike);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_NextLightningStrike = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (defender != null && DateTime.UtcNow >= m_NextLightningStrike)
            {
                if (defender.Alive)
                {
                    int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
                    defender.Damage(damage, attacker);
                    Effects.SendLocationParticles(EffectItem.Create(defender.Location, defender.Map, EffectItem.DefaultDuration), 0x1F4, 10, 10, 0);
                    defender.SendMessage("You are struck by a powerful lightning bolt!");

                    m_NextLightningStrike = DateTime.UtcNow + m_Refractory;
                }
            }
        }
    }
}
