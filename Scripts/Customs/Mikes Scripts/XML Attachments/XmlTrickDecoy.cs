using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTrickDecoy : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Default cooldown for the decoy ability
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlTrickDecoy(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTrickDecoy() { }

        [Attachable]
        public XmlTrickDecoy(double refractory)
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
            return String.Format("Creates a deceptive decoy every {0} seconds.", m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_NextUse)
            {
                CreateDecoy(attacker);
                m_NextUse = DateTime.Now + m_Refractory;
            }
        }

        private void CreateDecoy(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            Point3D loc = owner.Location; // You can make this random for variety
            Effects.SendLocationParticles(EffectItem.Create(loc, owner.Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 0x21, 0, 0, 0);
            TrickDecoy decoy = new TrickDecoy(owner);
            decoy.MoveToWorld(loc, owner.Map);
        }
    }

    public class TrickDecoy : BaseCreature
    {
        private Mobile m_Master;

        public TrickDecoy(Mobile master) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;
            Body = master.Body;
            Hue = master.Hue;
            Name = "a decoy";

            SetStr(1);
            SetDex(1);
            SetInt(1);
            SetHits(1);
            SetDamage(0);
            VirtualArmor = 100;
        }

        public TrickDecoy(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
