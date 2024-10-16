using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGravityWarp : XmlAttachment
    {
        private int m_Damage = 15; // Default damage
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(45); // Cooldown for the ability
        private DateTime m_NextGravityWarp;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlGravityWarp(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGravityWarp() { }

        [Attachable]
        public XmlGravityWarp(int damage, double cooldown)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextGravityWarp);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextGravityWarp = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Gravity Warp: {0} damage with a cooldown of {1} seconds.", m_Damage, m_Cooldown.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextGravityWarp)
            {
                PerformGravityWarp(attacker);
                m_NextGravityWarp = DateTime.UtcNow + m_Cooldown;
            }
        }

        public void PerformGravityWarp(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Warps the gravity around you *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16); // Particle effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Alive)
                {
                    m.SendMessage("The gravity warp throws you off balance!");
                    m.Damage(m_Damage, owner);

                    // Chance to slow
                    if (Utility.RandomDouble() < 0.25)
                    {
                        m.Dex -= 10;
                        if (m.Dex < 1) m.Dex = 1;
                    }
                }
            }
        }
    }
}
