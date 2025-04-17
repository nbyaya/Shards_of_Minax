using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    public class BodyCycler : BaseCreature
    {
        private static readonly int MaxBodyValue = 2000; // Adjust based on the maximum body ID allowed in your shard
        private static readonly int CycleDelay = 2000; // Delay in milliseconds (2 seconds)

        private Timer _bodyCycleTimer;
        private int _currentBodyValue;

        [Constructable]
        public BodyCycler() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            _currentBodyValue = 1015; // Set the starting body value here
            Body = _currentBodyValue; // Start with the specified body value
            Name = $"Body #{_currentBodyValue} Creature";
            BaseSoundID = 0; // Set to 0 initially since body values might not have specific sounds

            SetStr(100);
            SetDex(100);
            SetInt(100);

            SetHits(500);
            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20);
            SetResistance(ResistanceType.Fire, 20);
            SetResistance(ResistanceType.Cold, 20);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 20);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 50;

            Tamable = false;
			CanMove = false;

            _bodyCycleTimer = new BodyCycleTimer(this);
            _bodyCycleTimer.Start();
        }

        private void UpdateName()
        {
            Name = $"Body #{Body} Creature";
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (_bodyCycleTimer != null)
            {
                _bodyCycleTimer.Stop();
                _bodyCycleTimer = null;
            }
        }

        private class BodyCycleTimer : Timer
        {
            private readonly BodyCycler _cycler;

            public BodyCycleTimer(BodyCycler cycler) : base(TimeSpan.Zero, TimeSpan.FromMilliseconds(CycleDelay))
            {
                _cycler = cycler;
            }

            protected override void OnTick()
            {
                if (_cycler == null || _cycler.Deleted)
                    return;

                _cycler._currentBodyValue++;
                if (_cycler._currentBodyValue > MaxBodyValue)
                    _cycler._currentBodyValue = 0;

                _cycler.Body = _cycler._currentBodyValue;
                _cycler.UpdateName();
            }
        }

        public BodyCycler(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(_currentBodyValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _currentBodyValue = reader.ReadInt();
            Body = _currentBodyValue;
            UpdateName();

            _bodyCycleTimer = new BodyCycleTimer(this);
            _bodyCycleTimer.Start();
        }
    }
}
