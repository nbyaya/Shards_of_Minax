using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class Impale : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Impale", "Impale!",
            21002,
            9401
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public Impale(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster == null || target == null)
                return;

            if (!Caster.CanBeHarmful(target))
            {
                Caster.SendLocalizedMessage(1005463); // You cannot attack that.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoHarmful(target);

                // Play flashy visual effects and sound
                Caster.PlaySound(0x1F5);
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);

                // Calculate damage that bypasses armor
                int damage = Utility.RandomMinMax(30, 45); // Base damage range
                double ignoreArmorFactor = 0.5; // Ignore 50% of the armor

                damage += (int)(Caster.Skills[SkillName.Swords].Value / 5); // Bonus damage based on skill

                int ignoreArmor = (int)(target.ArmorRating * ignoreArmorFactor);
                int finalDamage = damage - ignoreArmor;

                if (finalDamage < 0)
                    finalDamage = 0;

                AOS.Damage(target, Caster, finalDamage, 100, 0, 0, 0, 0); // Physical damage bypassing armor

                // Apply bleeding effect
                if (Utility.RandomDouble() < 0.25) // 25% chance to apply bleeding
                {
                    target.PlaySound(0x1C3);
                    target.SendMessage("You are bleeding from the wound!");
                    BleedEffect.Start(target);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Impale m_Owner;

            public InternalTarget(Impale owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public class BleedEffect
    {
        public static void Start(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            Timer bleedTimer = new BleedTimer(target);
            bleedTimer.Start();
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private int m_Ticks;

            public BleedTimer(Mobile target) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Ticks = 5; // Bleed for 5 ticks (10 seconds total)
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                m_Target.PlaySound(0x133);
                m_Target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                AOS.Damage(m_Target, m_Target, 5, 100, 0, 0, 0, 0); // Bleed damage

                if (--m_Ticks <= 0)
                    Stop();
            }
        }
    }
}
