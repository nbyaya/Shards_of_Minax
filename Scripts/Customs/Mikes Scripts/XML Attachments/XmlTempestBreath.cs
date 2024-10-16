using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTempestBreath : XmlAttachment
    {
        private int m_MinDamage = 20; // Minimum damage
        private int m_MaxDamage = 30; // Maximum damage
        private TimeSpan m_NextBreath = TimeSpan.FromSeconds(30); // Cooldown time
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextBreath { get { return m_NextBreath; } set { m_NextBreath = value; } }

        public XmlTempestBreath(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTempestBreath() { }

        [Attachable]
        public XmlTempestBreath(int minDamage, int maxDamage, double nextBreath)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            NextBreath = TimeSpan.FromSeconds(nextBreath);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_NextBreath);
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_NextBreath = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Tempest Breath Attack: {0}-{1} damage every {2} seconds.", m_MinDamage, m_MaxDamage, m_NextBreath.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_EndTime)
            {
                PerformTempestBreath(attacker);
                m_EndTime = DateTime.UtcNow + NextBreath;
            }
        }

        public void PerformTempestBreath(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            IPooledEnumerable nearby = owner.GetMobilesInRange(5);
            foreach (Mobile m in nearby)
            {
                if (m != owner && m.Player && m.InLOS(owner))
                {
                    int damage = Utility.RandomMinMax(MinDamage, MaxDamage);
                    m.Damage(damage, owner);
                    m.SendMessage("You are blasted by a tempest breath!");
                    m.PlaySound(0x1F4); // Wind sound
                    m.BoltEffect(0); // Air effect
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z); // Knock back
                }
            }
            nearby.Free();
        }
    }
}
