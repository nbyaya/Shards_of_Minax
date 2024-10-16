using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSoothingWind : XmlAttachment
    {
        private int m_SpeedIncrease = 20; // Increase in Dexterity
        private TimeSpan m_Duration = TimeSpan.FromSeconds(30); // Duration of the speed increase
        private TimeSpan m_NextWindDelay = TimeSpan.FromSeconds(240); // Minimum delay for next wind cast
        private DateTime m_NextSoothingWind;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SpeedIncrease { get { return m_SpeedIncrease; } set { m_SpeedIncrease = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextWindDelay { get { return m_NextWindDelay; } set { m_NextWindDelay = value; } }

        public XmlSoothingWind(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSoothingWind() { }

        [Attachable]
        public XmlSoothingWind(int speedIncrease, double duration, double nextWindDelay)
        {
            SpeedIncrease = speedIncrease;
            Duration = TimeSpan.FromSeconds(duration);
            NextWindDelay = TimeSpan.FromSeconds(nextWindDelay);
            m_NextSoothingWind = DateTime.UtcNow + NextWindDelay;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_SpeedIncrease);
            writer.Write(m_Duration);
            writer.Write(m_NextWindDelay);
            writer.Write(m_NextSoothingWind);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_SpeedIncrease = reader.ReadInt();
                m_Duration = reader.ReadTimeSpan();
                m_NextWindDelay = reader.ReadTimeSpan();
                m_NextSoothingWind = reader.ReadDateTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextSoothingWind)
            {
                CastSoothingWind(attacker);
                m_NextSoothingWind = DateTime.UtcNow + NextWindDelay;
            }
        }

        private void CastSoothingWind(Mobile caster)
        {
            if (caster == null || !caster.Alive)
                return;

            foreach (Mobile m in caster.GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).Team == ((BaseCreature)caster).Team)
                {
                    // Increase speed by modifying Dexterity temporarily
                    ((BaseCreature)m).SetDex(((BaseCreature)m).Dex + SpeedIncrease);
                    m.SendMessage("The soothing wind increases your speed!");
                }
            }

            Timer.DelayCall(Duration, new TimerCallback(() => 
            {
                foreach (Mobile m in caster.GetMobilesInRange(10))
                {
                    if (m is BaseCreature && ((BaseCreature)m).Team == ((BaseCreature)caster).Team)
                    {
                        // Revert Dexterity to original value
                        ((BaseCreature)m).SetDex(((BaseCreature)m).Dex - SpeedIncrease);
                    }
                }
            }));
        }
    }
}
