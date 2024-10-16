using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.SkillHandlers;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class ArmLock : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arm Lock", "Restrictio Bracchium",
            21004, // Icon
            9300   // Sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public ArmLock(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArmLock m_Owner;

            public InternalTarget(ArmLock owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeHarmful(target))
                {
                    if (from.InRange(target, 1))
                    {
                        if (m_Owner.CheckSequence())
                        {
                            from.DoHarmful(target);

                            // Play visuals and sounds
                            Effects.SendTargetParticles(target, 0x376A, 9, 32, 5008, EffectLayer.Waist);
                            Effects.PlaySound(target.Location, target.Map, 0x205);

                            // Reduce Wrestling and Tactics skills by 50% for a duration
                            double duration = 10.0; // 10 seconds
                            double wrestlingReduction = target.Skills[SkillName.Wrestling].Base * 0.5;
                            double tacticsReduction = target.Skills[SkillName.Tactics].Base * 0.5;

                            target.SendMessage("Your wrestling and tactics skills have been reduced!");

                            SkillMod wrestlingMod = new DefaultSkillMod(SkillName.Wrestling, true, -wrestlingReduction);
                            SkillMod tacticsMod = new DefaultSkillMod(SkillName.Tactics, true, -tacticsReduction);

                            target.AddSkillMod(wrestlingMod);
                            target.AddSkillMod(tacticsMod);

                            Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                            {
                                target.RemoveSkillMod(wrestlingMod);
                                target.RemoveSkillMod(tacticsMod);
                                target.SendMessage("Your wrestling and tactics skills have returned to normal.");
                            });
                        }
                    }
                    else
                    {
                        from.SendMessage("You are too far away to perform an arm lock.");
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
