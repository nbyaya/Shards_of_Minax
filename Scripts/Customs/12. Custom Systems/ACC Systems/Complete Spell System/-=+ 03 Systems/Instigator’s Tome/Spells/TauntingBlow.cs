using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class TauntingBlow : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Taunting Blow", "Provocare Ferrum",
            21005, 9400 // Spell icon and effect
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public TauntingBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanBeHarmful(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Play visual and sound effects
                Effects.SendTargetEffect(target, 0x3709, 10, 30, 1160, 0); // Fire flash
                Effects.PlaySound(target.Location, target.Map, 0x3B7); // War cry sound

                // Apply taunt effect
                target.SendMessage("You have been provoked to attack " + Caster.Name + "!");
                target.Combatant = Caster; // Forces the target to focus attacks on the caster
                target.AggressiveAction(Caster); // Marks the caster as an aggressor for the target

                // Set a timer for the duration of the taunt effect
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                {
                    // After 5 seconds, reset the target's combatant if the target is still focused on the caster
                    if (target.Combatant == Caster)
                    {
                        target.Combatant = null;
                        target.SendMessage("You feel your anger subside.");
                    }
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private TauntingBlow m_Owner;

            public InternalTarget(TauntingBlow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
