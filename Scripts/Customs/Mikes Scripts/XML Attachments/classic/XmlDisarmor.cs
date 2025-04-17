using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDisarmor : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10); 
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlDisarmor(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlDisarmor()
        {
        }

        [Attachable]
        public XmlDisarmor(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (defender != null && attacker != null)
            {
                DisarmorTarget(defender);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        private void DisarmorTarget(Mobile target)
        {
            if (target == null) return;

            Item[] items = target.Items.ToArray();

            // Randomly select armor or clothing items
            Item randomArmor = null;
            foreach (Item item in items)
            {
                if (item is BaseArmor || item is BaseClothing)
                {
                    randomArmor = item;
                    break;
                }
            }

            if (randomArmor != null)
            {
                target.SendMessage("Your armor has been removed!");
                target.PlaySound(0x3B9);  // play a sound for the disarm
                target.AddToBackpack(randomArmor);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            if (Refractory > TimeSpan.Zero)
            {
                return "Disarmor - " + Refractory.TotalSeconds + " secs between uses";
            }
            else
            {
                return "Disarmor";
            }
        }
    }
}
