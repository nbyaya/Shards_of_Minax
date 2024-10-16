using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class MortalStrikeSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mortal Strike", "Mortalus",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public MortalStrikeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public static readonly TimeSpan PlayerDuration = TimeSpan.FromSeconds(6.0);
        public static readonly TimeSpan NPCDuration = TimeSpan.FromSeconds(12.0);

        private static readonly Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();
        private static readonly List<Mobile> m_EffectReduction = new List<Mobile>();

        public static bool IsWounded(Mobile m)
        {
            return m_Table.ContainsKey(m);
        }

        public static void BeginWound(Mobile m, TimeSpan duration)
        {
            Timer t;

            if (m_Table.ContainsKey(m))
            {
                EndWound(m, true);
            }

            if (m_EffectReduction.Contains(m))
            {
                double d = duration.TotalSeconds;
                duration = TimeSpan.FromSeconds(d / 2);
            }

            t = new InternalTimer(m, duration);
            m_Table[m] = t;

            t.Start();

            m.YellowHealthbar = true;
            BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.MortalStrike, 1075810, 1075811, duration, m));
        }

        public static void EndWound(Mobile m, bool natural)
        {
            if (!IsWounded(m))
                return;

            Timer t = m_Table[m];

            if (t != null)
                t.Stop();

            m_Table.Remove(m);

            BuffInfo.RemoveBuff(m, BuffIcon.MortalStrike);

            m.YellowHealthbar = false;
            m.SendLocalizedMessage(1060208); // You are no longer mortally wounded.

            if (natural && !m_EffectReduction.Contains(m))
            {
                m_EffectReduction.Add(m);

                Timer.DelayCall(TimeSpan.FromSeconds(8), delegate
                {
                    if (m_EffectReduction.Contains(m))
                        m_EffectReduction.Remove(m);
                });
            }
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (CheckSequence())
            {
                Caster.FixedEffect(0x3728, 10, 15);
                Caster.PlaySound(0x2A1);

                if (Caster.CanBeHarmful(target))
                {
                    Caster.DoHarmful(target);

                    if (CheckMana())
                    {
                        Caster.SendLocalizedMessage(1060086); // You deliver a mortal wound!
                        target.SendLocalizedMessage(1060087); // You have been mortally wounded!

                        target.PlaySound(0x1E1);
                        target.FixedParticles(0x37B9, 244, 25, 9944, 31, 0, EffectLayer.Waist);

                        if (!IsWounded(target))
                        {
                            BeginWound(target, target.Player ? PlayerDuration : NPCDuration);
                        }
                    }
                }
                else
                {
                    Caster.SendLocalizedMessage(1060161); // The spell fizzles.
                }
            }

            FinishSequence();
        }

        private bool CheckMana()
        {
            if (Caster.Mana >= RequiredMana)
            {
                Caster.Mana -= RequiredMana;
                return true;
            }
            else
            {
                Caster.SendLocalizedMessage(1060179); // You lack the mana required to cast this spell.
                return false;
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile;
            public InternalTimer(Mobile m, TimeSpan duration)
                : base(duration)
            {
                m_Mobile = m;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                EndWound(m_Mobile, true);
            }
        }

        private class InternalTarget : Target
        {
            private MortalStrikeSpell m_Owner;

            public InternalTarget(MortalStrikeSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    m_Owner.Target((Mobile)targeted);
                }
                else
                {
                    from.SendLocalizedMessage(1060161); // The spell fizzles.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
