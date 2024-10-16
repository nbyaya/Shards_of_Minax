using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTheftStrike : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10); // 10 seconds default time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlTheftStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTheftStrike()
        {
        }

        [Attachable]
        public XmlTheftStrike(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (defender != null && attacker != null)
            {
                TheftItemFromTarget(attacker, defender);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        private void TheftItemFromTarget(Mobile attacker, Mobile defender)
        {
            if (defender == null || attacker == null) return;

            Container backpack = defender.Backpack;
            if (backpack == null || backpack.Items.Count == 0) return;

            Item stolenItem = backpack.Items[Utility.Random(backpack.Items.Count)];
            if (stolenItem == null) return;

            defender.SendMessage("An item has been stolen from you!");
            attacker.SendMessage("You have stolen an item!");

            backpack.RemoveItem(stolenItem);

            // Ensure attacker has a backpack to store the stolen item
            Container attackerBackpack = attacker.Backpack;
            if (attackerBackpack == null)
            {
                attackerBackpack = new Backpack();
                attacker.AddItem(attackerBackpack);
            }

            attackerBackpack.DropItem(stolenItem);
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
                return "Theft Strike - " + Refractory.TotalSeconds + " secs between uses";
            }
            else
            {
                return "Theft Strike";
            }
        }
    }
}
