using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class CalmingPresence : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Calming Presence", "In Zan Calm",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public CalmingPresence(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x658); // Sound effect for spell casting
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022); // Visual effect

                foreach (Mobile mob in Caster.GetMobilesInRange(5)) // Radius of effect
                {
                    if (mob != Caster && mob.Combatant == Caster && mob.Alive && Caster.CanBeHarmful(mob, false))
                    {
                        Caster.DoHarmful(mob);

                        mob.SendMessage("You feel a calming presence soothing your anger.");
                        mob.AggressiveAction(Caster);

                        mob.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist); // Visual effect on the target
                        mob.PlaySound(0x1F2); // Sound effect for pacification

                        // Temporarily reduce aggression chance
                        new AggressionReductionTimer(mob).Start();
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private CalmingPresence m_Owner;

            public InternalTarget(CalmingPresence owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class AggressionReductionTimer : Timer
        {
            private Mobile m_Target;

            public AggressionReductionTimer(Mobile target) : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                if (Utility.RandomDouble() < 0.05) // 5% chance per tick to resume aggression
                {
                    m_Target.Aggressors.Clear();
                    m_Target.SendMessage("Your pacification wears off, and your aggression returns.");
                    Stop();
                }
            }
        }
    }
}
