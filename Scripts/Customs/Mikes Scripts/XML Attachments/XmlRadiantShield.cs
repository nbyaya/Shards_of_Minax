using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlRadiantShield : XmlAttachment
    {
        private bool m_HasRadiantShield = false; // To track if the shield is active
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(2); // Cooldown time
        private DateTime m_NextRadiantShield;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlRadiantShield(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlRadiantShield() { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HasRadiantShield);
            writer.Write(m_Cooldown);
            writer.Write(m_NextRadiantShield);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 0)
            {
                m_HasRadiantShield = reader.ReadBool();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextRadiantShield = reader.ReadDateTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextRadiantShield)
            {
                ActivateRadiantShield(attacker);
            }
        }

        private void ActivateRadiantShield(Mobile owner)
        {
            if (m_HasRadiantShield)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiant Shield Activated! *");
            owner.FixedEffect(0x373A, 10, 16); // Shield effect

            m_HasRadiantShield = true;
            owner.VirtualArmor += 30; // Increase defense

            Timer.DelayCall(TimeSpan.FromSeconds(20), () => 
            {
                DeactivateRadiantShield(owner);
            });

            m_NextRadiantShield = DateTime.UtcNow + m_Cooldown; // Set cooldown for the shield
        }

        private void DeactivateRadiantShield(Mobile owner)
        {
            m_HasRadiantShield = false;
            owner.VirtualArmor -= 30; // Revert defense
            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiant Shield Deactivated! *");
        }
    }
}
