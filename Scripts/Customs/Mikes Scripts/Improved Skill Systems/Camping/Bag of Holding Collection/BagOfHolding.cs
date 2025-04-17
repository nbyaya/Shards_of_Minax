using System;
using Server.Items;

namespace Server.Items
{
    public class BagOfHolding : Bag
    {
        [Constructable]
		public BagOfHolding() : this(24)      // create a 125 item bag by default
		{
		}
		
		[Constructable]
		public BagOfHolding(int maxitems) : base()
		{
			Weight = 1.0;
			MaxItems = maxitems;
			Name = "Bag of Holding";
			Hue = 1923;
            LootType = LootType.Blessed;
		}

		public BagOfHolding( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
                public override int GetTotal(TotalType type)
        {
            if (type != TotalType.Weight)
                return base.GetTotal(type);
            else
            {
                return (int)(TotalItemWeights() * (0.0));
            }
        } 
		      public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            if (type != TotalType.Weight)
                base.UpdateTotal(sender, type, delta);
            else
                base.UpdateTotal(sender, type, (int)(delta * (0.0)));
        }
		private double TotalItemWeights()
        {
            double weight = 0.0;

            foreach (Item item in Items)
                weight += (item.Weight * (double)(item.Amount));

            return weight;
        }
	}
}