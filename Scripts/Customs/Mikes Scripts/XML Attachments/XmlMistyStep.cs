using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMistyStep : XmlAttachment
    {
        private DateTime m_NextAirborneEscape;
        private int m_Range = 10; // Default range for the escape

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        public XmlMistyStep(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlMistyStep() { }

        [Attachable]
        public XmlMistyStep(int range)
        {
            Range = range;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Range);
            writer.WriteDeltaTime(m_NextAirborneEscape);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Range = reader.ReadInt();
                m_NextAirborneEscape = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Airborne Escape: Evades swiftly within {0} tiles.", m_Range);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextAirborneEscape)
            {
                PerformAirborneEscape(attacker);
                m_NextAirborneEscape = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Recalculate next activation
            }
        }

        public void PerformAirborneEscape(Mobile owner)
        {
            if (owner == null || owner.Combatant == null || owner.Map == null)
                return;

            Point3D newLocation = GetSpawnPosition(owner, m_Range);
            if (newLocation != Point3D.Zero)
            {
                owner.Location = newLocation;
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A swift maneuver evades the danger! *");
                Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);
            }
        }

        private Point3D GetSpawnPosition(Mobile owner, int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = owner.X + Utility.RandomMinMax(-range, range);
                int y = owner.Y + Utility.RandomMinMax(-range, range);
                int z = owner.Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (owner.Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }
    }
}
