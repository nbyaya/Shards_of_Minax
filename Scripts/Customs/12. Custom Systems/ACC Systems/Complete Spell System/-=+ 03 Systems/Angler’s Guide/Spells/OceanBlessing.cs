using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class OceanBlessing : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ocean's Blessing", "Mare Benedicite",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.First;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 20.0;
        public override int RequiredMana => 15;

        public OceanBlessing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private OceanBlessing m_Spell;

            public InternalTarget(OceanBlessing spell) : base(12, false, TargetFlags.Beneficial)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (m_Spell.CheckSequence())
                    {
                        // Apply the fishing skill and luck boost
                        target.SendMessage("You feel the blessing of the ocean upon you!");
                        target.PlaySound(0x1F5); // Play a water-related sound
                        target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Water splash effect

                        // Temporarily increase fishing skill and luck
                        target.Skills[SkillName.Fishing].Base += 10.0;

                        Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                        {
                            // Revert the skill and luck boost after 30 seconds
                            target.Skills[SkillName.Fishing].Base -= 10.0;
                            target.SendMessage("The blessing of the ocean fades away.");
                        });
                    }
                }
                m_Spell.FinishSequence();
            }
        }
    }
}
