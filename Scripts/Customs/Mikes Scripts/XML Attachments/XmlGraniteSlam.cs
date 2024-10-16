using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGraniteSlam : XmlAttachment
    {
        private int m_MinDamage = 50; // Minimum damage for the slam
        private int m_MaxDamage = 70; // Maximum damage for the slam
        private TimeSpan m_NextSlamTime = TimeSpan.FromSeconds(30); // Time between slams
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextSlamTime { get { return m_NextSlamTime; } set { m_NextSlamTime = value; } }

        public XmlGraniteSlam(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGraniteSlam() { }

        [Attachable]
        public XmlGraniteSlam(int minDamage, int maxDamage, double refractory)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            NextSlamTime = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_NextSlamTime);
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
                m_NextSlamTime = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_EndTime)
            {
                PerformGraniteSlam(attacker);
                Random rand = new Random();
                m_EndTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
            }
        }

        private void PerformGraniteSlam(Mobile owner)
        {
            if (owner.Combatant != null)
            {
                Mobile target = owner.Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
                    target.Damage(damage, owner);

                    // Knock back effect
                    if (Utility.RandomBool())
                    {
                        Point3D targetLoc = target.Location;
                        target.Location = new Point3D(targetLoc.X + Utility.RandomMinMax(-2, 2), targetLoc.Y + Utility.RandomMinMax(-2, 2), targetLoc.Z);
                        target.SendMessage("You are knocked back by the slam!");
                    }

                    owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A tremendous force slams the ground! *");
                }
            }
        }
    }
}
