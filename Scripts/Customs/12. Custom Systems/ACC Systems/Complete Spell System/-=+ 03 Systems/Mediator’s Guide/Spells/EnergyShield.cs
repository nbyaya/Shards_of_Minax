using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections.Generic;
using Server;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class EnergyShield : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Energy Shield", "Vas An Lor",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public EnergyShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (SpellHelper.CheckTown(target, Caster) && CheckSequence())
            {
                double duration = 10.0 + (Caster.Skills[CastSkill].Value * 0.1);
                int resistanceBonus = 20; // Increase resistances by 20%

                if (this.Scroll != null)
                    Scroll.Consume();

                // Create visual effects
                Effects.SendTargetParticles(target, 0x375A, 10, 15, 5021, EffectLayer.Waist);
                target.PlaySound(0x28E);

                // Apply resistance effect
                ResistanceEffect resistanceEffect = new ResistanceEffect(target, resistanceBonus, TimeSpan.FromSeconds(duration));
                resistanceEffect.Start();

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private EnergyShield m_Owner;

            public InternalTarget(EnergyShield owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class ResistanceEffect
        {
            private Mobile m_Target;
            private int m_ResistanceBonus;
            private Timer m_Timer;

            public ResistanceEffect(Mobile target, int resistanceBonus, TimeSpan duration)
            {
                m_Target = target;
                m_ResistanceBonus = resistanceBonus;
                m_Timer = new ResistanceTimer(target, duration, this);
            }

            public void Start()
            {
                // Apply resistance bonuses
                m_Target.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, m_ResistanceBonus));
                m_Target.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, m_ResistanceBonus));
                m_Target.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, m_ResistanceBonus));
                m_Target.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, m_ResistanceBonus));
                m_Target.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, m_ResistanceBonus));

                m_Timer.Start();
            }

            public void Stop()
            {
                // Remove resistance bonuses
                m_Target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, m_ResistanceBonus));
                m_Target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, m_ResistanceBonus));
                m_Target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, m_ResistanceBonus));
                m_Target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, m_ResistanceBonus));
                m_Target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, m_ResistanceBonus));

                m_Target.SendMessage("Your energy shield fades away.");
                Effects.SendTargetParticles(m_Target, 0x3779, 10, 15, 5021, EffectLayer.Waist);
                m_Target.PlaySound(0x1F8);
            }

            private class ResistanceTimer : Timer
            {
                private Mobile m_Target;
                private ResistanceEffect m_Effect;

                public ResistanceTimer(Mobile target, TimeSpan duration, ResistanceEffect effect) : base(duration)
                {
                    m_Target = target;
                    m_Effect = effect;
                }

                protected override void OnTick()
                {
                    m_Effect.Stop();
                }
            }
        }
    }
}
