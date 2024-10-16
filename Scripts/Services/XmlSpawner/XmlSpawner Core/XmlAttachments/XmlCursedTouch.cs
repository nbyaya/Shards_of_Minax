using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCursedTouch : XmlAttachment
    {
        private double m_DamageIncrease = 1.2; // 20% more damage taken by default
        private TimeSpan m_Duration = TimeSpan.FromSeconds(10); // Lasts for 10 seconds by default
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public double DamageIncrease { get { return m_DamageIncrease; } set { m_DamageIncrease = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        public XmlCursedTouch(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlCursedTouch(double damageIncrease)
        {
            m_DamageIncrease = damageIncrease;
        }

        [Attachable]
        public XmlCursedTouch(double damageIncrease, double durationInSeconds)
        {
            m_DamageIncrease = damageIncrease;
            m_Duration = TimeSpan.FromSeconds(durationInSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (defender == null || attacker == null) return;

            if (DateTime.Now > m_EndTime)
            {
                defender.SendMessage("You've been cursed!");
                defender.Damage((int)(damageGiven * m_DamageIncrease), attacker);
                m_EndTime = DateTime.Now + m_Duration;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_DamageIncrease);
            writer.Write(m_Duration);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_DamageIncrease = reader.ReadDouble();
                m_Duration = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Cursed Touch: Increases damage taken by " + (m_DamageIncrease * 100 - 100) + "% for " + m_Duration.TotalSeconds + " seconds.";
        }
    }
}
