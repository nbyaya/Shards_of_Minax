using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlStarlightBurst : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for the starlight burst
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Fixed cooldown time
        private DateTime m_NextStarlightBurst;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlStarlightBurst(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlStarlightBurst() { }

        [Attachable]
        public XmlStarlightBurst(int damage, double cooldown)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextStarlightBurst);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextStarlightBurst = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Starlight Burst: {0} damage with a cooldown of {1} minutes.", m_Damage, m_Cooldown.TotalMinutes);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextStarlightBurst)
            {
                PerformStarlightBurst(attacker);
                m_NextStarlightBurst = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformStarlightBurst(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a burst of starlight *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16); // Particle effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Alive)
                {
                    m.SendMessage("You are blinded by the starlight!");
                    m.Damage(m_Damage, owner);

                    // Chance to blind
                    if (Utility.RandomDouble() < 0.25)
                    {
                        m.SendMessage("You are blinded by the starlight!");
                        m.Freeze(TimeSpan.FromSeconds(3));
                    }
                }
            }
        }
    }
}
