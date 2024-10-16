using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Revitalize : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Revitalize", "Sanius Vivo",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public Revitalize(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Revitalize m_Spell;

            public InternalTarget(Revitalize spell) : base(10, false, TargetFlags.Beneficial)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile targetMobile)
                {
                    if (!from.CanBeBeneficial(targetMobile))
                    {
                        from.SendLocalizedMessage(1060508); // You cannot bless that target.
                        return;
                    }

                    if (m_Spell.CheckSequence())
                    {
                        from.DoBeneficial(targetMobile);

                        Effects.SendTargetParticles(targetMobile, 0x375A, 10, 30, 5052, EffectLayer.Waist); // Particle Effect
                        targetMobile.PlaySound(0x1F2); // Sound Effect

                        double duration = 20.0 + (from.Skills[m_Spell.CastSkill].Value * 0.2);
                        double staminaBoost = 20.0 + (from.Skills[m_Spell.CastSkill].Value * 0.1);

                        Timer.DelayCall(TimeSpan.FromSeconds(duration), () => RemoveBuff(targetMobile, staminaBoost));
                        ApplyBuff(targetMobile, staminaBoost);

                        from.SendMessage("You revitalize the target, temporarily boosting its stamina!");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500332); // You must target a creature.
                }

                m_Spell.FinishSequence();
            }
        }

        private static void ApplyBuff(Mobile target, double boostAmount)
        {
            if (target is BaseCreature creature)
            {
                creature.Stam += (int)boostAmount;
                creature.SendMessage("Your stamina has been temporarily boosted!");

                creature.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Additional Particle Effect
            }
        }

        private static void RemoveBuff(Mobile target, double boostAmount)
        {
            if (target is BaseCreature creature)
            {
                creature.Stam -= (int)boostAmount;
                creature.SendMessage("The revitalizing effect has worn off.");

                creature.FixedParticles(0x374A, 10, 15, 5023, EffectLayer.Waist); // Fading Particle Effect
                creature.PlaySound(0x1F8); // Fading Sound Effect
            }
        }
    }
}
