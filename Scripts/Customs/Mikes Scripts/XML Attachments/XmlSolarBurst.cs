using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSolarBurst : XmlAttachment
    {
        private int m_Damage = 30; // Damage dealt by the burst
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(3); // Cooldown duration
        private DateTime m_NextSolarBurst;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlSolarBurst(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSolarBurst() { }

        [Attachable]
        public XmlSolarBurst(int damage, double cooldownMinutes)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromMinutes(cooldownMinutes);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextSolarBurst);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextSolarBurst = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Solar Burst: Deals {0} damage with a cooldown of {1} minutes.", m_Damage, m_Cooldown.TotalMinutes);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextSolarBurst)
            {
                PerformSolarBurst(attacker);
                m_NextSolarBurst = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformSolarBurst(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Solar Burst! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x37B9, 10, 16); // Large burst effect

            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m != owner && m.Alive)
                {
                    m.SendMessage("You are caught in a powerful burst of sunlight!");
                    m.Damage(m_Damage, owner); // Damage inflicted by the owner
                }
            }
        }
    }
}
