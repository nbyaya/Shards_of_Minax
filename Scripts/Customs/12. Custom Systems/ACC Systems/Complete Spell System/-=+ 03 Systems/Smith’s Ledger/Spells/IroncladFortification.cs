using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items; // Make sure this is included
using System.Collections;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class IroncladFortification : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ironclad Fortification", // Name
            "Armor Fortis", // Cast phrase
            21019,
            9315
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public IroncladFortification(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private IroncladFortification m_Owner;

            public InternalTarget(IroncladFortification owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckSequence())
                    {
                        target.PlaySound(0x2D6);
                        target.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);

                        int buffAmount = 10 + (int)(m_Owner.Caster.Skills[SkillName.Blacksmith].Value / 5);
                        DefenseBuffTimer buffTimer = new DefenseBuffTimer(target, buffAmount);
                        buffTimer.Start();

                        from.SendMessage("You fortify your ally with ironclad defenses!");
                        target.SendMessage("You feel a surge of protection as your defenses are fortified!");

                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendMessage("You can only target a player or a creature.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class DefenseBuffTimer : Timer
        {
            private Mobile m_Target;
            private int m_BuffAmount;

            public DefenseBuffTimer(Mobile target, int buffAmount) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                m_Target = target;
                m_BuffAmount = buffAmount;
                Priority = TimerPriority.OneSecond;
                target.VirtualArmorMod += buffAmount;
            }

            protected override void OnTick()
            {
                Stop();
                m_Target.VirtualArmorMod -= m_BuffAmount;
                m_Target.SendMessage("The ironclad fortification fades, and your defenses return to normal.");
            }
        }
    }
}
