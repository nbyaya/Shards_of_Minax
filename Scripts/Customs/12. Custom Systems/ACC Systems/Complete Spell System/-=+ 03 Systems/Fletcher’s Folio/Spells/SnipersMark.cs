using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class SnipersMark : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sniper's Mark", "Ex Penitua",
            21005,
            9400,
            false
        );

        private static readonly Dictionary<Mobile, bool> sniperMarks = new Dictionary<Mobile, bool>();

        public override SpellCircle Circle => SpellCircle.Fifth;
        public override double CastDelay => 1.5;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 18;

        public SnipersMark(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SnipersMark m_Owner;

            public InternalTarget(SnipersMark owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.MarkTarget(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }
        }

        public void MarkTarget(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (SpellHelper.CheckTown(target, Caster) && CheckSequence())
            {
                Caster.DoHarmful(target);
                SpellHelper.Turn(Caster, target);

                target.SendMessage("You have been marked by Sniper's Mark!");
                target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                target.PlaySound(0x208);

                SetSnipersMarkStatus(target, true);
                new InternalTimer(target).Start();
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Target;

            public InternalTimer(Mobile target) : base(TimeSpan.FromSeconds(20.0))
            {
                m_Target = target;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                SetSnipersMarkStatus(m_Target, false);
            }
        }

        private static void SetSnipersMarkStatus(Mobile mobile, bool status)
        {
            if (sniperMarks.ContainsKey(mobile))
            {
                sniperMarks[mobile] = status;
            }
            else
            {
                sniperMarks.Add(mobile, status);
            }
        }

        private static bool IsMarkedBySnipersMark(Mobile mobile)
        {
            return sniperMarks.ContainsKey(mobile) && sniperMarks[mobile];
        }

        public static bool IsMarked(Mobile m)
        {
            return IsMarkedBySnipersMark(m);
        }

        public static void ApplyDamageBonus(Mobile attacker, Mobile defender, ref int damage)
        {
            if (IsMarked(defender))
            {
                damage = (int)(damage * 1.25); // Increases damage by 25%
            }
        }
    }
}
