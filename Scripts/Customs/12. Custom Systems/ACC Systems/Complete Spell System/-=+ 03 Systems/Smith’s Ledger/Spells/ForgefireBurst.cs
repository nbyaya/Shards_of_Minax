using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Engines;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class ForgefireBurst : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forgefire Burst", "Blast!",
            21007, // Icon ID
            9303,  // Cast animation
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 25; } }

        public ForgefireBurst(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ForgefireBurst m_Owner;

            public InternalTarget(ForgefireBurst owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile targetMobile = (Mobile)target;

                    if (from.CanBeHarmful(targetMobile) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(targetMobile);

                        double damage = Utility.RandomMinMax(15, 25) + (from.Skills[SkillName.Blacksmith].Value / 5.0);
                        double stunChance = 0.2 + (from.Skills[SkillName.Blacksmith].Value / 500.0); // Base 20% chance, increases with skill

                        targetMobile.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                        targetMobile.PlaySound(0x211);

                        // Deal damage
                        AOS.Damage(targetMobile, from, (int)damage, 100, 0, 0, 0, 0);

                        // Attempt to stun
                        if (Utility.RandomDouble() < stunChance)
                        {
                            TimeSpan stunDuration = TimeSpan.FromSeconds(1.5 + (from.Skills[SkillName.Blacksmith].Value / 100.0)); // Base 1.5 seconds, increases with skill
                            targetMobile.Paralyze(stunDuration);
                            targetMobile.SendMessage("You are stunned by the powerful hammer strike!");
                            from.SendMessage("Your hammer strike stuns your target!");
                        }
                    }
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
