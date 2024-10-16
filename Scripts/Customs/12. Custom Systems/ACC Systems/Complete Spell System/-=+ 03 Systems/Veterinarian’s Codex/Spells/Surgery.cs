using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Surgery : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Surgery", "Healing Incision",
            21005, // Animation ID for the spell
            9301 // Sound ID for the spell
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } } // Slight delay for surgery operation
        public override double RequiredSkill { get { return 60.0; } } // Required skill level
        public override int RequiredMana { get { return 20; } } // Mana cost for casting the spell

        public Surgery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Visual and sound effects
                Effects.SendTargetParticles(target, 0x376A, 10, 15, 5013, EffectLayer.Waist);
                target.PlaySound(9301);

                // Apply healing and long-term benefits
                int healAmount = Utility.RandomMinMax(10, 20) + (int)(Caster.Skills[SkillName.Healing].Value / 5); // Base healing amount with bonus from skill
                target.Hits += healAmount;
                
                // Long-term health benefit: Buffing target's regeneration rate for a short period
                target.SendMessage("You feel revitalized after the surgery!");
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1075643, 1075645, TimeSpan.FromMinutes(2), target)); // Adds a healing buff icon

                // Timer for the regeneration buff effect
                Timer buffTimer = new RegenerationBuffTimer(target, TimeSpan.FromMinutes(2));
                buffTimer.Start();
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Surgery m_Owner;

            public InternalTarget(Surgery owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class RegenerationBuffTimer : Timer
        {
            private Mobile m_Target;

            public RegenerationBuffTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Target.SendMessage("The regenerative effects of the surgery have worn off.");
                BuffInfo.RemoveBuff(m_Target, BuffIcon.Bless); // Removes the healing buff icon
            }
        }
    }
}
