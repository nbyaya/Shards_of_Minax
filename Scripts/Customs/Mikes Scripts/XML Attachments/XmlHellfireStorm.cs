using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlHellfireStorm : XmlAttachment
    {
        private int m_MinDamage = 20; // Minimum damage for the hellfire storm
        private int m_MaxDamage = 40; // Maximum damage for the hellfire storm
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown for the ability
        private DateTime m_NextHellfireStorm;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlHellfireStorm(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlHellfireStorm() { }

        [Attachable]
        public XmlHellfireStorm(int minDamage, int maxDamage, double cooldown)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextHellfireStorm);
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
                m_NextHellfireStorm = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextHellfireStorm)
            {
                PerformHellfireStorm(attacker);
                m_NextHellfireStorm = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformHellfireStorm(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("You are engulfed in a storm of hellfire!");
                    AOS.Damage(m, owner, Utility.RandomMinMax(m_MinDamage, m_MaxDamage), 0, 100, 0, 0, 0); // Fire damage
                }
            }

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A storm of hellfire is unleashed! *");
        }
    }
}
