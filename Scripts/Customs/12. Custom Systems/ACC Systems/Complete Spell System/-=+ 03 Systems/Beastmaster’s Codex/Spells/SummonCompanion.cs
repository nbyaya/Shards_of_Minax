using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class SummonCompanion : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Summon Companion", "Ex Animas",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public SummonCompanion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_Types = new Type[]
        {
            typeof(GreatHart),
            typeof(FrostSpider),
            typeof(GiantSerpent),
            typeof(DireWolf)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Choose a random creature type to summon
                    Type beastType = m_Types[Utility.Random(m_Types.Length)];
                    BaseCreature summonedBeast = (BaseCreature)Activator.CreateInstance(beastType);

                    // Summon the creature with flashy effects
                    SpellHelper.Summon(summonedBeast, Caster, 0x215, TimeSpan.FromSeconds(60.0), false, false);

                    // Play sound and visual effects
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE);
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 1, 13, 1153, 7, 9962, 0);

                    // Enhance the summoned creature with additional effects
                    EnhanceSummonedCreature(summonedBeast);

                    // Output a message to the player
                    Caster.SendMessage("You summon a powerful companion to aid you!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SummonCompanion: Error while casting spell - " + ex.Message);
                }
            }

            FinishSequence();
        }

        private void EnhanceSummonedCreature(BaseCreature creature)
        {
            if (creature != null)
            {
                creature.HitsMaxSeed *= 2; // Double the health
                creature.Hits = creature.HitsMax; // Set to max health
                creature.DamageMin += 5; // Increase minimum damage
                creature.DamageMax += 10; // Increase maximum damage

                // Add some additional effects (visual and buffs)
                creature.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                creature.PlaySound(0x2F);

                // Add a temporary buff
                BuffInfo.AddBuff(creature, new BuffInfo(BuffIcon.Strength, 1044120, 1075849, TimeSpan.FromSeconds(60), creature));
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
