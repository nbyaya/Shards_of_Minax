using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGlitterShield : XmlAttachment
    {
        private int m_VirtualArmor = 20; // Armor value to add
        private TimeSpan m_Duration = TimeSpan.FromSeconds(10); // Duration of the shield
        private TimeSpan m_ReuseDelay = TimeSpan.FromMinutes(1); // Time before it can be used again
        private DateTime m_NextGlitterShield;
        private bool m_HasShield;

        [CommandProperty(AccessLevel.GameMaster)]
        public int VirtualArmor { get { return m_VirtualArmor; } set { m_VirtualArmor = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan ReuseDelay { get { return m_ReuseDelay; } set { m_ReuseDelay = value; } }

        public XmlGlitterShield(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGlitterShield() { }

        [Attachable]
        public XmlGlitterShield(double duration, double reuseDelay, int virtualArmor)
        {
            Duration = TimeSpan.FromSeconds(duration);
            ReuseDelay = TimeSpan.FromMinutes(reuseDelay);
            VirtualArmor = virtualArmor;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_VirtualArmor);
            writer.Write(m_Duration);
            writer.Write(m_ReuseDelay);
            writer.Write(m_NextGlitterShield);
            writer.Write(m_HasShield);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_VirtualArmor = reader.ReadInt();
                m_Duration = reader.ReadTimeSpan();
                m_ReuseDelay = reader.ReadTimeSpan();
                m_NextGlitterShield = reader.ReadDateTime();
                m_HasShield = reader.ReadBool();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextGlitterShield && !m_HasShield)
            {
                ActivateShield(attacker);
            }
        }

        private void ActivateShield(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A shimmering shield surrounds the creature! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x373A, 10, 16); // Shield effect

            owner.VirtualArmor += m_VirtualArmor;
            m_HasShield = true;
            m_NextGlitterShield = DateTime.UtcNow + m_ReuseDelay;

            Timer.DelayCall(m_Duration, () => RemoveShield(owner));
        }

        private void RemoveShield(Mobile owner)
        {
            if (owner != null)
            {
                owner.VirtualArmor -= m_VirtualArmor;
                m_HasShield = false;
            }
        }
    }
}
