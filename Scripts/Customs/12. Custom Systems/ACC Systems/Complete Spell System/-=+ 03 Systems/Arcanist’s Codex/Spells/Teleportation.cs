using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class Teleportation : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Teleportation", "Rel Por",
            21005, 9410,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; } // You can adjust the circle level as needed.
        }

        public override double CastDelay { get { return 0.2; } } // Cast delay in seconds.
        public override double RequiredSkill { get { return 60.0; } } // Required skill level.
        public override int RequiredMana { get { return 40; } } // Mana cost.

        public Teleportation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Point3D from = Caster.Location;
                Point3D to = new Point3D(p);

                // Play sound and visual effects at the starting location
                Effects.PlaySound(from, Caster.Map, 0x1FE);

                // Teleport the caster to the target location
                Caster.Location = to;
                Caster.ProcessDelta();

                // Play sound and visual effects at the target location
                Effects.PlaySound(to, Caster.Map, 0x1FE);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Teleportation m_Owner;

            public InternalTarget(Teleportation owner) : base(12, true, TargetFlags.None)
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
