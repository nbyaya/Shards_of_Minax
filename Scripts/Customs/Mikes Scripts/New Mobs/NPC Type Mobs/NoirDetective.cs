using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "corpse of a mysterious detective" )]
	public class NoirDetective : BaseCreature
	{
		private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds( 10.0 );
		public DateTime m_NextSpeechTime;

		[Constructable]
		public NoirDetective() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = 0x455; // A dark hue
			Body = Utility.RandomBool() ? 0x190 : 0x191; // Male or female
			Name = NameList.RandomName("male") + " the Detective"; // Using elf name list for a different feel

			Item hat = new WideBrimHat();
			Item shirt = new FancyShirt(0x455);
			Item pants = new LongPants(0x1BB);
			Item boots = new Boots(0x1BB);
			Item cloak = new Cloak(0x497); // Dark cloak

			AddItem(hat);
			AddItem(shirt);
			AddItem(pants);
			AddItem(boots);
			AddItem(cloak);

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

		public override bool AlwaysMurderer{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle { get { return true; } }

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextSpeechTime )
			{
				Mobile combatant = this.Combatant as Mobile;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 8 ) )
				{
					int phrase = Utility.Random( 3 );

					switch ( phrase )
					{
						case 0: this.Say( true, "The truth always surfaces." ); break;
						case 1: this.Say( true, "Darkness cannot hide from me." ); break;
						case 2: this.Say( true, "What secrets do you keep?" ); break;
					}
					
					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
				}

				base.OnThink();
			}			
		}

		public void DetectTrueNature( Mobile target )
		{
			if ( target != null && target is BaseCreature )
			{
				if ( ((BaseCreature)target).AlwaysMurderer )
				{
					this.Say( target.Name + " is a creature of pure malice!" );
				}
				else
				{
					this.Say( target.Name + " seems to be innocent." );
				}
			}
			else if ( target != null && target is PlayerMobile )
			{
				if ( target.Kills > 0 )
				{
					this.Say( target.Name + " has blood on their hands!" );
				}
				else
				{
					this.Say( target.Name + " seems to have a clean conscience." );
				}
			}
		}


		public override void GenerateLoot()
		{
			PackGold( 100, 150 );
			// Add more loot as per your shard's economy
		}

		public NoirDetective( Serial serial ) : base( serial )
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
