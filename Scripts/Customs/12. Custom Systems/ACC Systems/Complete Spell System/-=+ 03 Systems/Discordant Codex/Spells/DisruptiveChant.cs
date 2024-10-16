using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DisruptiveChant : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disruptive Chant", "Chantus Perturbare",
            21004,
            9300,
            Reagent.BlackPearl,
            Reagent.Nightshade
        );

        public override SpellCircle Circle => SpellCircle.Second;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 40.0;
        public override int RequiredMana => 20;

        public DisruptiveChant(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private readonly DisruptiveChant m_Owner;

            public InternalTarget(DisruptiveChant owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1001016); // Cannot harm that target
                        return;
                    }

                    // Call CheckSequence on the spell instance
                    if (m_Owner.CheckSequence())
                    {
                        from.RevealingAction();
                        from.DoHarmful(target);

                        int dexReduction = (int)(target.Dex * 0.9); // Reduce 90% of target's Dexterity
                        StatMod dexMod = new StatMod(StatType.Dex, "DisruptiveChantDex", -dexReduction, TimeSpan.FromSeconds(10.0));

                        target.AddStatMod(dexMod);

                        // Visual and sound effects
                        target.FixedParticles(0x374A, 10, 15, 5029, EffectLayer.Head); // Sparkle effect
                        target.PlaySound(0x1FB); // Mysterious sound effect

                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => RemoveEffects(target, dexMod));

                        from.SendMessage("You chant disruptively, greatly hindering your target's dexterity!");
                        target.SendMessage("You feel your agility drastically reduced!");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1049545); // You must target a living creature.
                }
            }
        }

        private static void RemoveEffects(Mobile target, StatMod mod)
        {
            if (target != null && target.Alive)
            {
                target.RemoveStatMod(mod.Name);
                target.FixedEffect(0x373A, 10, 15); // Smoke-like effect upon recovery
                target.PlaySound(0x1F8); // Subtle recovery sound
                target.SendMessage("Your agility returns to normal.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
