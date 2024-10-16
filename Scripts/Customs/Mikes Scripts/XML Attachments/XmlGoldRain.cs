using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGoldRain : XmlAttachment
    {
        private DateTime m_NextGoldRain;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5); // Default cooldown time for Gold Rain

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlGoldRain(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlGoldRain() { }

        [Attachable]
        public XmlGoldRain(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextGoldRain);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Refractory = reader.ReadTimeSpan();
            m_NextGoldRain = reader.ReadDeltaTime();
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextGoldRain)
            {
                PerformGoldRain(attacker);
                m_NextGoldRain = DateTime.UtcNow + Refractory;
            }
        }

        private void PerformGoldRain(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* Rains gold coins *");
            owner.PlaySound(0x2E6);

            for (int i = 0; i < 5; i++)
            {
                Gold gold = new Gold(Utility.RandomMinMax(50, 100));
                gold.MoveToWorld(GetSpawnPosition(owner, 3), owner.Map);

                if (Utility.RandomDouble() < 0.2)
                {
                    gold.Name = "Trick Gold";
                    gold.Hue = 0x8A5;
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        if (!gold.Deleted) gold.Delete();
                    });
                }
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

            return owner.Location;
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Causes gold to rain down every {Refractory.TotalSeconds} seconds.";
        }
    }
}
