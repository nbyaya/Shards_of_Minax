using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class SneakAttack : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sneak Attack", "Backstab!",
            21011, 9210 // Remove School.SilentShadowsJournal if not required
        );

        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }
        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; } // Example value; adjust as needed
        }

        public SneakAttack(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden)
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                Caster.SendMessage("You must be hidden to perform a Sneak Attack.");
                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private SneakAttack m_Owner;

            public InternalTarget(SneakAttack owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.Hidden && m_Owner.CheckSequence())
                    {
                        from.RevealingAction();
                        target.FixedParticles(0x37B9, 10, 30, 5052, EffectLayer.Head);
                        target.PlaySound(0x1F1);

                        double bypassChance = 0.25 + (from.Skills[SkillName.Stealth].Value / 200.0);
                        double damageBonus = 0.25;

                        if (Utility.RandomDouble() < bypassChance)
                        {
                            target.SendMessage("Your armor is bypassed!");
                            target.Damage((int)(from.Str * damageBonus), from);
                        }
                        else
                        {
                            target.Damage((int)(from.Str * (1.0 + damageBonus)), from);
                        }
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
