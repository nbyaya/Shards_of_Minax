using System;
using Server;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class SkeletalLich : BaseCreature
	{
		[Constructable]
		public SkeletalLich() : base( AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal lich";
			Body = 309;
			Hue = 1345;
			BaseSoundID = 0x48D;
			

			SetStr( 301, 350 );
			SetDex( 75 );
			SetInt( 151, 200 );

			SetHits( 1200 );
			SetStam( 150 );
			SetMana( 0 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.EvalInt, 127.2 );
			SetSkill( SkillName.Magery, 127.2 );
			SetSkill( SkillName.Necromancy, 100.0, 120.0 );
			SetSkill( SkillName.MagicResist, 187.1 );
			SetSkill( SkillName.Tactics, 91.7 );
			SetSkill( SkillName.Wrestling, 98.5 );

			Fame = 6000;
			Karma = -6000;

            VirtualArmor = 40; 
            SetWeaponAbility(WeaponAbility.Dismount);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new StaffOfTheLichKing());
            }					
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override int TreasureMapLevel{ get{ return 1; } }

		public SkeletalLich( Serial serial ) : base( serial )
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