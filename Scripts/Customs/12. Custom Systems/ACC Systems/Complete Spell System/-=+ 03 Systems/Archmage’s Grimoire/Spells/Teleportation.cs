using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class Teleportation : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Teleportation", "Rel Por",
            21004, // Animation ID
            9300,  // Animation frame
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; } // Adjust circle level as needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } } // Adjust skill requirement
        public override int RequiredMana { get { return 15; } } // Mana cost as per your request

        public Teleportation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Teleportation m_Owner;

            public InternalTarget(Teleportation owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    m_Owner.Target(point);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                Point3D from = Caster.Location;
                Point3D to = new Point3D(p);

                SpellHelper.Turn(Caster, p);

                Caster.Location = to; // Teleport the player
                Caster.ProcessDelta(); // Update player position

                Effects.SendLocationParticles(EffectItem.Create(from, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023); // Visual effect at original location
                Effects.SendLocationParticles(EffectItem.Create(to, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);  // Visual effect at target location
                Caster.PlaySound(0x1FE); // Teleport sound

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
