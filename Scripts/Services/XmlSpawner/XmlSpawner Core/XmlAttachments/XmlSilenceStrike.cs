using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSilenceStrike : XmlAttachment
    {
        private TimeSpan m_SilenceDuration = TimeSpan.FromSeconds(5); // 5 seconds default silence duration
        private DateTime m_EndTime;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10); // 10 seconds default time between activations

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan SilenceDuration { get { return m_SilenceDuration; } set { m_SilenceDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlSilenceStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSilenceStrike(double duration)
        {
            m_SilenceDuration = TimeSpan.FromSeconds(duration);
        }

        [Attachable]
        public XmlSilenceStrike(double duration, double refractory)
        {
            m_SilenceDuration = TimeSpan.FromSeconds(duration);
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (defender != null && attacker != null)
            {
                // Silence the defender, preventing them from casting spells
                defender.SendMessage("You are silenced and cannot cast spells!");
                defender.Squelched = true;

                // Use a timer to remove the silence effect after the duration
                Timer.DelayCall(m_SilenceDuration, delegate() 
                {
                    defender.Squelched = false;
                    defender.SendMessage("You can cast spells again.");
                });

                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_SilenceDuration);
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_SilenceDuration = reader.ReadTimeSpan();
                m_Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Silence Strike: Silences target for " + m_SilenceDuration.TotalSeconds + 
                   " secs. Cooldown of " + Refractory.TotalSeconds + " secs between uses.";
        }
    }
}
