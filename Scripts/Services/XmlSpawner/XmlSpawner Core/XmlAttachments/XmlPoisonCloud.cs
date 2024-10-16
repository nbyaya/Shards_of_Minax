using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPoisonCloud : XmlAttachment
    {
        private int m_PoisonIntensity = 1; // Default intensity, can be scaled to various poison levels.
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10); // Time between activations
        private DateTime m_EndTime;
        private int proximityrange = 2; // Affects targets within 2 tiles by default.

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonIntensity { get { return m_PoisonIntensity; } set { m_PoisonIntensity = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return proximityrange; } set { proximityrange = value; } }

        public XmlPoisonCloud(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlPoisonCloud(int intensity)
        {
            m_PoisonIntensity = intensity;
        }

        [Attachable]
        public XmlPoisonCloud(int intensity, double refractory)
        {
            m_PoisonIntensity = intensity;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        [Attachable]
        public XmlPoisonCloud(int intensity, double refractory, double expiresin)
        {
            m_PoisonIntensity = intensity;
            Expiration = TimeSpan.FromMinutes(expiresin);
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (attacker != null)
            {
                foreach (Mobile m in attacker.GetMobilesInRange(proximityrange))
                {
                    if (m != attacker && m.Alive)
                    {
                        // Apply poison based on intensity.
                        switch (m_PoisonIntensity)
                        {
                            case 1: m.ApplyPoison(attacker, Poison.Lesser); break;
                            case 2: m.ApplyPoison(attacker, Poison.Regular); break;
                            case 3: m.ApplyPoison(attacker, Poison.Greater); break;
                            case 4: m.ApplyPoison(attacker, Poison.Deadly); break;
                            case 5: m.ApplyPoison(attacker, Poison.Lethal); break;
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
            writer.Write(proximityrange);
            writer.Write(m_PoisonIntensity);
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 0)
            {
                proximityrange = reader.ReadInt();
                m_PoisonIntensity = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            string msg = "Poison Cloud Intensity " + m_PoisonIntensity;

            if (Expiration > TimeSpan.Zero)
            {
                msg += " expires in " + Expiration.TotalMinutes + " mins";
            }

            if (Refractory > TimeSpan.Zero)
            {
                return msg + " - " + Refractory.TotalSeconds + " secs between uses";
            }
            else
            {
                return msg;
            }
        }
    }
}
