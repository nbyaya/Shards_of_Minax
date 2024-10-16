using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGroundSlam : XmlAttachment
    {
        private int m_MinDamage = 20; // Minimum damage
        private int m_MaxDamage = 30; // Maximum damage
        private int m_Radius = 4; // Radius of the slam attack
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(20); // Cooldown between ground slams
        private DateTime m_NextGroundSlam;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Radius { get { return m_Radius; } set { m_Radius = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlGroundSlam(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGroundSlam() { }

        [Attachable]
        public XmlGroundSlam(int minDamage, int maxDamage, int radius, double cooldown)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Radius = radius;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Radius);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextGroundSlam);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_Radius = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextGroundSlam = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextGroundSlam)
            {
                PerformGroundSlam(attacker);
                m_NextGroundSlam = DateTime.UtcNow + m_Cooldown;
            }
        }

        public override void OnUse(Mobile from)
        {
            base.OnUse(from);

            if (DateTime.UtcNow >= m_NextGroundSlam)
            {
                PerformGroundSlam(from);
                m_NextGroundSlam = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformGroundSlam(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*Slams the ground with tremendous force*");
            owner.PlaySound(0x2F3);
            owner.FixedEffect(0x3789, 10, 20);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = owner.GetMobilesInRange(m_Radius);

            foreach (Mobile m in eable)
            {
                if (m != owner && owner.CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                owner.DoHarmful(m);
                AOS.Damage(m, owner, Utility.RandomMinMax(m_MinDamage, m_MaxDamage), 100, 0, 0, 0, 0);
                m.SendMessage("The ground beneath you shakes and trembles!");

                Point3D knockBackPoint = GetKnockBackPoint(owner, m, 2);
                m.MovingEffect(m, 0x11B6, 5, 0, false, false, 0, 0);
                m.MoveToWorld(knockBackPoint, owner.Map);
            }
        }

        private Point3D GetKnockBackPoint(Mobile owner, Mobile target, int distance)
        {
            Point3D p = target.Location;

            for (int i = 0; i < distance; i++)
            {
                int x = p.X, y = p.Y;
                Movement.Movement.Offset(owner.GetDirectionTo(target.Location), ref x, ref y);
                p = new Point3D(x, y, owner.Map.GetAverageZ(x, y));
            }

            return p;
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Ground Slam Attack: {0}-{1} damage within {2} tile radius every {3} seconds.", m_MinDamage, m_MaxDamage, m_Radius, m_Cooldown.TotalSeconds);
        }
    }
}
