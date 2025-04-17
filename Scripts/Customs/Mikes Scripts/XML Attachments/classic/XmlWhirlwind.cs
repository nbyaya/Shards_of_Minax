using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlWhirlwind : XmlAttachment
    {
        private int m_Damage = 0;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5); // 5 seconds default time between activations
        private DateTime m_EndTime;
        private int proximityrange = 1; // default range of 1 tile around the monster

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return proximityrange; } set { proximityrange = value; } }

        public XmlWhirlwind(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlWhirlwind(int damage)
        {
            m_Damage = damage;
        }

        [Attachable]
        public XmlWhirlwind(int damage, double refractory)
        {
            m_Damage = damage;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        [Attachable]
        public XmlWhirlwind(int damage, double refractory, double expiresin)
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
                msg = "Whirlwind Damage " + m_Damage + " expires in " + Expiration.TotalMinutes + " mins";
            }
            else
            {
                msg = "Whirlwind Damage " + m_Damage;
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
