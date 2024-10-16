using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBananaBomb : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5);
        private DateTime m_EndTime;

        [Attachable]
        public XmlBananaBomb() { }

        public XmlBananaBomb(ASerial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_EndTime = reader.ReadDeltaTime();
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_EndTime && Utility.RandomDouble() < 0.25)
            {
                ThrowBananaBomb(attacker);
                m_EndTime = DateTime.UtcNow + m_Refractory;
            }
        }

        private void ThrowBananaBomb(Mobile attacker)
        {
            attacker.PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "* throws a Banana Bomb! *");
            attacker.PlaySound(0x208); // Throw sound

            Point3D loc = attacker.Location;
            BananaBombItem bomb = new BananaBombItem();
            bomb.MoveToWorld(loc, attacker.Map);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeBananaBomb(bomb, attacker));
        }

        private void ExplodeBananaBomb(BananaBombItem bomb, Mobile attacker)
        {
            if (bomb.Deleted)
                return;

            attacker.PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "* The Banana Bomb explodes in a mess of peels! *");
            attacker.PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(bomb.Location, bomb.Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in bomb.GetMobilesInRange(3))
            {
                if (m != attacker && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, attacker, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by the explosive banana bomb!");
                    m.PlaySound(0x1DD); // Explosion sound
                }
            }

            bomb.Delete();
        }
    }

    public class BananaBombItem : Item
    {
        public BananaBombItem() : base(0x171D) { Movable = false; }

        public BananaBombItem(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
