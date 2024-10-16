using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFieryIllusion : XmlAttachment
    {
        private TimeSpan m_ActivationDelay = TimeSpan.FromMinutes(2); // Time between activations
        private DateTime m_NextActivation;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan ActivationDelay { get { return m_ActivationDelay; } set { m_ActivationDelay = value; } }

        public XmlFieryIllusion(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFieryIllusion() { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_ActivationDelay);
            writer.Write(m_NextActivation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ActivationDelay = reader.ReadTimeSpan();
            m_NextActivation = reader.ReadDateTime();
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextActivation)
            {
                CreateFieryIllusion(attacker);
                m_NextActivation = DateTime.UtcNow + m_ActivationDelay;
            }
        }

        private void CreateFieryIllusion(Mobile owner)
        {
            Point3D loc = GetSpawnPosition(owner, 2);

            if (loc != Point3D.Zero)
            {
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A fiery illusion is created! *");
                Effects.SendLocationParticles(EffectItem.Create(loc, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);

                FieryIllusion illusion = new FieryIllusion();
                illusion.MoveToWorld(loc, owner.Map);

                Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerCallback(delegate()
                {
                    if (!illusion.Deleted)
                    {
                        illusion.Delete();
                    }
                }));
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
