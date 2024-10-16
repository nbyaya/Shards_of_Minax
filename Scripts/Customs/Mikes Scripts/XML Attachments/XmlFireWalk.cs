using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFireWalk : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1);
        private DateTime m_NextFireWalk;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlFireWalk(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFireWalk() { }

        [Attachable]
        public XmlFireWalk(double cooldown)
        {
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextFireWalk);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextFireWalk = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextFireWalk)
            {
                PerformFireWalk(attacker);
                m_NextFireWalk = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformFireWalk(Mobile owner)
        {
            Point3D loc = GetSpawnPosition(5, owner);

            if (loc != Point3D.Zero)
            {
                owner.Location = loc;
                owner.Map = owner.Map;

                Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* You teleport with a burst of flames! *");

                foreach (Mobile m in owner.GetMobilesInRange(3))
                {
                    if (m != owner && m.Alive)
                    {
                        m.SendMessage("You are scorched by the flames left behind!");
                        m.Damage(Utility.RandomMinMax(10, 20), owner);
                    }
                }
            }
        }

        private Point3D GetSpawnPosition(int range, Mobile owner)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = owner.Location.X + Utility.RandomMinMax(-range, range);
                int y = owner.Location.Y + Utility.RandomMinMax(-range, range);
                int z = owner.Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (owner.Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }
    }
}
