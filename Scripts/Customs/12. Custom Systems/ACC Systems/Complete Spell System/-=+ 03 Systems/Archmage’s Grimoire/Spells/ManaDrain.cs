using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class ManaDrain : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mana Drain", "Ex Manus",
            21004, 9300,
            false
        );

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override double CastDelay => 1.5; // 1.5 seconds cast delay
        public override double RequiredSkill => 60.0; // 60% magery skill required
        public override int RequiredMana => 20; // 20 mana required to cast

        public ManaDrain(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ManaDrain m_Owner;

            public InternalTarget(ManaDrain owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile m = (Mobile)target;

                    if (!from.CanBeHarmful(m))
                    {
                        from.SendLocalizedMessage(1001011); // You can't do that.
                        return;
                    }

                    if (m == from)
                    {
                        from.SendLocalizedMessage(501048); // You cannot cast this on yourself.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(m);

                        int manaDrained = (int)(m.Mana * 0.5); // 50% of the target's mana
                        manaDrained = Math.Min(manaDrained, from.ManaMax - from.Mana); // Ensure caster does not exceed max mana

                        m.Mana -= manaDrained;
                        from.Mana += manaDrained;

                        // Visual effects
                        m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Effect on the target
                        from.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Effect on the caster
                        m.PlaySound(0x208); // Drain sound

                        // Flashy sound and additional visual effect
                        Effects.SendBoltEffect(m, true); // Lightning effect
                        m.PlaySound(0x29); // Additional sound effect for drama

                        from.SendMessage($"You drain {manaDrained} mana from {m.Name}.");
                        m.SendMessage($"You feel {manaDrained} mana drained from you!");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501857); // Invalid target.
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
