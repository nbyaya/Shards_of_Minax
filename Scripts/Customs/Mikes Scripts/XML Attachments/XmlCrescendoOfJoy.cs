using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCrescendoOfJoy : XmlAttachment
    {
        private int m_Damage = 20; // Damage dealt by the ability
        private TimeSpan m_FreezeDuration = TimeSpan.FromSeconds(4); // Duration of the freeze effect
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown period
        private DateTime m_NextCrescendoOfJoy;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan FreezeDuration { get { return m_FreezeDuration; } set { m_FreezeDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlCrescendoOfJoy(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlCrescendoOfJoy() { }

        [Attachable]
        public XmlCrescendoOfJoy(int damage, double freezeDuration, double cooldown)
        {
            Damage = damage;
            FreezeDuration = TimeSpan.FromSeconds(freezeDuration);
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_FreezeDuration);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextCrescendoOfJoy);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_FreezeDuration = reader.ReadTimeSpan();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextCrescendoOfJoy = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Crescendo of Joy: Deals {0} damage and freezes enemies for {1} seconds every {2} minutes.", m_Damage, m_FreezeDuration.TotalSeconds, m_Cooldown.TotalMinutes);
        }

        public void PerformCrescendo(Mobile owner)
        {
            if (owner == null || DateTime.UtcNow < m_NextCrescendoOfJoy || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a powerful crescendo of joy *");
            owner.PlaySound(0x1F6); // Musical burst sound

            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m != owner)
                {
                    m.Damage(m_Damage, owner); // Damage dealt by the ability
                    m.SendMessage("You are struck by a burst of musical energy!");
                    m.SendMessage("You feel your movement slowed down!");
                    m.Freeze(m_FreezeDuration); // Slow down effect
                }
            }

            m_NextCrescendoOfJoy = DateTime.UtcNow + m_Cooldown; // Set cooldown
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            PerformCrescendo(attacker); // Trigger the ability on weapon hit
        }
    }
}
