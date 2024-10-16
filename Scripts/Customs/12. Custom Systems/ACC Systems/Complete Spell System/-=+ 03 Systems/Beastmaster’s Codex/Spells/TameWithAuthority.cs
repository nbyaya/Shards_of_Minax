using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class TameWithAuthority : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tame with Authority", "Domina Animalia",
            //SpellCircle.First,
            21004,
            9300,
            false,
            Reagent.Ginseng,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public TameWithAuthority(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        public void Target(Mobile target)
        {
            if (Caster == null || target == null)
                return;

            if (target is BaseCreature creature && creature.Controlled && creature.ControlMaster == Caster)
            {
                // Apply the temporary taming skill increase
                double originalTaming = Caster.Skills[SkillName.AnimalTaming].Base;
                double tamingBonus = 20.0;

                Caster.Skills[SkillName.AnimalTaming].Base += tamingBonus;
                Caster.SendMessage("Your authority grants you greater taming ability!");

                // Visual and sound effects
                Caster.PlaySound(0x20E); // Play a taming sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, 1153, 0, EffectLayer.Waist); // Blue sparkles effect

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    // Revert the taming skill back after 30 seconds
                    Caster.Skills[SkillName.AnimalTaming].Base = originalTaming;
                    Caster.SendMessage("The effect of your authority fades.");
                    Caster.PlaySound(0x1F8); // Play a fade-out sound
                });
            }
            else
            {
                Caster.SendMessage("This spell can only be used on your controlled creatures.");
            }
        }

        private class InternalTarget : Target
        {
            private TameWithAuthority m_Owner;

            public InternalTarget(TameWithAuthority owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile mobile)
                {
                    m_Owner.Target(mobile);
                }
                else
                {
                    from.SendMessage("You must target a controlled creature.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
