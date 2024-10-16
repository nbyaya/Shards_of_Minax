using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;


namespace Server.Engines.XmlSpawner2
{
    public class XmlEvasiveManeuver : XmlAttachment
    {
        private int m_DexIncrease = 20; // Dexterity increase for evasion
        private TimeSpan m_Refractory = TimeSpan.FromMinutes(1); // Time between maneuvers
        private DateTime m_NextEvasiveManeuver;

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexIncrease { get { return m_DexIncrease; } set { m_DexIncrease = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlEvasiveManeuver(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlEvasiveManeuver() { }

        [Attachable]
        public XmlEvasiveManeuver(int dexIncrease, double refractory)
        {
            DexIncrease = dexIncrease;
            Refractory = TimeSpan.FromMinutes(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_DexIncrease);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextEvasiveManeuver);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_DexIncrease = reader.ReadInt();
                m_Refractory = reader.ReadTimeSpan();
                m_NextEvasiveManeuver = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextEvasiveManeuver)
            {
                PerformEvasiveManeuver(attacker);
                m_NextEvasiveManeuver = DateTime.UtcNow + m_Refractory;
            }
        }

        private void PerformEvasiveManeuver(Mobile owner)
        {
            if (owner == null) return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Performs an evasive maneuver! *");
            owner.Dex += m_DexIncrease; // Temporarily increase dexterity for better evasion
            
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                if (owner != null) 
                {
                    owner.Dex -= m_DexIncrease; // Revert dexterity after 10 seconds
                }
            });
        }
    }
}
