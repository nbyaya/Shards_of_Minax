using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class HotLavaTile : Item
    {
        private Timer m_Timer;

        [Constructable]
        public HotLavaTile() : base(0x122A) // Use an appropriate item ID for a landmine tile
        {
            Movable = false;
            Name = "hot lava";
			Hue = 47; // Lava color

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), CheckForPlayers);
        }

        private void CheckForPlayers()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(0);
            foreach (Mobile m in eable)
            {
                    m.Damage(25, m);

                    this.Delete(); // Delete the landmine after triggering
                    break; // Stop checking after the mine has exploded

            }
            eable.Free();
        }

        public HotLavaTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), CheckForPlayers);
        }
    }
}
