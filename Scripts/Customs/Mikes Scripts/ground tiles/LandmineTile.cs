using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class LandmineTile : Item
    {
        private Timer m_Timer;

        [Constructable]
        public LandmineTile() : base(0x283B) // Use an appropriate item ID for a landmine tile
        {
            Movable = false;
            Name = "a landmine";

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), CheckForPlayers);
        }

        private void CheckForPlayers()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(0);
            foreach (Mobile m in eable)
            {
                    Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                    Effects.PlaySound(Location, Map, 0x307);
                    m.Damage(25, m);

                    this.Delete(); // Delete the landmine after triggering
                    break; // Stop checking after the mine has exploded

            }
            eable.Free();
        }

        public LandmineTile(Serial serial) : base(serial)
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
