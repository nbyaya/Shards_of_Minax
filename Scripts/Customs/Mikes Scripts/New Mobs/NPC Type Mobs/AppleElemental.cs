using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class AppleElemental : BaseCreature
    {
        [Constructable]
        public AppleElemental() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an Apple Elemental";
            this.Body = 752; // Adjust the body to fit an elemental visual you prefer
            
            this.SetStr(200);
            this.SetDex(200);
            this.SetInt(100);
            
            this.SetHits(150);
            
            this.SetDamage(10, 23);

            // Adjust these skills as you see fit
            this.SetSkill(SkillName.EvalInt, 80.0);
            this.SetSkill(SkillName.Magery, 80.0);
            this.SetSkill(SkillName.MagicResist, 75.0);
            this.SetSkill(SkillName.Tactics, 80.0);
            this.SetSkill(SkillName.Wrestling, 80.0);

            this.Fame = 4000;   // Set appropriate fame
            this.Karma = -4000; // Set appropriate karma
        }
		
		public override void OnDeath(Container c)
		{
						
			int applesToDrop = 10; // Number of apples to drop. Adjust as desired.

			for (int i = 0; i < applesToDrop; i++)
			{
				Point3D appleLocation = new Point3D(this.X + Utility.RandomMinMax(-2, 2), this.Y + Utility.RandomMinMax(-2, 2), this.Z);

				// Spawn the apple
				Apple droppedApple = new Apple();
				droppedApple.MoveToWorld(appleLocation, this.Map);

				// Create a flamestrike effect at the apple's location
				// EffectID for flamestrike, 0x3709 is a good starting point. Adjust as necessary.
				// The 10 is the speed of the effect, and 30 is the duration before it disappears. These values can be adjusted.
				// true for fixed direction, false for random direction, 0 for explosion effect, and 4 is the explosion sound.
				// 0 is the hue (normal flame color), and 0 is the render mode (normal rendering).
				Effects.SendLocationParticles(EffectItem.Create(appleLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 0, 0, 2023, 0);
			}
			
			base.OnDeath(c);
		}


        public AppleElemental(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
