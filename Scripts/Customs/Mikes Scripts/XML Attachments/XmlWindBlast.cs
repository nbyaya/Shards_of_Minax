using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlWindBlast : XmlAttachment
    {
        private int m_DamageMin = 10; // Minimum damage
        private int m_DamageMax = 15; // Maximum damage
        private int m_Range = 3; // Range for area damage
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(4); // Time between activations
        private DateTime m_NextWindBlast;

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageMin { get { return m_DamageMin; } set { m_DamageMin = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageMax { get { return m_DamageMax; } set { m_DamageMax = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlWindBlast(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlWindBlast() { }

        [Attachable]
        public XmlWindBlast(int damageMin, int damageMax, int range, double refractory)
        {
            DamageMin = damageMin;
            DamageMax = damageMax;
            Range = range;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_DamageMin);
            writer.Write(m_DamageMax);
            writer.Write(m_Range);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextWindBlast);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_DamageMin = reader.ReadInt();
                m_DamageMax = reader.ReadInt();
                m_Range = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_NextWindBlast = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextWindBlast)
            {
                PerformWindBlast(attacker);
                m_NextWindBlast = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformWindBlast(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile target in owner.GetMobilesInRange(m_Range))
            {
                if (target != owner && target.Alive)
                {
                    int damage = Utility.RandomMinMax(m_DamageMin, m_DamageMax);
                    target.Damage(damage, owner);
                    target.SendMessage("You are hit by a powerful gust of wind!");

                    // Chance to knock back
                    int chance = Utility.Random(100);
                    if (chance < 30) // 30% chance to knock back
                    {
                        Effects.SendMovingEffect(target, owner, 0x36D4, 16, 1, false, true);
                        target.Location = new Point3D(target.X + Utility.RandomMinMax(-2, 2), 
                                                       target.Y + Utility.RandomMinMax(-2, 2), 
                                                       target.Z);
                        target.SendMessage("You are knocked back by the gale!");
                    }
                }
            }
        }
    }
}
