using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class SanctuaryField : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Sanctuary Field", "An Sanct",
                                                        21004, // Animation ID for casting
                                                        9300,  // Effect sound ID
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public SanctuaryField(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SanctuaryField m_Owner;

            public InternalTarget(SanctuaryField owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }
        }

        public void Target(Mobile target)
        {
            if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Visual and sound effects for casting
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Effects.PlaySound(target.Location, target.Map, 0x20F);

                // Apply the armor buff
                int armorBonus = 10 + (int)(Caster.Skills[CastSkill].Value * 0.1);
                double duration = 10.0 + (Caster.Skills[CastSkill].Value * 0.1);

                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Protection, 1075810, TimeSpan.FromSeconds(duration), Caster, $"+{armorBonus} Armor"));

                target.SendMessage("A protective aura surrounds you, enhancing your armor!");

                // Start the timer to remove the buff
                new BuffTimer(target, armorBonus, TimeSpan.FromSeconds(duration)).Start();
            }

            FinishSequence();
        }

        private class BuffTimer : Timer
        {
            private Mobile m_Target;
            private int m_ArmorBonus;

            public BuffTimer(Mobile target, int armorBonus, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                m_ArmorBonus = armorBonus;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Target.Alive)
                {
                    // Remove the armor buff
                    BuffInfo.RemoveBuff(m_Target, BuffIcon.Protection);
                    m_Target.SendMessage("The protective aura fades away.");
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
