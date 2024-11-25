using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "corpse of a medieval meteorologist" )]
	public class MedievalMeteorologist : BaseCreature
	{
		private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds( 20.0 ); // Time between meteorologist's predictions
		public DateTime m_NextSpeechTime;

		[Constructable]
		public MedievalMeteorologist() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = Utility.RandomSkinHue();
			Team = Utility.RandomMinMax(1, 5);

			if ( Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
				Title = "the Weather Seer";
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
				Title = "the Storm Caller";
			}
			
			// Giving our meteorologist a robe and sandals, typical of scholars
			Item robe = new Robe( Utility.RandomNeutralHue() );
			Item sandals = new Sandals( Utility.RandomNeutralHue() );
			
			AddItem( robe );
			AddItem( sandals );

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

		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle { get { return true; } }

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextSpeechTime )
			{
				int phrase = Utility.Random( 5 );

				switch ( phrase )
				{
					case 0: this.Say( true, "A storm brews in the distance." ); break;
					case 1: this.Say( true, "My relics foretell sunshine tomorrow." ); break;
					case 2: this.Say( true, "The ancient tomes predict rain." ); break;
					case 3: this.Say( true, "Winds from the west signal change." ); break;
					case 4: this.Say( true, "The stars suggest a clear night ahead." ); break;
				}
				
				m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
				
				base.OnThink();
			}		
		}

		public override void GenerateLoot()
		{
			PackGold( 50, 100 );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Potions );


		}

		public MedievalMeteorologist( Serial serial ) : base( serial )
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
