using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class TranquilStrike : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tranquil Strike", "Calma Nex",
            // SpellCircle.Fourth,
            21004,
            9300,
            false,
            Reagent.Ginseng,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public TranquilStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckHSequence(target))
            {
                if (target is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)target;
                    SpellHelper.Turn(Caster, target);

                    // Visual and sound effects
                    Effects.SendTargetParticles(target, 0x374A, 10, 30, 5052, EffectLayer.Waist);
                    Effects.PlaySound(target.Location, target.Map, 0x1F2);

                    // Apply stun effect
                    target.Freeze(TimeSpan.FromSeconds(3.0)); // Stun for 3 seconds

                    // Reduce damage output temporarily
                    creature.DamageMin -= 5;
                    creature.DamageMax -= 5;
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        creature.DamageMin += 5;
                        creature.DamageMax += 5;
                    });

                    // Notify caster and target
                    Caster.SendMessage("You channel a wave of calm, temporarily stunning your target and reducing their damage output.");
                    target.SendMessage("You feel a wave of calm wash over you, stunning you and weakening your attacks.");
                }
                else
                {
                    Caster.SendMessage("This spell only works on creatures.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private TranquilStrike m_Owner;

            public InternalTarget(TranquilStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
