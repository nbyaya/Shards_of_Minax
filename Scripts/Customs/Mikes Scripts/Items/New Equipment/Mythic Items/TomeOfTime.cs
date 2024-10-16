using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class TomeOfTime : Item
    {
        private Timer m_Timer;

        [Constructable]
        public TomeOfTime() : base(0x1C11) // You can change the item ID to a book graphic or other appropriate graphic
        {
            Name = "Tome of Time";
            Hue = 1150; // Color of the item, change as you see fit
            Weight = 3.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Timer == null || !m_Timer.Running)
            {
                from.SendMessage("You use the power of the Tome of Time!");

                foreach (Mobile mob in GetMobilesInRange(5)) // 5 is the range; adjust as needed
                {
                    if (mob is BaseCreature)
                    {
                        BaseCreature creature = (BaseCreature)mob;
                        creature.ActiveSpeed *= 2;   // Double the delay between actions
                        creature.PassiveSpeed *= 2;  // Double the delay between actions
                    }
                }

                m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10), ResetSpeed, from); // 10 seconds duration, adjust as needed
            }
            else
            {
                from.SendMessage("The tome is still recharging.");
            }
        }

        private void ResetSpeed(Mobile from)
        {
            foreach (Mobile mob in GetMobilesInRange(5))
            {
                if (mob is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)mob;
                    creature.ActiveSpeed /= 2;   // Reset to normal speed
                    creature.PassiveSpeed /= 2;  // Reset to normal speed
                }
            }
            from.SendMessage("The effect of the Tome of Time fades.");
        }

        public TomeOfTime(Serial serial) : base(serial)
        {
        }

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
