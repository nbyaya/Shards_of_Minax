using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class SpectralAnalysis : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Spectral Analysis", "Ghosus Mostus",
            21004,
            9300,
            false,
            Reagent.Bloodmoss,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 20; } }

        public SpectralAnalysis(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_Types = new Type[]
        {
            typeof(Shade),
            typeof(Spectre)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    int numSummons = Utility.RandomMinMax(2, 5); // Summon between 2 to 5 creatures
                    for (int i = 0; i < numSummons; i++)
                    {
                        Type creatureType = m_Types[Utility.Random(m_Types.Length)];
                        BaseCreature creature = (BaseCreature)Activator.CreateInstance(creatureType);
                        creature.Controlled = true;
                        creature.ControlMaster = Caster;
                        creature.SummonMaster = Caster;
                        creature.IsBonded = true; // Prevents creatures from turning hostile when duration expires
                        creature.MoveToWorld(Caster.Location, Caster.Map);

                        SpellHelper.Summon(creature, Caster, 0x215, TimeSpan.FromSeconds(60.0 * Caster.Skills[CastSkill].Value / 100.0), false, false);

                        // Visual and sound effects for each summon
                        Effects.SendLocationParticles(EffectItem.Create(creature.Location, creature.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5044);
                        creature.PlaySound(0x482); // Play ghostly sound effect
                    }
                }
                catch
                {
                    // Handle errors gracefully
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0); // 5-second casting delay
        }
    }
}
