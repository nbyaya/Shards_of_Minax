using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class CrossbowOfPrecision : HeavyCrossbow
    {
        public override int LabelNumber { get { return 1070850; } } // Crossbow of Precision

        [Constructable]
        public CrossbowOfPrecision() : base()
        {
            Hue = 0x4F2;
            Attributes.WeaponSpeed = 35;
            Attributes.WeaponDamage = 50;
        }

        public CrossbowOfPrecision(Serial serial) : base(serial)
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

    public class TellsApple : Food
    {
        public override int LabelNumber { get { return 1070851; } } // Tell's Apple

        [Constructable]
        public TellsApple() : base(0x9D0)
        {
            Hue = 0x21;
            Stackable = false;
            Weight = 1.0;
        }

        public override bool Eat(Mobile from)
        {
            if (base.Eat(from))
            {
                from.PlaySound(0x1F7);
                from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                
                new AppleTimer(from).Start();
                
                return true;
            }
            return false;
        }

        private class AppleTimer : Timer
        {
            private Mobile m_Mobile;

            public AppleTimer(Mobile m) : base(TimeSpan.FromMinutes(5))
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && !m_Mobile.Deleted)
                {
                    m_Mobile.SendLocalizedMessage(1070852); // The effects of Tell's Apple wear off.
                }
            }
        }

        public TellsApple(Serial serial) : base(serial)
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

    public class ArrowOfPiercing : Arrow
    {
        public override int LabelNumber { get { return 1070853; } } // Arrow of Piercing

        [Constructable]
        public ArrowOfPiercing() : this(1)
        {
        }

        [Constructable]
        public ArrowOfPiercing(int amount) : base(amount)
        {
            Hue = 0x4F2;
        }

        public ArrowOfPiercing(Serial serial) : base(serial)
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

    public class BlackQuiver : BaseQuiver
    {
        public override int LabelNumber { get { return 1070855; } } // Quiver of Infinity

        [Constructable]
        public BlackQuiver() : base()
        {
            Hue = 0x4F2;
            WeightReduction = 50;
            DamageIncrease = 10;
            Attributes.WeaponSpeed = 10;
            LowerAmmoCost = 20;
        }

        public BlackQuiver(Serial serial) : base(serial)
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