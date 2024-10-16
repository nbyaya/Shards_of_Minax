using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class MapOfTheLost : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Map of the Lost", "Summon Cat",
                                                        // SpellCircle.Second, // This is optional depending on your spell system
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 30; } }

        public MapOfTheLost(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual effects
                Caster.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Head); // Example visual effect
                Caster.PlaySound(0x20F); // Example sound effect

                // Summoning the cat
                BaseCreature cat = new Cat();
                SpellHelper.Summon(cat, Caster, 0x215, TimeSpan.FromMinutes(10), false, false);

                // Flashy entry effect for the summoned creature
                cat.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Effect on the cat
                cat.PlaySound(0x69); // Cat sound upon summoning
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0); // Adjusted for balance
        }
    }
}
