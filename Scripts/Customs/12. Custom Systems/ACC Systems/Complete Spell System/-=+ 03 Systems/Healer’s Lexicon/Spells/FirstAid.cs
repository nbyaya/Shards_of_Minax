using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class FirstAid : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "First Aid", "In Vas Mini",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public FirstAid(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FirstAid m_Owner;

            public InternalTarget(FirstAid owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);
                        m_Owner.Caster.Mana -= m_Owner.RequiredMana;

                        Effects.SendTargetParticles(target, 0x376A, 1, 30, 1153, EffectLayer.Waist); // Healing sparkle effect
                        target.PlaySound(0x1F2); // Healing sound effect

                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 5, () =>
                        {
                            if (target.Alive && !target.Deleted)
                            {
                                int healAmount = Utility.RandomMinMax(3, 6); // Heal for a small random amount each tick
                                target.Hits += healAmount;
                                target.FixedParticles(0x376A, 1, 30, 1153, EffectLayer.Waist);
                                target.PlaySound(0x1F2);
                            }
                        });
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendMessage("You can only use this ability on a living target.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
