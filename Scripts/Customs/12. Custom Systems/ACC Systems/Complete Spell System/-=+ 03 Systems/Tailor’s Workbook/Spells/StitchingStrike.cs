using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;


namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class StitchingStrike : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Stitching Strike", "Tere Stichia",
            21010, 9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public StitchingStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                // Visual and sound effects
                Caster.PlaySound(0x1F2);
                Caster.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Waist);

                SpellHelper.Turn(Caster, target);
                Caster.DoHarmful(target);

                // Calculate damage
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, Caster);

                // Apply bleeding effect with a 30% chance
                if (Utility.RandomDouble() < 0.3)
                {
                    target.SendMessage("You have been struck by the enchanted needle, causing you to bleed!");
                    target.PlaySound(0x231);
                    BleedEffect.Start(target);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private StitchingStrike m_Owner;

            public InternalTarget(StitchingStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
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

    public static class BleedEffect
    {
        public static void Start(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                Timer timer = new BleedTimer(target);
                timer.Start();
            }
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private int m_Ticks;

            public BleedTimer(Mobile target) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Ticks = 5; // Bleed effect lasts for 10 seconds
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || m_Ticks <= 0)
                {
                    Stop();
                    return;
                }

                int bleedDamage = Utility.RandomMinMax(2, 5);
                m_Target.Damage(bleedDamage);
                m_Target.SendMessage("You continue to bleed...");

                m_Ticks--;

                if (m_Ticks <= 0)
                {
                    m_Target.SendMessage("The bleeding has stopped.");
                    Stop();
                }
            }
        }
    }
}
