using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCycloneRampage : XmlAttachment
    {
        private int m_MinDamage = 15; // Minimum damage dealt to targets
        private int m_MaxDamage = 25; // Maximum damage dealt to targets
        private int m_DamageMin = 25; // Damage range for rampage
        private int m_DamageMax = 35; // Damage range for rampage
        private int m_VirtualArmorIncrease = 20; // Armor increase during rampage
        private TimeSpan m_RampageDuration = TimeSpan.FromSeconds(10); // Duration of rampage
        private TimeSpan m_NextRampageCooldown = TimeSpan.FromMinutes(1); // Cooldown before rampage can be used again
        private DateTime m_NextRampageTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageMin { get { return m_DamageMin; } set { m_DamageMin = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageMax { get { return m_DamageMax; } set { m_DamageMax = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int VirtualArmorIncrease { get { return m_VirtualArmorIncrease; } set { m_VirtualArmorIncrease = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RampageDuration { get { return m_RampageDuration; } set { m_RampageDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextRampageCooldown { get { return m_NextRampageCooldown; } set { m_NextRampageCooldown = value; } }

        public XmlCycloneRampage(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlCycloneRampage() { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_DamageMin);
            writer.Write(m_DamageMax);
            writer.Write(m_VirtualArmorIncrease);
            writer.Write(m_RampageDuration);
            writer.Write(m_NextRampageCooldown);
            writer.Write(m_NextRampageTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_DamageMin = reader.ReadInt();
                m_DamageMax = reader.ReadInt();
                m_VirtualArmorIncrease = reader.ReadInt();
                m_RampageDuration = reader.ReadTimeSpan();
                m_NextRampageCooldown = reader.ReadTimeSpan();
                m_NextRampageTime = reader.ReadDateTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextRampageTime)
            {
                Rampage(attacker);
            }
        }

        public void Rampage(Mobile owner)
        {
            if (owner == null || !owner.Alive)
                return;

            owner.SendMessage("The creature goes on a rampage!");
            owner.Body = 12; // Change body for rampage effect
            owner.VirtualArmor += m_VirtualArmorIncrease; // Increase armor

            // Start the rampage
            Timer.DelayCall(m_RampageDuration, () =>
            {
                owner.Body = 0; // Revert size (use appropriate body ID)
                owner.VirtualArmor -= m_VirtualArmorIncrease; // Revert armor
                // Note: Reset damage handling in a different way if needed
            });

            IPooledEnumerable nearby = owner.GetMobilesInRange(7);
            foreach (Mobile m in nearby)
            {
                if (m != owner && m.Player)
                {
                    int damage = Utility.RandomMinMax(m_DamageMin, m_DamageMax);
                    m.Damage(damage, owner);
                    m.SendMessage("You are caught in the creature's rampage!");
                    m.PlaySound(0x1F4); // Wind sound
                    m.BoltEffect(0); // Air effect
                }
            }
            nearby.Free();

            m_NextRampageTime = DateTime.UtcNow + m_NextRampageCooldown; // Set next rampage time
        }
    }
}
