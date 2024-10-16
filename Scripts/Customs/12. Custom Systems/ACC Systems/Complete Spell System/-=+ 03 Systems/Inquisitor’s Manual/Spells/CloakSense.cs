using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class CloakSense : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cloak Sense", "In Vas Wis",
            21004,
            9300
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public CloakSense(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Targeting for the spell; the caster selects the area to sense
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CloakSense m_Owner;

            public InternalTarget(CloakSense owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    m_Owner.Target(new Point3D(point)); // Convert IPoint3D to Point3D
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Point3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                // Effects and sounds to make the ability flashy
                Effects.PlaySound(Caster.Location, Caster.Map, 0x653);
                Effects.SendLocationParticles(
                    EffectItem.Create(p, Caster.Map, EffectItem.DefaultDuration), 
                    0x376A, 1, 29, 1153, 4, 9502, 0
                );

                // Detect invisible or cloaked enemies within a certain radius
                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(p, 8); // Use Point3D directly here
                foreach (Mobile m in eable)
                {
                    if (m != Caster && m.Hidden && m.AccessLevel == AccessLevel.Player)
                    {
                        m.RevealingAction();
                        m.SendMessage("You feel a presence trying to sense you!");

                        // Visual cue to approximate location
                        Effects.SendLocationParticles(
                            EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                            0x376A, 9, 32, 5024
                        );

                        m.PlaySound(0x28E);
                    }
                }
                eable.Free();

                FinishSequence();
            }
        }
    }
}
