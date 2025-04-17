using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPoisonAppleThrow : XmlAttachment
    {
        private int m_Damage = 20;
        private int m_Range = 8;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10);
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlPoisonAppleThrow(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlPoisonAppleThrow() { }

        [Attachable]
        public XmlPoisonAppleThrow(double refractory, int damage, int range)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
            Damage = damage;
            Range = range;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Damage);
            writer.Write(m_Range);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Range = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Poison Apple Throw: {0} damage over {1} tiles every {2} seconds.", m_Damage, m_Range, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_EndTime)
            {
                PerformPoisonAppleThrow(attacker);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public void PerformPoisonAppleThrow(Mobile owner)
        {
            if (owner == null || DateTime.Now < m_EndTime || owner.Map == null)
                return;

            Map map = owner.Map;
            Mobile target = owner.Combatant as Mobile;

            if (target != null && target.Map == owner.Map && target.InRange(owner, m_Range))
            {
                owner.Say("How do you like them apples?!");

                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 10, 10, 0, 0); // Green flamestrike
                Effects.PlaySound(target.Location, target.Map, 0x208); // Explosion sound

                foreach (Mobile m in owner.GetMobilesInRange(2))
                {
                    if (m == owner || !owner.CanBeHarmful(m, false)) continue;

                    owner.DoHarmful(m);
                    AOS.Damage(m, owner, Utility.RandomMinMax(m_Damage, m_Damage * 2), 0, 0, 0, 100, 0);
                }

                for (int i = 0; i < 5; i++)
                {
                    Point3D dropPoint = new Point3D(target.X + Utility.RandomMinMax(-1, 1), target.Y + Utility.RandomMinMax(-1, 1), target.Z);
                    if (map.CanFit(dropPoint, 0, true, false))
                    {
                        Effects.SendLocationEffect(dropPoint, map, 0x3709, 10, 10, 0, 0); // Green flamestrike
                        Item poisonTile = new PoisonTile();
                        poisonTile.MoveToWorld(dropPoint, map);
                    }
                }
            }

            m_EndTime = DateTime.Now + m_Refractory;
        }
    }
}
