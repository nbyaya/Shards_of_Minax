using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class MentalClarity : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mental Clarity", "In Fortio Sento",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public MentalClarity(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                Effects.SendTargetParticles(target, 0x376A, 9, 32, 5008, EffectLayer.Head);
                Effects.PlaySound(target.Location, target.Map, 0x1F3);

                double skillBonus = 0.10; // 10% skill effectiveness increase

                target.FixedParticles(0x375A, 1, 15, 5017, 1153, 2, EffectLayer.Waist);
                target.PlaySound(0x5C3);

                target.SendMessage("You feel your mind sharpen as your focus increases!");
                ApplyMentalClarity(target, skillBonus);

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => RemoveMentalClarity(target, skillBonus));
            }

            FinishSequence();
        }

        private void ApplyMentalClarity(Mobile target, double skillBonus)
        {
            // Apply skill bonus
            foreach (var skill in target.Skills)
            {
                skill.Base += skill.Base * skillBonus;
            }
        }

        private void RemoveMentalClarity(Mobile target, double skillBonus)
        {
            // Revert skill bonus
            foreach (var skill in target.Skills)
            {
                skill.Base -= skill.Base * skillBonus;
            }

            target.SendMessage("The effects of Mental Clarity have worn off.");
        }

        private class InternalTarget : Target
        {
            private MentalClarity m_Owner;

            public InternalTarget(MentalClarity owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
