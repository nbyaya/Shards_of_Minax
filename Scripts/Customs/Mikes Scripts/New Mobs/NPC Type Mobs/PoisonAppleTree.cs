using System;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    public class PoisonAppleTree : BaseCreature
    {

        [Constructable]
        public PoisonAppleTree() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a poison apple tree";
            Body = 47; // Adjust the body to match a tree graphic
            BaseSoundID = 442; // Tree rustling sound
			Hue = 2174; // Poison green color
			Team = Utility.RandomMinMax(1, 5);

            SetStr(196, 250);
            SetDex(31, 45);
            SetInt(66, 90);

            SetHits(118, 142);

            SetDamage(12, 19);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 40;

            // Scheduled method to perform the special attack
            Timer.DelayCall(TimeSpan.FromSeconds(4.0), TimeSpan.FromSeconds(8.0), ThrowPoisonApple);
        }
		
		public void ThrowPoisonApple()
		{
			if (this.Map == null || this.Map == Map.Internal || !this.Alive || this.IsDeadBondedPet) return;

			Mobile target = this.Combatant as Mobile;
			if (target != null && target.Map == this.Map && target.InRange(this, 8))
			{
				this.Say("How do you like them apples?!");
				
				// Simulate the poison apple effect
				Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 10, 10, 0, 0); // Green flamestrike
				Effects.PlaySound(target.Location, target.Map, 0x208); // Explosion sound

				// Apply damage within a radius
				foreach (Mobile m in this.GetMobilesInRange(2))
				{
					if (m == this || !CanBeHarmful(m, false)) continue;

					DoHarmful(m);
					AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 100, 0);
				}

				// Drop PoisonTile items around the target
				for (int i = 0; i < 5; i++)
				{
					Point3D dropPoint = new Point3D(target.X + Utility.RandomMinMax(-1, 1), target.Y + Utility.RandomMinMax(-1, 1), target.Z);
					if (Map.CanFit(dropPoint, 0, true, false))
					{
						Effects.SendLocationEffect(dropPoint, Map, 0x3709, 10, 10, 0, 0); // Green flamestrike
						Item poisonTile = new PoisonTile();
						poisonTile.MoveToWorld(dropPoint, Map);
					}
				}
			}
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

            Timer.DelayCall(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(45.0), ThrowPoisonApple);
        }

        public PoisonAppleTree(Serial serial) : base(serial)
        {
        }
    }
}
