using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class FeintSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Feint", "Pax",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        private static Dictionary<Mobile, FeintTimer> m_Registry = new Dictionary<Mobile, FeintTimer>();
        public static Dictionary<Mobile, FeintTimer> Registry { get { return m_Registry; } }

        public FeintSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile defender = Caster.Combatant as Mobile;

                if (defender == null)
                {
                    Caster.SendLocalizedMessage(1063362); // You must have a target to use this ability!
                    return;
                }

                if (Registry.ContainsKey(Caster))
                {
                    if (Registry[Caster] != null)
                        Registry[Caster].Stop();

                    Registry.Remove(Caster);
                }

                Caster.SendLocalizedMessage(1063360); // You baffle your target with a feint!
                defender.SendLocalizedMessage(1063361); // You were deceived by an attacker's feint!

                Caster.FixedParticles(0x3728, 1, 13, 0x7F3, 0x962, 0, EffectLayer.Waist);
                Caster.PlaySound(0x525);

                double skill = Math.Max(Caster.Skills[SkillName.Ninjitsu].Value, Caster.Skills[SkillName.Bushido].Value);

                int bonus = (int)(20.0 + 3.0 * (skill - 50.0) / 7.0);

                FeintTimer t = new FeintTimer(Caster, defender, bonus);

                t.Start();
                Registry[Caster] = t;

                string args = String.Format("{0}\t{1}", defender.Name, bonus);
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Feint, 1151308, 1151307, TimeSpan.FromSeconds(6), Caster, args));
            }

            FinishSequence();
        }
    }

    public class FeintTimer : Timer
    {
        private Mobile m_Owner;
        private Mobile m_Enemy;
        private int m_DamageReduction;

        public Mobile Owner { get { return m_Owner; } }
        public Mobile Enemy { get { return m_Enemy; } }
        public int DamageReduction { get { return m_DamageReduction; } }

        public FeintTimer(Mobile owner, Mobile enemy, int damageReduction)
            : base(TimeSpan.FromSeconds(6.0))
        {
            m_Owner = owner;
            m_Enemy = enemy;
            m_DamageReduction = damageReduction;
            Priority = TimerPriority.FiftyMS;
        }

        protected override void OnTick()
        {
            FeintSpell.Registry.Remove(m_Owner);
        }
    }
}