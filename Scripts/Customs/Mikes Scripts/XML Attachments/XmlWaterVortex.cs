using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlWaterVortex : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Cooldown for Water Vortex
        private DateTime m_NextWaterVortex;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlWaterVortex(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlWaterVortex() { }

        [Attachable]
        public XmlWaterVortex(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextWaterVortex);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Refractory = reader.ReadTimeSpan();
                m_NextWaterVortex = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Water Vortex Attack: Cooldown of {m_Refractory.TotalSeconds} seconds.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextWaterVortex)
            {
                PerformWaterVortex(attacker);
                m_NextWaterVortex = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformWaterVortex(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(3))
            {
                if (m != owner && m != owner.Combatant && m.Player)
                {
                    m.SendMessage("You are pulled into a swirling vortex of water!");
                    m.MoveToWorld(new Point3D(owner.X + Utility.RandomMinMax(-2, 2), owner.Y + Utility.RandomMinMax(-2, 2), owner.Z), owner.Map);
                    m.Damage(Utility.RandomMinMax(10, 20), owner); // Damage dealt to the player
                }
            }
        }
    }
}
