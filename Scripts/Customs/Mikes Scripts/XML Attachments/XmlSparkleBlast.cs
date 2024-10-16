using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSparkleBlast : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextSparkleBlast;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlSparkleBlast(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSparkleBlast() { }

        [Attachable]
        public XmlSparkleBlast(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextSparkleBlast);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Refractory = reader.ReadTimeSpan();
                m_NextSparkleBlast = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextSparkleBlast)
            {
                PerformSparkleBlast(attacker);
                m_NextSparkleBlast = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformSparkleBlast(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Emits a burst of glittery particles! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x3728, 10, 16); // Glitter effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("You are dazzled and confused by the glimmering blast!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                    m.SendMessage("You feel slowed down!");

                    // Apply damage over time
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => m.Damage(5, owner));
                }
            }
        }
    }
}
