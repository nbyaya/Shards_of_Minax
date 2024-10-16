using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTeleportAbility : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(60); // Cooldown for Teleport
        private DateTime m_NextTeleport;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlTeleportAbility(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTeleportAbility() { }

        public XmlTeleportAbility(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextTeleport);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextTeleport = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Teleport Ability: Allows the user to teleport to a nearby location.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextTeleport)
            {
                Teleport(attacker);
                m_NextTeleport = DateTime.UtcNow + m_Cooldown; // Set the next teleport time
            }
        }

        private void Teleport(Mobile owner)
        {
            if (owner.Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(10, owner); // Pass owner here
                if (newLocation != Point3D.Zero && !newLocation.Equals(owner.Location))
                {
                    owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A creature vanishes in a puff of smoke! *");
                    Effects.SendLocationParticles(owner, 0x3709, 10, 30, 5025);
                    owner.MoveToWorld(newLocation, owner.Map);
                    owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A creature reappears at a new location! *");
                }
            }
        }

        private Point3D GetSpawnPosition(int range, Mobile owner)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = owner.Location.X + Utility.RandomMinMax(-range, range);
                int y = owner.Location.Y + Utility.RandomMinMax(-range, range);
                int z = owner.Map.GetAverageZ(x, y); // Use owner.Map here

                Point3D p = new Point3D(x, y, z);

                if (owner.Map.CanSpawnMobile(p)) // Use owner.Map here
                    return p;
            }

            return Point3D.Zero;
        }
    }
}
