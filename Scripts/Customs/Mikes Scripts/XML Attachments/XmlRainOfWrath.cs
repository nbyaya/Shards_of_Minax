using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlRainOfWrath : XmlAttachment
    {
        private TimeSpan m_Interval = TimeSpan.FromMinutes(2);
        private DateTime m_NextRainOfWrath;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Interval { get { return m_Interval; } set { m_Interval = value; } }

        public XmlRainOfWrath(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlRainOfWrath() { }

        [Attachable]
        public XmlRainOfWrath(double interval)
        {
            Interval = TimeSpan.FromMinutes(interval);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Interval);
            writer.WriteDeltaTime(m_NextRainOfWrath);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Interval = reader.ReadTimeSpan();
                m_NextRainOfWrath = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Torrential Rain of Wrath: Deals damage to nearby enemies.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextRainOfWrath)
            {
                RainOfWrath(attacker);
                m_NextRainOfWrath = DateTime.UtcNow + m_Interval;
            }
        }

        private void RainOfWrath(Mobile owner)
        {
            Point3D loc = GetSpawnPosition(5, owner);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, owner.Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 2023);
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A torrential rain of wrath pours down! *");

                foreach (Mobile m in owner.GetMobilesInRange(5))
                {
                    if (m != owner && m.Alive)
                    {
                        m.Damage(Utility.RandomMinMax(5, 10), owner); // Damage dealt to target
                    }
                }
            }
        }

        private Point3D GetSpawnPosition(int range, Mobile owner)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = owner.X + Utility.RandomMinMax(-range, range);
                int y = owner.Y + Utility.RandomMinMax(-range, range);
                int z = owner.Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (owner.Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }
    }
}
