using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPhantomStrike : XmlAttachment
    {
        private int m_Damage = 25; // Default damage for the phantom strike
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlPhantomStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlPhantomStrike() { }

        [Attachable]
        public XmlPhantomStrike(int damage, double refractory)
        {
            Damage = damage;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public void ExecutePhantomStrike(Mobile attacker)
        {
            if (DateTime.Now >= m_EndTime && attacker.Combatant is Mobile target)
            {
                Point3D behindLocation = new Point3D(target.X + Utility.RandomMinMax(-1, 1), target.Y + Utility.RandomMinMax(-1, 1), target.Z);
                
                if (target.Map.CanSpawnMobile(behindLocation))
                {
                    attacker.Location = behindLocation;
                    target.SendMessage("You feel a sudden chill as something strikes from behind!");
                    target.Damage(Utility.RandomMinMax(20, m_Damage), attacker);
                    m_EndTime = DateTime.Now + m_Refractory; // Recalculate next activation
                }
            }
        }
    }
}
