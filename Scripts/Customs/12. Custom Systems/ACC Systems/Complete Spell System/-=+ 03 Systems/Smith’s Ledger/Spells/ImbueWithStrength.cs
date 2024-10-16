using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;


namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class ImbueWithStrength : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Imbue with Strength", "Fortitudo Imbuere",
                                                        21015,
                                                        9311,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public ImbueWithStrength(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ImbueWithStrength m_Owner;

            public InternalTarget(ImbueWithStrength owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile m = (Mobile)target;

                    if (!m_Owner.CheckSequence())
                        return;

                    SpellHelper.Turn(m_Owner.Caster, m);

                    int strengthBoost = 10 + (int)(m_Owner.Caster.Skills[SkillName.Blacksmith].Value * 0.1); // Calculate the strength boost
                    int duration = 30 + (int)(m_Owner.Caster.Skills[SkillName.Blacksmith].Value * 0.2); // Duration in seconds

                    m.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Visual effect
                    m.PlaySound(0x1E3); // Sound effect
                    m.SendMessage("You feel a surge of strength!");

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Strength, 1075847, duration, strengthBoost)); // Buff icon

                    m.Str += strengthBoost;
                    m.Delta(MobileDelta.Stat); // Update stats

                    Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                    {
                        if (!m.Deleted && m.Alive)
                        {
                            m.Str -= strengthBoost; // Revert the strength boost
                            m.Delta(MobileDelta.Stat);
                            m.SendMessage("The surge of strength fades away.");
                        }
                    });
                }
                else
                {
                    from.SendMessage("You can only target players or creatures.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
