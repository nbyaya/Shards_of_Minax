using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGustBarrier : XmlAttachment
    {
        private int m_VirtualArmorBoost = 20; // Armor boost for the gust barrier
        private TimeSpan m_Duration = TimeSpan.FromMinutes(1); // Duration of the barrier effect
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(180); // Minimum time before next barrier activation
        private DateTime m_NextGustBarrier;

        [CommandProperty(AccessLevel.GameMaster)]
        public int VirtualArmorBoost { get { return m_VirtualArmorBoost; } set { m_VirtualArmorBoost = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlGustBarrier(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGustBarrier() { }

        [Attachable]
        public XmlGustBarrier(int virtualArmorBoost, double duration, double refractory)
        {
            VirtualArmorBoost = virtualArmorBoost;
            Duration = TimeSpan.FromMinutes(duration);
            Refractory = TimeSpan.FromSeconds(refractory);
            m_NextGustBarrier = DateTime.UtcNow; // Initialize the next barrier time
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_VirtualArmorBoost);
            writer.Write(m_Duration);
            writer.Write(m_Refractory);
            writer.Write(m_NextGustBarrier);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_VirtualArmorBoost = reader.ReadInt();
                m_Duration = reader.ReadTimeSpan();
                m_Refractory = reader.ReadTimeSpan();
                m_NextGustBarrier = reader.ReadDateTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextGustBarrier)
            {
                ActivateGustBarrier(attacker);
                Random rand = new Random();
                m_NextGustBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(180, 240));
            }
        }

        private void ActivateGustBarrier(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).Team == ((BaseCreature)owner).Team)
                {
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                    m.SendMessage("A gust barrier surrounds you, reducing incoming damage!");

                    ((BaseCreature)m).VirtualArmor += m_VirtualArmorBoost;

                    // Timer to remove the armor boost after the duration
                    Timer.DelayCall(m_Duration, () =>
                    {
                        if (m is BaseCreature bc && bc.Team == ((BaseCreature)owner).Team)
                        {
                            bc.VirtualArmor -= m_VirtualArmorBoost;
                        }
                    });
                }
            }
        }
    }
}
