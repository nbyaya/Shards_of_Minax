using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class ShadowStep : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shadow Step", "In Jux Rel Xen",
                                                        21015,
                                                        9214,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 50; } }

        private static readonly TimeSpan m_Cooldown = TimeSpan.FromSeconds(45.0);
        private static readonly TimeSpan m_DisorientationDuration = TimeSpan.FromSeconds(5.0);
        private DateTime m_LastUse = DateTime.MinValue;

        public ShadowStep(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (DateTime.Now < (m_LastUse + m_Cooldown))
            {
                Caster.SendMessage("You must wait before using this ability again.");
                return;
            }

            m_LastUse = DateTime.Now; // Update the last use time

            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowStep m_Owner;

            public InternalTarget(ShadowStep owner) : base(12, false, TargetFlags.Harmful)
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
                        Point3D behindTarget = new Point3D(target.X - 1, target.Y, target.Z); // Simplistic calculation for behind target
                        m_Owner.Caster.Location = behindTarget;

                        Effects.PlaySound(m_Owner.Caster.Location, m_Owner.Caster.Map, 0x1FB); // Teleport sound
                        m_Owner.Caster.Hidden = true; // Brief invisibility after teleport
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => m_Owner.Caster.Hidden = false); // Unhide after 1 second

                        m_Owner.Caster.Direction = m_Owner.Caster.GetDirectionTo(target); // Face the target
                        target.Freeze(m_DisorientationDuration); // Freeze target for 5 seconds to simulate disorientation
                        
                        // Confusion and disorientation effects
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5023);
                        Effects.PlaySound(target.Location, target.Map, 0x209); // Sound of disorientation

                        target.SendMessage("You are disoriented and confused!");

                        // Optional: Add damage or status effect here if needed
                    }
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
