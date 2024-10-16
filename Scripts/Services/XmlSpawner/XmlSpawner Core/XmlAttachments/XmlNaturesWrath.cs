using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlNaturesWrath : XmlAttachment
    {
        private int m_Damage = 0;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5);
        private DateTime m_EndTime;
        private int proximityrange = 2; // 2 tiles around the monster by default

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return proximityrange; } set { proximityrange = value; } }

        public XmlNaturesWrath(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlNaturesWrath(int damage)
        {
            m_Damage = damage;
        }

        [Attachable]
        public XmlNaturesWrath(int damage, double refractory)
        {
            m_Damage = damage;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        [Attachable]
        public XmlNaturesWrath(int damage, double refractory, double expiresin)
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
                foreach (Mobile m in attacker.GetMobilesInRange(proximityrange))
                {
                    if (m != attacker && m.Alive)
                    {
                        int damage = m_Damage > 0 ? Utility.Random(m_Damage) : 0;

                        if (damage > 0)
                        {
                            m.Damage(damage, attacker);
                            m.SendMessage("You are entangled by nature's wrath!");
                            m.Paralyze(TimeSpan.FromSeconds(2)); // Paralyze for 2 seconds
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
                proximityrange = reader.ReadInt();
                m_Damage = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            string msg;

            if (Expiration > TimeSpan.Zero)
            {
                msg = "Nature's Wrath Damage " + m_Damage + " expires in " + Expiration.TotalMinutes + " mins";
            }
            else
            {
                msg = "Nature's Wrath Damage " + m_Damage;
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
