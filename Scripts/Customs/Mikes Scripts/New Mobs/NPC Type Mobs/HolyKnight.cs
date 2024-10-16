using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("corpse of a Holy Knight")]
	public class HolyKnight : BaseCreature
	{
		private DateTime lastDirectionChange;
		private TimeSpan directionChangeInterval = TimeSpan.FromSeconds(20);

		[Constructable]
		public HolyKnight() : base(AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4)
		{
			Name = "Holy Knight";
			Body = 400; // Adjust this as needed to fit the desired appearance
			BaseSoundID = 367;

			SetStr(100);
			SetDex(150);
			SetInt(25);

			SetHits(100);
			SetDamage(10, 20);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 30);
			SetResistance(ResistanceType.Fire, 30);
			SetResistance(ResistanceType.Cold, 30);
			SetResistance(ResistanceType.Poison, 30);
			SetResistance(ResistanceType.Energy, 30);

			SetSkill(SkillName.MagicResist, 50.0);
			SetSkill(SkillName.Tactics, 65.0);
			SetSkill(SkillName.Wrestling, 90.0);

			Fame = 9000;
			Karma = 9000;

			VirtualArmor = 30;
			
			AddItem( new PlateChest() );
			AddItem( new PlateArms() );
			AddItem( new PlateLegs() );
			AddItem( new Doublet( Utility.RandomNondyedHue() ) );
			Halberd weapon = new Halberd();

			weapon.Movable = false;
			weapon.Crafter = this;

			AddItem( weapon );

			lastDirectionChange = DateTime.Now;
			
			// Add the horse when the creature is created
			AddHorse();
			
			ActiveSpeed = 0.01;   // Default is around 0.2, lower is faster
			PassiveSpeed = 0.02;  // Default is around 0.4, lower is faster
		}

		public void AddHorse()
		{
			Horse mount = new Horse();
			mount.Rider = this; // This automatically mounts the creature on the horse
		}
		
		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now - lastDirectionChange > directionChangeInterval)
			{
				// Randomly change direction
				Direction = (Direction)Utility.Random(8);
				lastDirectionChange = DateTime.Now;
			}

			// Attempt to move in the current direction
			if (!Move(Direction))
			{
				// If movement is blocked, try a different direction
				Direction = (Direction)Utility.Random(8);
			}
		}


		public HolyKnight(Serial serial) : base(serial)
		{
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