using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlToxicSludge : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromMinutes(1);
        private DateTime m_NextToxicSludge;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlToxicSludge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlToxicSludge() { }

        [Attachable]
        public XmlToxicSludge(double refractory)
        {
            Refractory = TimeSpan.FromMinutes(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextToxicSludge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_NextToxicSludge = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Creates a pool of toxic sludge that poisons nearby foes.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextToxicSludge)
            {
                CastToxicSludge(attacker);
                m_NextToxicSludge = DateTime.UtcNow + m_Refractory;
            }
        }

        private void CastToxicSludge(Mobile owner)
        {
            Point3D loc = GetSpawnPosition(2, owner);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 9502);
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A pool of toxic sludge forms! *");

                foreach (Mobile m in owner.Map.GetMobilesInRange(loc, 2))
                {
                    if (m != owner && m.Alive)
                    {
                        m.SendMessage("You are poisoned by the toxic sludge!");
                        m.ApplyPoison(owner, Poison.Lethal);
                    }
                }
            }
        }

        private Point3D GetSpawnPosition(int range, Mobile owner)
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
