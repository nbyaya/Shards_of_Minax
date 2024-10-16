using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class MelodicDespair : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Melodic Despair", "Discordus Paralysis",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public MelodicDespair(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play sound and visual effect for casting
                Caster.PlaySound(0x58B);
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x376A, 1, 32, 99, 0, 0, 0
                );

                // Get targets within 4-tile radius
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(4))
                {
                    if (Caster.CanBeHarmful(m, false) && m != Caster)
                        targets.Add(m);
                }

                foreach (Mobile target in targets)
                {
                    // Apply paralysis effect
                    Caster.DoHarmful(target);
                    target.Paralyze(TimeSpan.FromSeconds(5.0)); // Paralyzes the target for 5 seconds

                    // Apply Melodic Despair effect
                    ApplyMelodicDespairEffect(Caster, target);

                    // Visual and sound effects for each target
                    target.PlaySound(0x5C2);
                    target.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Waist);
                }
            }

            FinishSequence();
        }

        private void ApplyMelodicDespairEffect(Mobile caster, Mobile target)
        {
            caster.SendLocalizedMessage(1049539); // You play the song suppressing your target's strength

            // Define the effect amount and scalar
            int effect = (int)Math.Max(-28.0, (caster.Skills[SkillName.Discordance].Value / -4.0));
            double scalar = (double)effect / 100;

            // Apply resistance reductions
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, effect));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, effect));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, effect));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, effect));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, effect));

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
                RemoveMelodicDespairEffect(target, effect);
            });
        }

        private void RemoveMelodicDespairEffect(Mobile target, int effect)
        {
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, effect));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, effect));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, effect));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, effect));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, effect));

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
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
