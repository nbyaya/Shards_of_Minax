using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlStarShower : XmlAttachment
    {
        private int m_Damage = 10; // Default damage for the star shower attack
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(20); // Fixed cooldown
        private DateTime m_NextStarShower;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlStarShower(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlStarShower() { }

        [Attachable]
        public XmlStarShower(int damage, double cooldown)
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
            writer.WriteDeltaTime(m_NextStarShower);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextStarShower = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Star Shower Attack: {0} damage with a cooldown of {1} seconds.", m_Damage, m_Cooldown.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextStarShower)
            {
                PerformStarShower(attacker);
                m_NextStarShower = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformStarShower(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons a shower of stars *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16); // Particle effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Alive)
                {
                    m.SendMessage("The star shower dazzles you!");
                    m.Damage(m_Damage, owner); // Pass the attacker as the source of damage

                    // Chance to stun
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.Freeze(TimeSpan.FromSeconds(3));
                    }
                }
            }
        }
    }
}
