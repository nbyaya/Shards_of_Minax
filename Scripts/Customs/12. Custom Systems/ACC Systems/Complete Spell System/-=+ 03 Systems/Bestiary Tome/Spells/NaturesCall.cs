using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class NaturesCall : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Nature’s Call", "Call of the Wild",
                                                        21005,
                                                        9400,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public NaturesCall(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_AnimalTypes = new Type[]
        {
            typeof(DireWolf),
            typeof(BlackBear),
            typeof(Eagle),
            typeof(Pig),
            typeof(GreatHart),
            typeof(GiantSpider)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    int numberOfAnimals = 2 + (int)(Caster.Skills[CastSkill].Value / 50.0);
                    for (int i = 0; i < numberOfAnimals; i++)
                    {
                        SummonRandomAnimal();
                    }

                    // Visual effects for the summoning
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x20E);

                    Caster.SendMessage("You call upon the wild to assist you!");
                }
                catch
                {
                    Caster.SendMessage("The summoning failed.");
                }
            }

            FinishSequence();
        }

		private void SummonRandomAnimal()
		{
			Type animalType = m_AnimalTypes[Utility.Random(m_AnimalTypes.Length)];
			BaseCreature animal = (BaseCreature)Activator.CreateInstance(animalType);

			// Set animal properties
			animal.Controlled = true;
			animal.ControlMaster = Caster;
			animal.IsBonded = false;

			// Random location around caster
			Point3D location = new Point3D(
				Caster.X + Utility.RandomMinMax(-2, 2),
				Caster.Y + Utility.RandomMinMax(-2, 2),
				Caster.Z
			);

			// Summon animal with the additional parameters
			SpellHelper.Summon(
				animal,
				Caster,
				0,                      // scaleStats (set to 0 if you don't want to scale)
				TimeSpan.FromSeconds(60.0), // duration
				true,                   // isControlled
				false,                  // isBonded
				true,                   // isPet
				SkillName.AnimalLore     // skill (change this if needed)
			);
		}


        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
