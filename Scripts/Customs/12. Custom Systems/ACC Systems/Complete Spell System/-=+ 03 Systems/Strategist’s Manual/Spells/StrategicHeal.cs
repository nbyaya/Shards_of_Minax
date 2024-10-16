using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class StrategicHeal : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Strategic Heal", "Uus Mani Resisto",
            21004,
            9300,
            false,
            Reagent.Ginseng,
            Reagent.Garlic
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public StrategicHeal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private StrategicHeal m_Owner;

            public InternalTarget(StrategicHeal owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (target.Alive && from.CanBeBeneficial(target, true))
                    {
                        from.DoBeneficial(target);

                        if (m_Owner.CheckSequence())
                        {
                            // Heal Effect
                            int healAmount = (int)(from.Skills[SkillName.Magery].Value / 5.0 + Utility.RandomMinMax(10, 20));
                            target.Heal(healAmount);

                            // Resistance Buff
                            double duration = 10.0 + (from.Skills[SkillName.Magery].Value / 5.0);
                            BuffResistance(target, duration);

                            // Visual and Sound Effects
                            Effects.PlaySound(target.Location, target.Map, 0x213);
                            Effects.SendTargetParticles(target, 0x376A, 9, 32, 5008, EffectLayer.Waist);

                            from.SendMessage("You strategically heal and enhance your ally's resistances!");
                            target.SendMessage("You feel rejuvenated and your resistances are strengthened!");
                        }
                    }
                }

                m_Owner.FinishSequence();
            }

            private void BuffResistance(Mobile target, double duration)
            {


                Timer.DelayCall(TimeSpan.FromSeconds(duration), () => 
                {
                    // Revert resistances
                    target.SendMessage("Your resistance enhancement fades away.");
                });
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
