using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlRallyingRoar : XmlAttachment
    {
        private int m_HealAmount = 15;
        private int m_Radius = 10;
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(5);
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HealAmount { get { return m_HealAmount; } set { m_HealAmount = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Radius { get { return m_Radius; } set { m_Radius = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlRallyingRoar(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlRallyingRoar() { }

        public override string OnIdentify(Mobile from)
        {
            return $"Rallying Roar: Heals allies in a {m_Radius}-tile radius by {m_HealAmount} HP.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextUse)
            {
                PerformRallyingRoar(attacker);
                m_NextUse = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformRallyingRoar(Mobile owner)
        {
            owner.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "*A powerful roar inspires nearby allies!*");
            foreach (Mobile m in owner.GetMobilesInRange(m_Radius))
            {
                if (m is BaseCreature && m != owner)
                {
                    m.Hits += m_HealAmount;
                    m.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "You feel invigorated by the roar!");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_HealAmount);
            writer.Write(m_Radius);
            writer.Write(m_Cooldown);
            writer.Write(m_NextUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_HealAmount = reader.ReadInt();
            m_Radius = reader.ReadInt();
            m_Cooldown = reader.ReadTimeSpan();
            m_NextUse = reader.ReadDateTime();
        }
    }
}
