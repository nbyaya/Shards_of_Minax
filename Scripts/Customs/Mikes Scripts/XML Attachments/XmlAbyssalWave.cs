using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAbyssalWave : XmlAttachment
    {
        private int m_MinDamage = 15; // Minimum damage for the wave attack
        private int m_MaxDamage = 25; // Maximum damage for the wave attack
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown for the wave ability
        private DateTime m_NextAbyssalWave;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlAbyssalWave(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlAbyssalWave() { }

        [Attachable]
        public XmlAbyssalWave(int minDamage, int maxDamage, double cooldown)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextAbyssalWave);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextAbyssalWave = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextAbyssalWave)
            {
                PerformAbyssalWave(attacker);
                m_NextAbyssalWave = DateTime.UtcNow + Cooldown;
            }
        }

        public void PerformAbyssalWave(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Player)
                {
                    int damage = Utility.RandomMinMax(MinDamage, MaxDamage);
                    m.Damage(damage, owner); // Pass the owner as the source of damage
                    m.SendMessage("You are struck by a wave of dark water!");
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);
                }
            }
        }
    }
}
