using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class SealOfInvisibility : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Seal of Invisibility", "Vanish", // Spell name and command phrase
            21004, // Gump ID
            9300,  // Sound ID
            false
        );

        public override SpellCircle Circle => SpellCircle.Sixth; // Adjust the circle as needed

        public override double CastDelay => 0.1; // 1-second cast delay
        public override double RequiredSkill => 75.0; // Required skill level
        public override int RequiredMana => 35; // Mana cost

        public SealOfInvisibility(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SealOfInvisibility m_Owner;

            public InternalTarget(SealOfInvisibility owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.ApplyEffect(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private void ApplyEffect(Mobile target)
        {
            if (target == null || target.Deleted || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (!Caster.CanBeBeneficial(target, false, true))
            {
                Caster.SendLocalizedMessage(1060508); // You cannot perform beneficial acts on that target.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoBeneficial(target);
                SpellHelper.Turn(Caster, target);

                // Visual effect and sound
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x375A, 10, 15, 5023);
                target.PlaySound(0x1FD);

                // Apply invisibility effect
                target.Hidden = true;

                // Timer to remove invisibility
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => RemoveEffect(target));
            }

            FinishSequence();
        }

        private void RemoveEffect(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            target.Hidden = false;

            // Visual effect when the invisibility wears off
            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x374A, 10, 15, 5023);
            target.PlaySound(0x1FE); // Sound effect when becoming visible again
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Cast delay for balance
        }
    }
}
