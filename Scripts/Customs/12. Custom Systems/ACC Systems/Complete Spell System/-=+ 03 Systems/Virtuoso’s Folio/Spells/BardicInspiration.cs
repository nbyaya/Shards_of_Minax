using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class BardicInspiration : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Bardic Inspiration", "Cadentia Fortis",
            // SpellCircle.Third,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 65; } }

        public BardicInspiration(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || target.Deleted || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckBSequence(target))
            {
                SpellHelper.Turn(Caster, target);

                // Visual and Sound Effects
                target.FixedParticles(0x373A, 10, 15, 5013, EffectLayer.Waist); // Flare effect around the waist
                target.PlaySound(0x1F7); // Magical sound effect

                // Temporary skill boost logic
                double skillBoost = Caster.Skills[CastSkill].Value * 0.1;
                double attributeBoost = Caster.Skills[CastSkill].Value * 0.05;

                foreach (Skill skill in target.Skills)
                {
                    target.AddSkillMod(new DefaultSkillMod(skill.SkillName, true, skillBoost));
                }

                target.Str += (int)attributeBoost;
                target.Dex += (int)attributeBoost;
                target.Int += (int)attributeBoost;

                // Timer to remove buffs after duration
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    foreach (Skill skill in target.Skills)
                    {
                        target.RemoveStatMod("BardicInspirationSkillMod");
                    }

                    target.Str -= (int)attributeBoost;
                    target.Dex -= (int)attributeBoost;
                    target.Int -= (int)attributeBoost;

                    // End of effect visual
                    target.FixedParticles(0x375A, 1, 15, 5013, EffectLayer.Head); // Sparkle effect
                    target.PlaySound(0x1F8); // Fade out sound effect
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private BardicInspiration m_Owner;

            public InternalTarget(BardicInspiration owner) : base(12, false, TargetFlags.Beneficial)
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
