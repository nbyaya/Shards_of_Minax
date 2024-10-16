using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlVortexPull : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextVortexPull;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlVortexPull(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlVortexPull() { }

        [Attachable]
        public XmlVortexPull(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextVortexPull);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Refractory = reader.ReadTimeSpan();
                m_NextVortexPull = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextVortexPull)
            {
                PerformVortexPull(attacker);
                m_NextVortexPull = DateTime.UtcNow + m_Refractory;
            }
        }

        private void PerformVortexPull(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Alive)
                {
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                    m.MoveToWorld(owner.Location, owner.Map);
                    m.SendMessage("You are pulled into a powerful vortex!");
                    m.Damage(Utility.RandomMinMax(10, 15), owner); // Pass the owner as the attacker
                }
            }
        }
    }
}
