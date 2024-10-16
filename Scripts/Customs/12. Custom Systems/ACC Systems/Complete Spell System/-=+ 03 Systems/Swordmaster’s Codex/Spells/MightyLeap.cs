using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class MightyLeap : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mighty Leap", "Salta Potens",
            21015, // Icon
            9414   // Sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override int RequiredMana { get { return 25; } }

        public MightyLeap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MightyLeap m_Owner;

            public InternalTarget(MightyLeap owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    IPoint3D targetLocation = (IPoint3D)targeted;

                    if (from.CanSee(targetLocation))
                    {
                        SpellHelper.Turn(from, targetLocation);
                        SpellHelper.GetSurfaceTop(ref targetLocation);

                        Point3D dest = new Point3D(targetLocation);

                        // Visual and sound effects for the leap
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        from.PlaySound(0x1FE);

                        // Move the caster to the target location
                        from.MoveToWorld(dest, from.Map);

                        // Flashy arrival effect
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5044);
                        from.PlaySound(0x208);

                        m_Owner.FinishSequence();
                    }
                    else
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Quick cast for an agile maneuver
        }

        public override double RequiredSkill { get { return 30.0; } } // Skill requirement for the ability
    }
}
