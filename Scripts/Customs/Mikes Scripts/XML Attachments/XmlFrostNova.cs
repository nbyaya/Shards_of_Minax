using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFrostNova : XmlAttachment
    {
        private int m_Damage = 15; // Damage dealt by the frost nova
        private TimeSpan m_Duration = TimeSpan.FromSeconds(4); // Duration of the freeze effect
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(2); // Cooldown period for the ability
        private DateTime m_NextFrostNova;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlFrostNova(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFrostNova() { }

        [Attachable]
        public XmlFrostNova(int damage, double duration, double cooldown)
        {
            Damage = damage;
            Duration = TimeSpan.FromSeconds(duration);
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Duration);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextFrostNova);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Duration = reader.ReadTimeSpan();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextFrostNova = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Frost Nova: Deals {0} damage and freezes enemies for {1} seconds every {2} minutes.", m_Damage, m_Duration.TotalSeconds, m_Cooldown.TotalMinutes);
        }

        public void TriggerFrostNova(Mobile owner)
        {
            if (DateTime.UtcNow >= m_NextFrostNova)
            {
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a frost nova *");
                Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 30); // Adjusted line

                foreach (Mobile m in owner.GetMobilesInRange(5))
                {
                    if (m != owner && m.Player)
                    {
                        m.SendMessage("A wave of frost radiates around you, chilling you to the bone!");
                        m.Freeze(m_Duration);
                        m.Damage(m_Damage, owner);
                    }
                }

                m_NextFrostNova = DateTime.UtcNow + m_Cooldown; // Reset cooldown
            }
        }
    }
}
