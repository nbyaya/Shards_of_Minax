using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class EnchantedNeedle : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Enchanted Needle", "Mendulus",
                                                        21013,
                                                        9300,
                                                        false,
                                                        Reagent.SpidersSilk,
                                                        Reagent.Garlic
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public EnchantedNeedle(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EnchantedNeedle m_Owner;

            public InternalTarget(EnchantedNeedle owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeBeneficial(target))
                    {
                        from.DoBeneficial(target);

                        // Apply healing effect
                        int healAmount = Utility.RandomMinMax(20, 40) + (int)(from.Skills[SkillName.Tailoring].Value / 5.0);

                        target.Hits += healAmount;

                        // Visual and sound effects
                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        target.PlaySound(0x1F7);

                        from.SendMessage($"You use the Enchanted Needle to heal {target.Name} for {healAmount} hit points.");

                        m_Owner.FinishSequence();
                    }
                    else
                    {
                        from.SendMessage("That target cannot be healed.");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
