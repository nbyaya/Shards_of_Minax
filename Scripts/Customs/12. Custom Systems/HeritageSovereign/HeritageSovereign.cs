// Created by Dan(Tasanar) of https://trueuo.com
using System;
using System.Collections;
using System.Collections.Generic;

using Server.Accounting;
using Server.Mobiles;

namespace Server.Items
{
    public class HeritageSovereign : Item
    {
		[Constructable]
        public HeritageSovereign()
            : this(1)
        {
        }
		
        [Constructable]
        public HeritageSovereign(int amount)
            : base(0x1F31)
        {
			LootType = LootType.Blessed;
			Name = "Sovereign Notes";
			Hue = 1152;
			Stackable = true;
			Amount = amount;
        }

        public HeritageSovereign(Serial serial)
            : base(serial)
        {
        }
		
		public override void GetProperties( ObjectPropertyList list )
        {
            base.GetProperties( list );

            list.Add( "Reward Currency" );
			list.Add( "Double Click: Deposit to your virtual wallet" );
        }
		
		public override double DefaultWeight
        {
            get
            {
                return (Core.ML ? (0.01 / 3) : 0.01);
            }
		}
		
		public override void OnDoubleClick(Mobile from) 
      	{
			if (!IsChildOf(from.Backpack))
		{
            from.SendLocalizedMessage(1042001);
        }
        else
            {	
				PlayerMobile pm = from as PlayerMobile;
						
				pm.DepositSovereigns(this.Amount);                
		
				this.Delete(); 
				from.SendMessage("Your Sovereigns have been added to your virtual wallet. Visit the UO Store to view and spend your Sovereigns");			
			}

		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
			
        }
    }
}