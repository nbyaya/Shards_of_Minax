using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class MagnetTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public MagnetTile() : base(0x1AC5) // Use an appropriate tile ID
        {
            Movable = false;
            Name = "a magnetic disturbance";
            Hue = 1341; // Dark blue hue

            AutoDelete = TimeSpan.FromSeconds(30); // Tile lasts for 30 seconds

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), DoMagneticEffect);
            StartDeleteTimer();
        }

        private void DoMagneticEffect()
        {
            if (Deleted) return;

            // Play the magical animation
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 
                0x376A, 9, 32, 5024, 0, 5024, 0); // Dark blue magical effect
            Effects.PlaySound(Location, Map, 0x1F0); // Magnetic sound effect

            // Check for nearby players
            IPooledEnumerable eable = GetMobilesInRange(1); // Affects tiles adjacent to this one too
            foreach (Mobile m in eable)
            {
                if (m is PlayerMobile player)
                {
                    TryDropRandomItem(player);
                }
            }
            eable.Free();
        }

        private void TryDropRandomItem(PlayerMobile player)
        {
            List<Item> equippedItems = new List<Item>();
            foreach (Item item in player.Items)
            {
                if (item.Layer != Layer.Bank && item.Layer != Layer.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair)
                {
                    equippedItems.Add(item);
                }
            }

            if (equippedItems.Count > 0)
            {
                Item toDrop = equippedItems[Utility.Random(equippedItems.Count)];
                player.RemoveItem(toDrop);
                toDrop.MoveToWorld(player.Location, player.Map);
                player.SendLocalizedMessage(502690); // Your equipment is yanked from you and falls to the ground!
            }
        }

        private void StartDeleteTimer()
        {
            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            m_DeleteTimer = Timer.DelayCall(AutoDelete, DeleteTimer);
        }

        private void DeleteTimer()
        {
            this.Delete();
        }

        public MagnetTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), DoMagneticEffect);
            StartDeleteTimer();
        }
    }
}