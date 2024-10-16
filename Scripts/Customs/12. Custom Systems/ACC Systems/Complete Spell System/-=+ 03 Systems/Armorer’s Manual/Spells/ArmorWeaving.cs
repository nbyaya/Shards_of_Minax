using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class ArmorWeaving : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Armor Weaving", "Fortius Armatura",
                                                        //SpellCircle.Fourth,
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.BlackPearl,
                                                        Reagent.Bloodmoss,
                                                        Reagent.Garlic
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public ArmorWeaving(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (CheckBSequence(target))
            {
                if (Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Add visual and sound effects for casting
                target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                target.PlaySound(0x1F7);

                // Apply the physical resistance buff
                int duration = (int)(Caster.Skills[CastSkill].Value * 0.1); // Duration based on caster's skill
                double bonus = 0.1 + (Caster.Skills[CastSkill].Value / 100.0); // Resistance increase based on skill level

                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1044099, 1075643, TimeSpan.FromSeconds(duration), target)); // Buff icon and tooltip
                target.VirtualArmorMod += (int)(target.VirtualArmor * bonus); // Increase physical resistance

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    // Remove the buff and reset armor value
                    target.VirtualArmorMod -= (int)(target.VirtualArmor * bonus);
                    target.SendLocalizedMessage(1075612); // Armor Weaving effect has worn off.
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ArmorWeaving m_Owner;

            public InternalTarget(ArmorWeaving owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
                else
                    from.SendLocalizedMessage(500237); // Target can not be seen.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
