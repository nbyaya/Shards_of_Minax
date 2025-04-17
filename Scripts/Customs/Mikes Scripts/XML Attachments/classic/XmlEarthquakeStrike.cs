using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlEarthquakeStrike : XmlAttachment
    {
        private int m_Damage = 0;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5);
        private DateTime m_EndTime;
        private int m_Range = 3; // Affects targets within 3 tiles by default

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        public XmlEarthquakeStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlEarthquakeStrike(int damage)
        {
            m_Damage = damage;
        }

        [Attachable]
        public XmlEarthquakeStrike(int damage, double refractory)
        {
            m_Damage = damage;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        [Attachable]
        public XmlEarthquakeStrike(int damage, double refractory, double expiresin)
        {
            m_Damage = damage;
            Expiration = TimeSpan.FromMinutes(expiresin);
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (attacker != null)
            {
                foreach (Mobile m in attacker.GetMobilesInRange(m_Range))
                {
                    if (m != attacker && m.Alive)
                    {
                        int damage = m_Damage > 0 ? Utility.Random(m_Damage) : 0;

                        if (damage > 0)
                        {
                            m.Damage(damage, attacker);
                            m.SendMessage("The ground shakes violently!");
                            
                            // Optionally, you can apply a movement speed penalty here for a "knockdown" effect.
                            // m.Freeze(TimeSpan.FromSeconds(1)); // Freezes the target for 1 second
                        }
                    }
                }

                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Range);
            writer.Write(m_Damage);
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Range = reader.ReadInt();
                m_Damage = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            string msg = "Earthquake Damage " + m_Damage;

            if (Expiration > TimeSpan.Zero)
            {
                msg += " expires in " + Expiration.TotalMinutes + " mins";
            }

            if (Refractory > TimeSpan.Zero)
            {
                msg += " - " + Refractory.TotalSeconds + " secs between uses";
            }

            return msg;
        }
    }
}
