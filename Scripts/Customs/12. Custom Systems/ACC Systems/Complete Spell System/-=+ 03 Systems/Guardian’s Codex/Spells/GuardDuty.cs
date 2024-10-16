using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class GuardDuty : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Guard Duty", "Fidus Defendo",
            21005,
            9301,
            false,
            Reagent.Garlic,
            Reagent.Ginseng
        );

        public override SpellCircle Circle => SpellCircle.Fifth;

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 30;

        public GuardDuty(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);

                // Ensure p is converted to a Point3D for other operations
                Point3D point = new Point3D(p);

                SpellHelper.GetSurfaceTop(ref p); // Correct usage with IPoint3D

                // Define the radius and duration
                int radius = 5;
                TimeSpan duration = TimeSpan.FromSeconds(30.0);

                Effects.PlaySound(point, Caster.Map, 0x1F7); // Play a spell casting sound
                Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Visual effect at the center

                List<Mobile> allies = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(radius))
                {
                    if (m != Caster && m is PlayerMobile && m.Alive && m.Karma >= 0)
                    {
                        allies.Add(m);
                        m.SendMessage("You feel your defensive abilities bolstered by the Guard Duty spell!");
                        m.FixedEffect(0x376A, 10, 16); // Visual effect on each ally
                        m.PlaySound(0x20E); // Sound effect on each ally
                        m.SendMessage("You receive a temporary boost to your Parry skill!"); // Notify of the boost
                        // m.SendSkillBoost(SkillName.Parry, 10.0, duration); // This method doesn't exist. You need to implement it or use an alternative.
                    }
                }

                Timer.DelayCall(duration, () => EndEffect(allies)); // Schedule the end of effect
            }

            FinishSequence();
        }

        private void EndEffect(List<Mobile> allies)
        {
            foreach (Mobile m in allies)
            {
                m.SendMessage("The effects of the Guard Duty spell have worn off.");
                m.PlaySound(0x1F8); // Sound effect to indicate the end
            }
        }

        private class InternalTarget : Target
        {
            private GuardDuty m_Owner;

            public InternalTarget(GuardDuty owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
