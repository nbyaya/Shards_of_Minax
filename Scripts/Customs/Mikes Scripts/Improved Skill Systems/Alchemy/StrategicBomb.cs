using System;
using Server.Targeting;

namespace Server.Items
{
    public class StrategicBomb : BaseStrategicBomb
    {

        [Constructable]
        public StrategicBomb()
            : base(PotionEffect.ExplosionGreater)
        {
            Hue = 2609; // A distinct purple color
			Name = "Strategic Bomb";
        }

        public StrategicBomb(Serial serial)
            : base(serial)
        {
        }

        public override int MinDamage
        {
            get
            {
                return 50; // Adjust this value for desired minimum damage
            }
        }

        public override int MaxDamage
        {
            get
            {
                return 100; // Adjust this value for desired maximum damage
            }
        }

        public override void Drink(Mobile from)
        {
            if (from.Skills.Alchemy.Value < 75.0)
            {
                from.SendMessage("You lack the alchemy skill to use this potion.");
                return;
            }

            base.Drink(from);
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