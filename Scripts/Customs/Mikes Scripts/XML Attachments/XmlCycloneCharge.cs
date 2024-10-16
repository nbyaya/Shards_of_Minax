using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCycloneCharge : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for the cyclone charge
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown time between charges
        private DateTime m_NextCycloneCharge;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlCycloneCharge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlCycloneCharge() { }

        [Attachable]
        public XmlCycloneCharge(int damage, double cooldown)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextCycloneCharge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextCycloneCharge = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextCycloneCharge && defender != null)
            {
                PerformCycloneCharge(attacker, defender);
                m_NextCycloneCharge = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformCycloneCharge(Mobile owner, Mobile target)
        {
            if (target == null || owner.Map == null)
                return;

            Point3D destination = target.Location;
            owner.Location = destination;
            owner.Map = target.Map;

            Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);

            target.Damage(m_Damage, owner);
            target.SendMessage("A powerful cyclone charges at you!");
        }
    }
}
