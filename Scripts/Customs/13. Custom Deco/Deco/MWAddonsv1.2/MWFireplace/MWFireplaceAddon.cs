/////////////////////////////////////////////////
//
// Automatically generated by the
// AddonGenerator script by Arya
//
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MWFireplaceAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new MWFireplaceAddonDeed();
			}
		}

		[ Constructable ]
		public MWFireplaceAddon()
		{
			AddComponent( new AddonComponent( 1981 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 2 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 4 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 6 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 8 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 10 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 12 );
			AddComponent( new AddonComponent( 1981 ), 0, 2, 14 );
			AddComponent( new AddonComponent( 1981 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 1981 ), 0, 1, 14 );
			AddComponent( new AddonComponent( 4012 ), 0, 1, 2 );
			AddComponent( new AddonComponent( 6571 ), 0, 1, 5 );
			AddComponent( new AddonComponent( 1981 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 1981 ), 0, 0, 14 );
			AddComponent( new AddonComponent( 6571 ), 0, 0, 4 );
			AddComponent( new AddonComponent( 4012 ), 0, 0, 2 );
			AddComponent( new AddonComponent( 1981 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 1981 ), 0, -1, 14 );
			AddComponent( new AddonComponent( 4012 ), 0, -1, 2 );
			AddComponent( new AddonComponent( 6571 ), 0, -1, 5 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 2 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 4 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 6 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 8 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 10 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 12 );
			AddComponent( new AddonComponent( 1981 ), 0, -2, 14 );
			AddonComponent ac = null;
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 2 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 2 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 4 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 6 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 8 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 10 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 12 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 4 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 6 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 8 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 10 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 12 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 2, 14 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -2, 14 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 1, 14 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, 0, 14 );
			ac = new AddonComponent( 1981 );
			ac.Hue = 1872;
			AddComponent( ac, 0, -1, 14 );
			ac = new AddonComponent( 6571 );
			ac.Light = LightType.ArchedWindowEast;
			AddComponent( ac, 0, 0, 4 );
			ac = new AddonComponent( 4012 );
			ac.Light = LightType.Circle225;
			AddComponent( ac, 0, 0, 2 );
			ac = new AddonComponent( 4012 );
			ac.Light = LightType.Circle225;
			AddComponent( ac, 0, -1, 2 );
			ac = new AddonComponent( 4012 );
			ac.Light = LightType.Circle225;
			AddComponent( ac, 0, 1, 2 );
			ac = new AddonComponent( 6571 );
			ac.Light = LightType.Circle225;
			AddComponent( ac, 0, -1, 5 );
			ac = new AddonComponent( 6571 );
			ac.Light = LightType.Circle225;
			AddComponent( ac, 0, 1, 5 );

		}

		public MWFireplaceAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class MWFireplaceAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new MWFireplaceAddon();
			}
		}

		[Constructable]
		public MWFireplaceAddonDeed()
		{
			Name = "MWFireplace";
		}

		public MWFireplaceAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}