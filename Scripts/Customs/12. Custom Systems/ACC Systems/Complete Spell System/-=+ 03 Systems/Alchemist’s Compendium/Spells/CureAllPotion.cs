using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class CureAllPotion : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Cure-All Potion", "Cure Various Status Effects",
                                                        //SpellCircle.First,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 25; } }

        public CureAllPotion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CureAllPotion m_Owner;

            public InternalTarget(CureAllPotion owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeBeneficial(target))
                    {
                        from.DoBeneficial(target);
                        m_Owner.ApplyEffects(target);
                    }
                    else
                    {
                        from.SendLocalizedMessage(1060239); // You cannot target that.
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        private void ApplyEffects(Mobile target)
        {
            if (CheckSequence())
            {
                // Cure Poison

                target.CurePoison(Caster);
                target.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist);
                target.PlaySound(0x1E0);


                // Remove Stun (if applicable)
                if (target.Paralyzed)
                {
                    target.Paralyzed = false;
                    target.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }

                // Additional visual effect to show a successful cure
                target.FixedEffect(0x373A, 10, 15);
                target.SendMessage("You feel a powerful wave of relief as your ailments are cured!");
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
