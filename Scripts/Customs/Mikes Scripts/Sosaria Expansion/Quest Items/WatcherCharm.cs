using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class WatcherCharm : Item
    {
        [Constructable]
        public WatcherCharm() : base(0x1F06) // Charm model
        {
            Name = "Watcher’s Charm";
            Hue = 2075; // Glowing effect
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!Deleted && m is PlayerMobile && m.InRange(this.GetWorldLocation(), 8))
            {
                foreach (Mobile mob in m.GetMobilesInRange(8))
                {
                    if (mob is CultVoicebearer)
                    {
                        m.SendMessage(0x23, "Your Watcher’s Charm pulses faintly...");
                        break;
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public WatcherCharm(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
