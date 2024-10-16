using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network; // Make sure to include this for MessageType

namespace Server.Engines.XmlSpawner2
{
    public class XmlTrap : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(6); // Default cooldown for the trap ability
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlTrap(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTrap() { }

        [Attachable]
        public XmlTrap(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.Write(m_NextUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Refractory = reader.ReadTimeSpan();
            m_NextUse = reader.ReadDateTime();
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Sets a devious trap every {0} seconds.", m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_NextUse)
            {
                LayTrap(attacker);
                m_NextUse = DateTime.Now + m_Refractory;
            }
        }

        private void LayTrap(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            Point3D loc = owner.Location; // Optionally randomize this as well
            Effects.SendLocationParticles(EffectItem.Create(loc, owner.Map, EffectItem.DefaultDuration), 0x36BD, 10, 16, 0x21, 0, 0, 0);
            Trap trap = new Trap(owner);
            trap.MoveToWorld(loc, owner.Map);
        }
    }

    public class Trap : Item
    {
        private Mobile m_Owner;
        private DateTime m_TriggerTime;

        public Trap(Mobile owner) : base(0x9B5)
        {
            m_Owner = owner;
            Movable = false;
            Hue = 1155; // Unique hue for Trap
            m_TriggerTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
        }

        public Trap(Serial serial) : base(serial) { }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.UtcNow >= m_TriggerTime)
            {
                Trigger();
            }
        }

        private void Trigger()
        {
            if (Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The trap is triggered!*");
            Effects.PlaySound(GetWorldLocation(), Map, 0x307);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != m_Owner && m.Alive)
                {
                    AOS.Damage(m, m_Owner, Utility.RandomMinMax(10, 30), 0, 100, 0, 0, 0);
                    m.PlaySound(0x1DD);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Owner);
            writer.Write(m_TriggerTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_TriggerTime = reader.ReadDateTime();
        }
    }
}
