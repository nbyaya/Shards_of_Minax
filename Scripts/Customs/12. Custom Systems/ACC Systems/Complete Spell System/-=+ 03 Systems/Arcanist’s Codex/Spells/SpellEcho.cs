using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class SpellEcho : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Spell Echo", "Red Energy",
            21005, // Spell icon graphic ID
            9415   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public SpellEcho(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SpellEcho m_Owner;

            public InternalTarget(SpellEcho owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CheckHSequence(target))
                    {
                        // Visual and sound effects
                        Effects.SendMovingEffect(from, target, 0x36D4, 7, 0, false, false, 1161, 0);
                        target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Head);
                        target.PlaySound(0x208);

                        // Calculate damage based on target's mana
                        int damage = target.Mana;

                        // Inflict damage
                        SpellHelper.Damage(m_Owner, target, damage, 0, 100, 0, 0, 0);

                        // Reduce caster's mana cost
                        m_Owner.Caster.Mana -= m_Owner.RequiredMana;
                    }
                }
                else
                {
                    from.SendMessage("You must target a valid mobile.");
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
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
