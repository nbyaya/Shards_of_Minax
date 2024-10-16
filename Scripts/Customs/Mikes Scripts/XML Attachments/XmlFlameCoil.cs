using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFlameCoil : XmlAttachment
    {
        private int m_Damage = 15; // Damage dealt per tick
        private int m_Ticks = 5; // Number of damage ticks
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(60); // Cooldown duration
        private DateTime m_NextFlameCoil;
        private Mobile m_Attacker; // Field to store the attacker

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Ticks { get { return m_Ticks; } set { m_Ticks = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlFlameCoil(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFlameCoil() { }

        [Attachable]
        public XmlFlameCoil(int damage, int ticks, double cooldown)
        {
            Damage = damage;
            Ticks = ticks;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Ticks);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextFlameCoil);
            writer.Write(m_Attacker); // Serialize attacker
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Ticks = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextFlameCoil = reader.ReadDeltaTime();
                m_Attacker = reader.ReadMobile(); // Deserialize attacker
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            m_Attacker = attacker; // Store the attacker

            if (DateTime.UtcNow >= m_NextFlameCoil && defender.Alive)
            {
                UseFlameCoil(defender);
                m_NextFlameCoil = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void UseFlameCoil(Mobile target)
        {
            if (target == null || !target.Alive)
                return;

            for (int i = 0; i < m_Ticks; i++)
            {
                target.Damage(m_Damage, m_Attacker); // Use the stored attacker
                target.SendMessage("You are engulfed in flames as a fiery coil wraps around you!");
                target.SendMessage("Your movement is hindered by the intense heat!");
                target.Dex -= 10;

                Timer.DelayCall(TimeSpan.FromSeconds(1), () => { });
            }

            // Optional: Add a visual message
            target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A fiery coil engulfs its target, burning with fierce heat! *");
        }
    }
}
