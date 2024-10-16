using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class SurveyorsInsight : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Surveyor’s Insight", "Map Agnito",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        // Convert TimeSpan to double by using TotalSeconds
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public SurveyorsInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(SurveyorsInsight)))
            {
                Caster.SendLocalizedMessage(1070727); // You must wait before using that ability again.
                return;
            }

            if (CheckSequence())
            {
                // Apply skill bonus
                Caster.SendMessage("You gain insight into the land around you!");
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                Caster.PlaySound(0x1FD);

                Caster.Skills[SkillName.Cartography].Base += 15;
                Caster.Skills[SkillName.Tracking].Base += 15;

                // Set a timer to remove the bonus after 5 minutes
                Timer.DelayCall(TimeSpan.FromMinutes(5), () =>
                {
                    Caster.Skills[SkillName.Cartography].Base -= 15;
                    Caster.Skills[SkillName.Tracking].Base -= 15;
                    Caster.SendMessage("Your Surveyor’s Insight fades.");
                });

                // Start cooldown timer
                Caster.BeginAction(typeof(SurveyorsInsight));
                Timer.DelayCall(TimeSpan.FromMinutes(20), () =>
                {
                    Caster.EndAction(typeof(SurveyorsInsight));
                    Caster.SendMessage("You may now use Surveyor’s Insight again.");
                });
            }

            FinishSequence();
        }
    }
}
