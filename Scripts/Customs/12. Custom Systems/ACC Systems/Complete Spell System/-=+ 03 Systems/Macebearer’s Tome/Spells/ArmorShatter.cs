using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class ArmorShatter : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Armor Shatter", "Armor Shatter",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 30; } }

        public ArmorShatter(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(target))
            {
                SpellHelper.Turn(Caster, target);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x2F4); // Play a sound effect for the strike

                // Visual effect to show a powerful strike
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);

                // Apply armor reduction
                int armorReduction = 10 + (int)(Caster.Skills[SkillName.Macing].Value / 10); // Example formula
                TimeSpan duration = TimeSpan.FromSeconds(10); // Duration of armor reduction

                target.SendMessage("Your armor has been shattered, reducing its effectiveness!");

                new ArmorReductionTimer(target, armorReduction, duration).Start();

                // Animate the caster to show a powerful attack
                Caster.Animate(31, 5, 1, true, false, 0);
            }

            FinishSequence();
        }

        private class ArmorReductionTimer : Timer
        {
            private Mobile m_Target;
            private int m_ArmorReduction;
            private DateTime m_End;

            public ArmorReductionTimer(Mobile target, int armorReduction, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_ArmorReduction = armorReduction;
                m_End = DateTime.Now + duration;

                target.VirtualArmorMod -= armorReduction;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_End)
                {
                    m_Target.VirtualArmorMod += m_ArmorReduction;
                    m_Target.SendMessage("Your armor's effectiveness has been restored.");
                    Stop();
                }
            }
        }

        private class InternalTarget : Target
        {
            private ArmorShatter m_Owner;

            public InternalTarget(ArmorShatter owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
                else
                {
                    from.SendLocalizedMessage(1009094); // That is not a valid target.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
