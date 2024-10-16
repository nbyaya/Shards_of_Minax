using System;
using Server;
using Server.Mobiles;
using Server.Items; // Make sure this line is added

namespace Server.Engines.XmlSpawner2
{
    public class XmlWebCooldown : XmlAttachment
    {
        private TimeSpan m_CooldownDuration = TimeSpan.FromSeconds(3); // Paralyze duration
        private TimeSpan m_Cooldown = TimeSpan.Zero; // Cooldown time, starts at zero
        private DateTime m_NextPossibleUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan CooldownDuration { get { return m_CooldownDuration; } set { m_CooldownDuration = value; } }

        public XmlWebCooldown(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlWebCooldown()
        {
        }

        [Attachable]
        public XmlWebCooldown(double cooldownDuration)
        {
            CooldownDuration = TimeSpan.FromSeconds(cooldownDuration);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_CooldownDuration);
            writer.WriteDeltaTime(m_NextPossibleUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_CooldownDuration = reader.ReadTimeSpan();
                m_NextPossibleUse = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Web Cooldown: Paralyzes for {0} seconds on hit with a 5% chance.", m_CooldownDuration.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_NextPossibleUse)
                return; // Still cooling down, no effect

            // 5% chance to activate
            if (Utility.RandomDouble() <= 0.05)
            {
                defender.Paralyze(m_CooldownDuration);
				defender.Say("Caught in a web!");
                m_NextPossibleUse = DateTime.Now + m_Cooldown; // Set the next possible use to now + cooldown duration
                // Visual or sound effect to indicate activation
                Effects.SendLocationEffect(defender.Location, defender.Map, 0x376A, 10, 10, 0, 0); // Example effect, adjust as needed
            }
        }
    }
}
