using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class ColdWindSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cold Wind", "Aeglens",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        private static readonly Dictionary<Mobile, ExpireTimer> m_Table = new Dictionary<Mobile, ExpireTimer>();

        public ColdWindSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedEffect(0x373A, 10, 15);
                Caster.PlaySound(0x208);

                Mobile defender = Caster.Combatant as Mobile;

                if (defender != null && defender.Map == Caster.Map && Caster.CanBeHarmful(defender) && Caster.CanSee(defender))
                {
                    Caster.DoHarmful(defender);

                    ExpireTimer timer = null;

                    if (m_Table.ContainsKey(defender))
                        timer = m_Table[defender];

                    if (timer != null)
                    {
                        timer.DoExpire();
                        defender.SendLocalizedMessage(1070831); // The freezing wind continues to blow!
                    }
                    else
                        defender.SendLocalizedMessage(1070832); // An icy wind surrounds you, freezing your lungs as you breathe!

                    timer = new ExpireTimer(defender, Caster);
                    timer.Start();
                    m_Table[defender] = timer;
                }
            }

            FinishSequence();
        }

        private class ExpireTimer : Timer
        {
            private Mobile m_Mobile;
            private Mobile m_From;
            private int m_Count;

            public ExpireTimer(Mobile m, Mobile from)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Mobile = m;
                m_From = from;
                Priority = TimerPriority.TwoFiftyMS;
            }

            public void DoExpire()
            {
                Stop();
                m_Table.Remove(m_Mobile);
            }

            public void DrainLife()
            {
                if (m_Mobile.Alive)
                    m_Mobile.Damage(2, m_From);
                else
                    DoExpire();
            }

            protected override void OnTick()
            {
                DrainLife();

                if (++m_Count >= 5)
                {
                    DoExpire();
                    m_Mobile.SendLocalizedMessage(1070830); // The icy wind dissipates.
                }
            }
        }
    }
}
