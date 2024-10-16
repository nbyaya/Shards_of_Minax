using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPhantomBurn : XmlAttachment
    {
        private int m_Damage = 10; // Damage per tick
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(2); // Cooldown for the ability
        private DateTime m_NextPhantomBurn;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlPhantomBurn(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlPhantomBurn() { }

        [Attachable]
        public XmlPhantomBurn(int damage, double cooldown)
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
            writer.WriteDeltaTime(m_NextPhantomBurn);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextPhantomBurn = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Phantom Burn: {0} damage per tick with a cooldown of {1} minutes.", m_Damage, m_Cooldown.TotalMinutes);
        }

        public void CastPhantomBurn(Mobile attacker, Mobile target)
        {
            if (target != null && target.Alive && DateTime.UtcNow >= m_NextPhantomBurn)
            {
                target.SendMessage("You feel intense pain as the phantom burn affects you!");
                target.SendMessage("Your attacks are weakened by the phantom burn!");

                // Apply damage over time
                Timer.DelayCall(TimeSpan.FromSeconds(0), delegate { target.Damage(m_Damage, attacker); });
                Timer.DelayCall(TimeSpan.FromSeconds(1), delegate { target.Damage(m_Damage, attacker); });
                Timer.DelayCall(TimeSpan.FromSeconds(2), delegate { target.Damage(m_Damage, attacker); });

                // Use the attacker to send the overhead message
                attacker.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The attacker inflicts a phantom burn upon its target! *");
                m_NextPhantomBurn = DateTime.UtcNow + m_Cooldown; // Cooldown for Phantom Burn
            }
        }
    }
}
