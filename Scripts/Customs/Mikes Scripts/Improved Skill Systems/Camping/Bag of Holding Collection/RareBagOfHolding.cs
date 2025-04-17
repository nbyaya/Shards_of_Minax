using Server;
using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class RareBagOfHolding : Bag
    {
        [Constructable]
		public RareBagOfHolding() : this(125)      // create a 125 item bag by default
		{
		}
		
		[Constructable]
		public RareBagOfHolding(int maxitems) : base()
		{
			Weight = 1.0;
			MaxItems = maxitems;
			Name = "Rare Bag of Holding";
			Hue = 1919;
            LootType = LootType.Blessed;
		}

		public RareBagOfHolding( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					Weight = 1.0;
					MaxItems = 125;
					Name = "Rare Bag of Holding";
					Hue = 1919;
            		LootType = LootType.Blessed;
					break;
				}
			}
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