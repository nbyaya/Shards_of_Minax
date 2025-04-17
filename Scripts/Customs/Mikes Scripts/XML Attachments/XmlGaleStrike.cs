using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGaleStrike : XmlAttachment
    {
        private int m_MinDamage = 15; // Minimum damage
        private int m_MaxDamage = 20; // Maximum damage
        private TimeSpan m_Interval = TimeSpan.FromSeconds(20); // Time between activations
        private DateTime m_NextGaleStrike;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Interval { get { return m_Interval; } set { m_Interval = value; } }

        public XmlGaleStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGaleStrike() { }

        [Attachable]
        public XmlGaleStrike(int minDamage, int maxDamage, double interval)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Interval = TimeSpan.FromSeconds(interval);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Interval);
            writer.WriteDeltaTime(m_NextGaleStrike);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_Interval = reader.ReadTimeSpan();
                m_NextGaleStrike = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (attacker != null && defender != null && DateTime.UtcNow >= m_NextGaleStrike)
            {
                PerformGaleStrike(defender);
                m_NextGaleStrike = DateTime.UtcNow + m_Interval;
            }
        }

        public void PerformGaleStrike(Mobile target)
        {
            if (target != null || !target.Alive)
                return;

            // Use target.Map instead of Map
            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
            target.SendMessage("You are struck by a powerful gale!");

            // Change 'this' to 'target' to specify who is dealing the damage
            target.Damage(Utility.RandomMinMax(m_MinDamage, m_MaxDamage), target); 

            // Knock back effect
            Point3D newLocation = target.Location;
            newLocation.X += Utility.RandomMinMax(-2, 2);
            newLocation.Y += Utility.RandomMinMax(-2, 2);

            // Use target.Map to check for spawnable location
            if (target.Map.CanSpawnMobile(newLocation))
                target.Location = newLocation;
        }
    }
}
