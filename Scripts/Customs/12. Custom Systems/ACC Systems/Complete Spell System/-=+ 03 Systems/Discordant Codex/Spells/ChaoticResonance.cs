using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.SkillHandlers;
using Server;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class ChaoticResonance : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Chaotic Resonance", "In Vas Grav",
            21004, // Animation ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 25; } }

        public ChaoticResonance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.RevealingAction();
                Caster.FixedParticles(0x3709, 1, 30, 9965, 1160, 7, EffectLayer.Waist);
                Caster.PlaySound(0x5C9); // Flashy sound effect

                // Get all mobiles within 2 tile radius
                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(Caster.Location, 2);

                foreach (Mobile m in eable)
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                eable.Free();

                foreach (Mobile m in targets)
                {
                    Caster.DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(20, 40), Caster); // Inflict random damage between 20 and 40

                    // Apply Chaotic Resonance effects
                    ApplyChaoticResonanceEffect(Caster, m);

                    // Visual and sound effects for targets
                    m.FixedParticles(0x3779, 10, 30, 5052, EffectLayer.Head);
                    m.PlaySound(0x1FB);
                }
            }

            FinishSequence();
        }

        private void ApplyChaoticResonanceEffect(Mobile caster, Mobile target)
        {
            caster.SendLocalizedMessage(1049539); // You play a chaotic resonance affecting your target

            int effect = -20; // Example effect value; adjust as needed
            double scalar = effect * 0.01;

            // Apply stat reductions
            target.AddStatMod(new StatMod(StatType.Str, "ChaoticResonanceStr", (int)(target.RawStr * scalar), TimeSpan.FromSeconds(10)));
            target.AddStatMod(new StatMod(StatType.Int, "ChaoticResonanceInt", (int)(target.RawInt * scalar), TimeSpan.FromSeconds(10)));
            target.AddStatMod(new StatMod(StatType.Dex, "ChaoticResonanceDex", (int)(target.RawDex * scalar), TimeSpan.FromSeconds(10)));

            // Apply skill reductions
            for (int i = 0; i < target.Skills.Length; ++i)
            {
                if (target.Skills[i].Value > 0)
                {
                    target.AddSkillMod(new DefaultSkillMod((SkillName)i, true, target.Skills[i].Value * scalar));
                }
            }

            // Set a timer to remove the effects after a duration
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                RemoveChaoticResonanceEffect(target, effect);
            });
        }

        private void RemoveChaoticResonanceEffect(Mobile target, int effect)
        {
            target.RemoveStatMod("ChaoticResonanceStr");
            target.RemoveStatMod("ChaoticResonanceInt");
            target.RemoveStatMod("ChaoticResonanceDex");

            for (int i = 0; i < target.Skills.Length; ++i)
            {
                if (target.Skills[i].Value > 0)
                {
                    target.RemoveSkillMod(new DefaultSkillMod((SkillName)i, true, target.Skills[i].Value));
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
