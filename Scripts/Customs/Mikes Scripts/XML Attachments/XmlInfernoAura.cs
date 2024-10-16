using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlInfernoAura : XmlAttachment
    {
        private int m_MinDamage = 5; // Minimum damage of the aura
        private int m_MaxDamage = 10; // Maximum damage of the aura
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown period for the aura
        private DateTime m_NextInfernoAura;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlInfernoAura(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlInfernoAura() { }

        [Attachable]
        public XmlInfernoAura(int minDamage, int maxDamage, double cooldown)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextInfernoAura);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextInfernoAura = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Inferno Aura: Damages players within range for {0} to {1} damage every minute.", m_MinDamage, m_MaxDamage);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextInfernoAura)
            {
                ApplyInfernoAura(attacker);
                m_NextInfernoAura = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void ApplyInfernoAura(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Player && m.Alive)
                {
                    m.SendMessage("The intense heat from the aura burns you!");
                    m.Damage(Utility.RandomMinMax(m_MinDamage, m_MaxDamage), owner); // Damage inflicted by the owner
                }
            }
        }
    }
}
