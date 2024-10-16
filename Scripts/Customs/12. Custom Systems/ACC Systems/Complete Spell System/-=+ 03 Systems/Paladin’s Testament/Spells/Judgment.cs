using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class Judgment : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Judgment", "Divinum Fulmen",
            21013,
            9300,
            false,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 30; } }

        public Judgment(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Judgment m_Owner;

            public InternalTarget(Judgment owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeHarmful(target))
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendMessage("You cannot target that.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanBeHarmful(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be harmed.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoHarmful(target);

                // Deal heavy damage
                int damage = Utility.RandomMinMax(40, 60);
                SpellHelper.Damage(TimeSpan.FromSeconds(0.5), target, Caster, damage, 0, 0, 100, 0, 0);

                // Apply stunning effect
                TimeSpan stunDuration = TimeSpan.FromSeconds(3.0);
                target.Freeze(stunDuration);

                // Visual and sound effects for the lightning bolt
                target.BoltEffect(0);
                Effects.SendLocationEffect(target.Location, target.Map, 0x37CC, 30, 10, 1153, 0);
                Effects.PlaySound(target.Location, target.Map, 0x29);

                Caster.SendMessage("You call down a bolt of divine lightning, smiting your foe with heavenly fury!");
                target.SendMessage("You are struck by divine lightning and stunned!");
            }

            FinishSequence();
        }
    }
}
