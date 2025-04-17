using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGrasp : XmlAttachment
    {
        private double m_Chance = 10.0; // 10% chance to trigger
        private TimeSpan m_ParalyzeDuration = TimeSpan.FromSeconds(6); // Duration of paralysis

        [CommandProperty(AccessLevel.GameMaster)]
        public double Chance { get { return m_Chance; } set { m_Chance = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan ParalyzeDuration { get { return m_ParalyzeDuration; } set { m_ParalyzeDuration = value; } }

        public XmlGrasp(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGrasp() { }

        [Attachable]
        public XmlGrasp(double chance, double duration)
        {
            Chance = chance;
            ParalyzeDuration = TimeSpan.FromSeconds(duration);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Chance);
            writer.Write(m_ParalyzeDuration);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Chance = reader.ReadDouble();
                m_ParalyzeDuration = reader.ReadTimeSpan();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Grasp: {0}% chance to paralyze for {1} seconds on melee hit.", m_Chance, m_ParalyzeDuration.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            // Only proceed if it's a melee hit
            if (weapon is BaseMeleeWeapon)
            {
                if (Utility.RandomDouble() < m_Chance / 100)
                {
                    defender.Paralyze(m_ParalyzeDuration);
                    defender.SendMessage("You've been grasped and cannot move!");
                    attacker.SendMessage("You've paralyzed your target!");
					defender.Say("Grasped!");
                }
            }
        }
    }
}
