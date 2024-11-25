using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "corpse of a spacefaring pirate" )]
	public class PirateOfTheStars : BaseCreature
	{
		private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds( 15.0 ); // time between space pirate speech
		public DateTime m_NextSpeechTime;

		[Constructable]
		public PirateOfTheStars() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = 0x482; // Galactic silver hue
			Body = (Utility.RandomBool()) ? 0x191 : 0x190; // Using the same gender determination logic

			Name = "Pirate Of The Stars";
			Title = "The Dimensional Raider";

			AddItem(new PlateChest());
			AddItem(new PlateHelm());

			// Laser Cutlass
			AddItem(new Cutlass());
			Team = Utility.RandomMinMax(1, 5);

			SetStr( 800, 1200 );
			SetDex( 177, 255 );
			SetInt( 151, 250 );

			SetHits( 600, 1000 );

			SetDamage( 10, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Meditation, 25.1, 50.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill(SkillName.Anatomy, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Archery, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.ArmsLore, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Bushido, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Chivalry, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Fencing, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Lumberjacking, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Ninjitsu, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Parry, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Swords, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Tactics, Utility.RandomMinMax(50, 100));
			SetSkill(SkillName.Wrestling, Utility.RandomMinMax(50, 100));

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -18.9;

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 58;

			m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
		}

		public override int TreasureMapLevel { get { return 5; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return true; } }

		public override void OnThink()
		{
			base.OnThink();

			if ( DateTime.Now >= m_NextSpeechTime )
			{
				int phrase = Utility.Random( 4 );

				switch ( phrase )
				{
					case 0: this.Say( true, "Galaxies will bow before me!" ); break;
					case 1: this.Say( true, "You cannot hide from the stars!" ); break;
					case 2: this.Say( true, "Prepare to be plundered, space dweller!" ); break;
					case 3: this.Say( true, "The cosmic void is mine!" ); break;
				}

				m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
			}
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold( 300, 350 );
			AddLoot( LootPack.UltraRich );

			int phrase = Utility.Random( 2 );
			switch ( phrase )
			{
				case 0: this.Say( true, "To the black hole with me..." ); break;
				case 1: this.Say( true, "Stars... fading..." ); break;
			}

			PackItem( new MandrakeRoot( Utility.RandomMinMax( 15, 25 ) ) );
		}

		public PirateOfTheStars( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
