using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Spells.Necromancy;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class BleedAttackSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Bleed Attack", "Sanguinem",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        private static Dictionary<Mobile, BleedTimer> m_BleedTable = new Dictionary<Mobile, BleedTimer>();

        public BleedAttackSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                // Necromancers under Lich or Wraith Form are immune to Bleed Attacks.
                TransformContext context = TransformationSpellHelper.GetContext(m);

                if ((context != null && (context.Type == typeof(LichFormSpell) || context.Type == typeof(WraithFormSpell))) ||
                    (m is BaseCreature && ((BaseCreature)m).BleedImmune) || Server.Spells.Mysticism.StoneFormSpell.CheckImmunity(m))
                {
                    Caster.SendLocalizedMessage(1062052); // Your target is not affected by the bleed attack!
                    return;
                }

                BeginBleed(m, Caster);
            }

            FinishSequence();
        }

        public static bool IsBleeding(Mobile m)
        {
            return m_BleedTable.ContainsKey(m);
        }

        public static void BeginBleed(Mobile m, Mobile from)
        {
            BleedTimer timer = null;

            if (m_BleedTable.ContainsKey(m))
            {
                return;
            }

            BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bleed, 1075829, 1075830, TimeSpan.FromSeconds(10), m, String.Format("{0}\t{1}\t{2}", "1", "10", "2")));

            timer = new BleedTimer(from, m, CheckBloodDrink(from));
            m_BleedTable[m] = timer;
            timer.Start();

            from.SendLocalizedMessage(1060159); // Your target is bleeding!
            m.SendLocalizedMessage(1060160); // You are bleeding!

            if (m is PlayerMobile)
            {
                m.LocalOverheadMessage(MessageType.Regular, 0x21, 1060757); // You are bleeding profusely
                m.NonlocalOverheadMessage(MessageType.Regular, 0x21, 1060758, m.Name); // ~1_NAME~ is bleeding profusely
            }

            m.PlaySound(0x133);
            m.FixedParticles(0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist);
        }

        public static void DoBleed(Mobile m, Mobile from, int damage, bool blooddrinker)
        {
            if (m.Alive && !m.IsDeadBondedPet)
            {
                if (!m.Player)
                    damage *= 2;

                m.PlaySound(0x133);
                AOS.Damage(m, from, damage, false, 0, 0, 0, 0, 0, 0, 100, false, false, false);

                if (blooddrinker && from.Hits < from.HitsMax)
                {
                    from.SendLocalizedMessage(1113606); //The blood drinker effect heals you.
                    from.Heal(damage);
                }

                Blood blood = new Blood();
                blood.ItemID = Utility.Random(0x122A, 5);
                blood.MoveToWorld(m.Location, m.Map);
            }
            else
            {
                EndBleed(m, false);
            }
        }

        public static void EndBleed(Mobile m, bool message)
        {
            Timer t = null;

            if (m_BleedTable.ContainsKey(m))
            {
                t = m_BleedTable[m];
                m_BleedTable.Remove(m);
            }

            if (t == null)
                return;

            t.Stop();
            BuffInfo.RemoveBuff(m, BuffIcon.Bleed);

            if (message)
                m.SendLocalizedMessage(1060167); // The bleeding wounds have healed, you are no longer bleeding!
        }

        public static bool CheckBloodDrink(Mobile attacker)
        {
            return attacker.Weapon is BaseWeapon && ((BaseWeapon)attacker.Weapon).WeaponAttributes.BloodDrinker > 0;
        }

        private class BleedTimer : Timer
        {
            private Mobile m_From;
            private Mobile m_Mobile;
            private int m_Count;
            private int m_MaxCount;
            private bool m_BloodDrinker;

            public BleedTimer(Mobile from, Mobile m, bool blooddrinker)
                : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_From = from;
                m_Mobile = m;
                Priority = TimerPriority.TwoFiftyMS;
                m_BloodDrinker = blooddrinker;

                m_MaxCount = Spells.SkillMasteries.SkillMasterySpell.UnderPartyEffects(m, typeof(Spells.SkillMasteries.ResilienceSpell)) ? 3 : 5;
            }

            protected override void OnTick()
            {
                if (!m_Mobile.Alive || m_Mobile.Deleted)
                {
                    EndBleed(m_Mobile, true);
                }
                else
                {
                    int damage = 0;

                    if (!Server.Spells.SkillMasteries.WhiteTigerFormSpell.HasBleedMod(m_From, out damage))
                        damage = Math.Max(1, Utility.RandomMinMax(5 - m_Count, (5 - m_Count) * 2));

                    DoBleed(m_Mobile, m_From, damage, m_BloodDrinker);

                    if (++m_Count == m_MaxCount)
                        EndBleed(m_Mobile, true);
                }
            }
        }

        private class InternalTarget : Target
        {
            private BleedAttackSpell m_Owner;

            public InternalTarget(BleedAttackSpell owner)
                : base(12, false, TargetFlags.Harmful)
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
    }
}