using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealingShield : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Healing Shield", "Protectus Vitalis",
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
        public override int RequiredMana { get { return 25; } }

        public HealingShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (m == null || m.Deleted || !Caster.CanSee(m) || !Caster.InRange(m, 10))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen or is out of range.
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, m);

                Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x373A, 1, 29, 1153, 4, 0, 0);
                Effects.PlaySound(m.Location, m.Map, 0x1F7);

                Caster.SendMessage("A protective shield surrounds " + m.Name + ", absorbing damage and converting it into healing!");

                m.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist);
                m.PlaySound(0x1F8);

                int shieldStrength = (int)(Caster.Skills[CastSkill].Value * 0.2); // Shield absorbs up to 20% of caster's skill level as damage

                new HealingShieldTimer(m, Caster, TimeSpan.FromSeconds(10.0), shieldStrength).Start(); // Shield lasts for 10 seconds
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HealingShield m_Owner;

            public InternalTarget(HealingShield owner) : base(12, false, TargetFlags.Beneficial)
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

        private class HealingShieldTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Caster;
            private int m_ShieldStrength;
            private DateTime m_EndTime;
            private static Dictionary<Mobile, int> m_AbsorbedDamage = new Dictionary<Mobile, int>();

            public HealingShieldTimer(Mobile target, Mobile caster, TimeSpan duration, int shieldStrength) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_Caster = caster;
                m_ShieldStrength = shieldStrength;
                m_EndTime = DateTime.Now + duration;

                // Initialize absorbed damage for the target
                if (!m_AbsorbedDamage.ContainsKey(m_Target))
                    m_AbsorbedDamage[m_Target] = 0;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || DateTime.Now >= m_EndTime)
                {
                    Stop();
                    m_Target.SendMessage("The Healing Shield fades away.");
                    if (m_AbsorbedDamage.ContainsKey(m_Target))
                        m_AbsorbedDamage.Remove(m_Target);
                    return;
                }

                if (m_ShieldStrength > 0)
                {
                    // Check if the target has taken damage
                    int absorbed = m_AbsorbedDamage[m_Target];
                    int damageTaken = absorbed - m_ShieldStrength;

                    if (damageTaken > 0)
                    {
                        int healingAmount = damageTaken / 2; // Convert 50% of absorbed damage into healing
                        m_Target.Heal(healingAmount);
                        m_Target.FixedEffect(0x376A, 10, 16);
                        m_Target.SendMessage("The shield absorbs the damage and heals you for " + healingAmount + " health!");

                        m_ShieldStrength -= damageTaken;
                    }
                }
            }

            public static void OnDamageTaken(Mobile target, int amount)
            {
                if (m_AbsorbedDamage.ContainsKey(target))
                {
                    m_AbsorbedDamage[target] += amount;
                }
            }
        }
    }
}
