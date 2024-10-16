using System;
using Server;

namespace Server.Items
{
    public class ShadowArmor : BaseClothing
    {
        [Constructable]
        public ShadowArmor() : base(0x13DB)
        {
            Name = "Shadow Armor";
            Hue = 0x455;
            Attributes.BonusDex = 10;
            Attributes.BonusStam = 5;
        }

        public ShadowArmor(Serial serial) : base(serial)
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

    public class SmokeBoomb : Item
    {
        [Constructable]
        public SmokeBoomb() : base(0x2808)
        {
            Name = "Smoke Bomb";
            Hue = 0x8A5;
            Stackable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            if (from.BeginAction(typeof(SmokeBoomb)))
            {
                from.Hidden = true;
                from.SendMessage("You have used a smoke bomb and vanished!");
                from.FixedParticles(0x373A, 10, 15, 5037, EffectLayer.Head);
                from.PlaySound(0x1FD);

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(ReleaseUseLock), from);
            }
        }

        private static void ReleaseUseLock(object state)
        {
            ((Mobile)state).EndAction(typeof(SmokeBoomb));
        }

        public SmokeBoomb(Serial serial) : base(serial)
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
