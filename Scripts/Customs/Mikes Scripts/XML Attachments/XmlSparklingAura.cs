using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSparklingAura : XmlAttachment
    {
        private int m_Damage = 5; // Damage dealt to nearby players
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextSparklingAura;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlSparklingAura(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSparklingAura() { }

        [Attachable]
        public XmlSparklingAura(int damage, double refractory)
        {
            Damage = damage;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextSparklingAura);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Refractory = reader.ReadTimeSpan();
                m_NextSparklingAura = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextSparklingAura)
            {
                PerformSparklingAura(attacker);
                m_NextSparklingAura = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformSparklingAura(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A sparkling aura surrounds the area! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x3728, 10, 16); // Aura effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("The sparkling aura burns your skin!");
                    m.Damage(m_Damage, owner); // Minor damage
                    m.SendMessage("Your attacks feel sluggish!");
                    m.SendMessage("Your attack speed is reduced!");
                }
            }
        }
    }
}
