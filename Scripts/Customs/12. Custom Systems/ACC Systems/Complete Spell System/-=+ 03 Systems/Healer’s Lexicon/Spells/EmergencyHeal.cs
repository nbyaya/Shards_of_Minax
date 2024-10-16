using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class EmergencyHeal : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Emergency Heal", "In Mtni Ylem",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } } // Less powerful, so lower skill requirement
        public override int RequiredMana { get { return 15; } } // 15 Mana as described

        public EmergencyHeal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EmergencyHeal m_Owner;

            public InternalTarget(EmergencyHeal owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckBSequence(target))
            {
                SpellHelper.Turn(Caster, target);
                
                int healAmount = (int)(Caster.Skills[SkillName.Healing].Value * 0.3); // Healing amount is less than a standard heal
                healAmount = Utility.RandomMinMax(healAmount / 2, healAmount); // Randomize the heal amount for a bit of variance

                // Apply the heal
                target.Heal(healAmount, Caster);

                // Visual and sound effects for the flashy effect
                target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // Light particle effect
                target.PlaySound(0x1F2); // Healing sound effect

                // Additional flashy effects
                Effects.SendTargetParticles(target, 0x373A, 10, 15, 5013, 1153, 7, EffectLayer.Head, 0); // Additional light effect (added '0' for the unknown parameter)
                Effects.PlaySound(target.Location, target.Map, 0x212); // Additional healing sound

                Caster.SendMessage("You feel a surge of healing energy course through you as you quickly heal your target!");
                target.SendMessage("A surge of healing energy rapidly restores some of your health!");
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Short cast time for quick response
        }
    }
}
