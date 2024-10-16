using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFrostyTrail : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(35); // Cooldown for FrostyTrail
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlFrostyTrail(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFrostyTrail() { }

        [Attachable]
        public XmlFrostyTrail(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_EndTime)
            {
                PerformFrostyTrail(attacker);
                m_EndTime = DateTime.UtcNow + Refractory;
            }
        }

        public void PerformFrostyTrail(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Leaves a frosty trail *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16); // Frosty trail effect

            foreach (Mobile m in owner.GetMobilesInRange(2))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("You feel a chill as you walk through the frosty trail!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Freezing the player
                }
            }
        }
    }
}
