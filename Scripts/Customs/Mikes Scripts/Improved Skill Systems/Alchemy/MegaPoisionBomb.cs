using System;
using Server.Targeting;

namespace Server.Items
{
    public class MegaPoisionBomb : BasePoisonBomb
    {

        [Constructable]
        public MegaPoisionBomb()
            : base(PotionEffect.ExplosionGreater)
        {
            Hue = 2511; // A distinct green color
			Name = "Mega Poision Bomb";
        }

        public MegaPoisionBomb(Serial serial)
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
            if (from.Skills.Alchemy.Value < 100.0)
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