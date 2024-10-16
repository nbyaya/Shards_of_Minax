using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTidalPull : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(40); // Default cooldown for TidalPull
        private DateTime m_NextTidalPull;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlTidalPull(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTidalPull() { }

        [Attachable]
        public XmlTidalPull(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextTidalPull);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextTidalPull = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Tidal Pull: Pulls nearby players toward the caster.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextTidalPull)
            {
                PerformTidalPull(attacker);
                m_NextTidalPull = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformTidalPull(Mobile caster)
        {
            if (caster == null || caster.Map == null)
                return;

            foreach (Mobile m in caster.GetMobilesInRange(5))
            {
                if (m != caster && m.Player)
                {
                    m.Location = caster.Location; // Pulls the target to the caster's location
                    m.SendMessage("You are pulled toward the Abyssal Tide!");
                }
            }
        }
    }
}
