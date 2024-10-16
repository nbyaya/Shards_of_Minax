using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RitualCircle : Item
    {
        private Timer _flashTimer;
        private Timer _explodeTimer;

        [Constructable]
        public RitualCircle() : base(0x11B6) // Use an appropriate item ID for visual effect
        {
            Movable = false;
            _flashTimer = new FlashTimer(this);
            _flashTimer.Start();

            _explodeTimer = new ExplosionTimer(this);
            _explodeTimer.Start();
        }

        public RitualCircle(Serial serial) : base(serial)
        {
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (_flashTimer != null)
                _flashTimer.Stop();
            if (_explodeTimer != null)
                _explodeTimer.Stop();
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

        private class FlashTimer : Timer
        {
            private RitualCircle _circle;
            private bool _isOn;

            public FlashTimer(RitualCircle circle) : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.5))
            {
                _circle = circle;
                _isOn = false;
            }

            protected override void OnTick()
            {
                if (_circle.Deleted)
                    return;

                _isOn = !_isOn;
                _circle.ItemID = _isOn ? 0x1F3A : 0x1FD3; // Toggle between two different item IDs for flashing
            }
        }

        private class ExplosionTimer : Timer
        {
            private RitualCircle _circle;

            public ExplosionTimer(RitualCircle circle) : base(TimeSpan.FromSeconds(5))
            {
                _circle = circle;
            }

            protected override void OnTick()
            {
                if (_circle.Deleted)
                    return;

                _circle.Delete();

                // Create explosion effect
                Effects.PlaySound(_circle.Location, _circle.Map, 0x208); // Sound effect for explosion

                // Apply damage to players in the area
                foreach (Mobile m in _circle.GetMobilesInRange(2))
                {
                    if (m.Player)
                    {
                        m.Damage(50); // Apply damage, no need for a second argument
                    }
                }
            }
        }
    }
}
