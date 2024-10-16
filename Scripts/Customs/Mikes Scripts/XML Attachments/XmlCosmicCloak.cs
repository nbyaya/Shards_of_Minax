using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCosmicCloak : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown for the ability
        private DateTime m_NextCosmicCloak;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlCosmicCloak(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlCosmicCloak() { }

        [Attachable]
        public XmlCosmicCloak(double cooldown)
        {
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextCosmicCloak);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextCosmicCloak = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Envelops itself in a cloak of starlight, gaining temporary invisibility.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextCosmicCloak)
            {
                PerformCosmicCloak(attacker);
                m_NextCosmicCloak = DateTime.UtcNow + m_Cooldown;
            }
        }

        public void PerformCosmicCloak(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Envelops itself in a cloak of starlight *");
            Effects.SendLocationEffect(owner, owner.Map, 0x376A, 10, 16); // Particle effect

            // Temporary invisibility
            owner.Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => owner.Hidden = false);
        }
    }
}
