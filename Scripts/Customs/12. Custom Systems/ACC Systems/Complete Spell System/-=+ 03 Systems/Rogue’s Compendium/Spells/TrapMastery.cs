using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class TrapMastery : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Mastery", "Placere Lacus!",
            // SpellCircle.Second, // Optional if needed
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public TrapMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TrapMastery m_Owner;

            public InternalTarget(TrapMastery owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    m_Owner.Target(p);
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            SpellHelper.Turn(Caster, p);
            SpellHelper.GetSurfaceTop(ref p);

            Point3D loc = new Point3D(p.X, p.Y, p.Z);
            Map map = Caster.Map;

            // Create a trap at the targeted location
            LandmineTile trap = new LandmineTile(); // Example trap type; you can use other trap types as well
            trap.MoveToWorld(loc, map);


            // Visual and sound effects
            Effects.SendLocationEffect(loc, map, 0x36BD, 20, 10, 0, 0);
            Effects.PlaySound(loc, map, 0x2B2); // Explosion sound effect

            // Additional visual flair
            for (int i = 0; i < 3; i++)
            {
                Effects.SendLocationEffect(new Point3D(loc.X + Utility.RandomMinMax(-1, 1), loc.Y + Utility.RandomMinMax(-1, 1), loc.Z), map, 0x3709, 30, 10, 0x481, 4);
            }

            Caster.SendMessage("You have set a trap with increased effectiveness!");
            FinishSequence();
        }

    }
}
