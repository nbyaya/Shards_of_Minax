using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class PotionOfStrength : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Potion of Strength", "Fortis Potio",
            21005, // Spell icon
            9301   // Sound effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public PotionOfStrength(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                FinishSequence();
            }
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (Caster == m)
            {
                Caster.SendMessage("You drink the potion and feel a surge of strength!");
                Caster.PlaySound(0x44); // Play a potion-drinking sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, 37, 0, EffectLayer.Waist); // Visual effect around the caster

                // Temporarily increase the strength
                int strBonus = (int)(Caster.Skills[SkillName.Alchemy].Value / 10);
                TimeSpan duration = TimeSpan.FromSeconds(30 + (Caster.Skills[SkillName.Alchemy].Value * 0.5));

                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Strength, 1075845, duration, Caster, strBonus.ToString())); // Show a buff icon with duration

                m.SendMessage("Your strength is increased!");
                m.Str += strBonus;

                // Revert the strength increase after the duration
                Timer.DelayCall(duration, () =>
                {
                    m.Str -= strBonus;
                    m.SendMessage("The effects of the potion wear off, and your strength returns to normal.");
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private PotionOfStrength m_Owner;

            public InternalTarget(PotionOfStrength owner) : base(1, false, TargetFlags.Beneficial)
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
