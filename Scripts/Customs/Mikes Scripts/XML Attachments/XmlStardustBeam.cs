using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlStardustBeam : XmlAttachment
    {
        private int m_Damage = 30; // Default damage for the stardust beam
        private TimeSpan m_FreezeDuration = TimeSpan.FromSeconds(2); // Duration of the stun effect
        private TimeSpan m_Refractory = TimeSpan.FromMinutes(2); // Time between activations
        private DateTime m_NextStardustBeam;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan FreezeDuration { get { return m_FreezeDuration; } set { m_FreezeDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlStardustBeam(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlStardustBeam() { }

        [Attachable]
        public XmlStardustBeam(int damage, double freezeDuration, double refractory)
        {
            Damage = damage;
            FreezeDuration = TimeSpan.FromSeconds(freezeDuration);
            Refractory = TimeSpan.FromMinutes(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_FreezeDuration);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextStardustBeam);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_FreezeDuration = reader.ReadTimeSpan();
                m_Refractory = reader.ReadTimeSpan();
                m_NextStardustBeam = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextStardustBeam)
            {
                PerformStardustBeam(defender);
                m_NextStardustBeam = DateTime.UtcNow + m_Refractory;
            }
        }

        private void PerformStardustBeam(Mobile target)
        {
            if (target == null || target.Map == null)
                return;

            Effects.SendLocationEffect(target.Location, target.Map, 0x373A, 10, 16); // Beam effect

            target.SendMessage("You are struck by a powerful beam of stardust!");
            target.Damage(m_Damage); // Pass the attacker (Mobile)
            target.Freeze(m_FreezeDuration); // Stun effect
        }
    }
}
