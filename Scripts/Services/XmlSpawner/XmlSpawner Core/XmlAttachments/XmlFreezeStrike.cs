using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFreezeStrike : XmlAttachment
    {
        private TimeSpan m_Duration = TimeSpan.FromSeconds(5); // default freeze duration is 5 seconds
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        public XmlFreezeStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFreezeStrike()
        {
        }

        [Attachable]
        public XmlFreezeStrike(double durationInSeconds)
        {
            Duration = TimeSpan.FromSeconds(durationInSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (defender != null && DateTime.Now >= m_EndTime)
            {
                defender.Freeze(Duration);
                defender.SendMessage("You've been frozen!");
                m_EndTime = DateTime.Now + Duration;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Duration);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Duration = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Freeze on strike for " + m_Duration.TotalSeconds + " secs";
        }
    }
}
