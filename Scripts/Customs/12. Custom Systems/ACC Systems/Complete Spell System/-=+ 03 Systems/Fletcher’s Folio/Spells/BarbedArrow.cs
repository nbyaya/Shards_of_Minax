using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class BarbedArrow : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Barbed Arrow", "Barba Booy",
            21005,
            9400,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 16; } }

        public BarbedArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                Caster.DoHarmful(target);

                // Play arrow sound and visual effects
                Effects.SendMovingEffect(Caster, target, 0xF42, 18, 1, false, false, 1167, 0); // Arrow effect
                Caster.PlaySound(0x145); // Arrow sound

                // Inflict initial damage
                int damage = Utility.RandomMinMax(10, 20);
                SpellHelper.Damage(this, target, damage, 0, 100, 0, 0, 0);

                // Apply bleeding effect
                BleedTimer bleedTimer = new BleedTimer(target, Caster);
                bleedTimer.Start();

                // Apply healing reduction effect
                target.SendMessage("You are bleeding from the barbed arrow, reducing your healing effectiveness!");
                HealingReductionTimer healingReductionTimer = new HealingReductionTimer(target);
                healingReductionTimer.Start();
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private BarbedArrow m_Owner;

            public InternalTarget(BarbedArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Caster;
            private int m_Count;

            public BleedTimer(Mobile target, Mobile caster) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Caster = caster;
                m_Count = 0;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Target.Alive && !m_Target.Deleted)
                {
                    int bleedDamage = Utility.RandomMinMax(3, 6);
                    m_Target.Damage(bleedDamage, m_Caster);
                    m_Target.SendMessage("You bleed from the barbed arrow!");

                    m_Count++;

                    if (m_Count >= 5) // Bleed effect lasts for 10 seconds (5 ticks)
                        Stop();
                }
                else
                {
                    Stop();
                }
            }
        }

        private class HealingReductionTimer : Timer
        {
            private Mobile m_Target;

            public HealingReductionTimer(Mobile target) : base(TimeSpan.FromSeconds(10.0))
            {
                m_Target = target;
                Priority = TimerPriority.OneSecond;

                m_Target.SendMessage("Your healing effectiveness is reduced!");
            }

            protected override void OnTick()
            {
                if (m_Target.Alive && !m_Target.Deleted)
                {
                    m_Target.SendMessage("The effects of the barbed arrow wear off.");
                    Stop();
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
