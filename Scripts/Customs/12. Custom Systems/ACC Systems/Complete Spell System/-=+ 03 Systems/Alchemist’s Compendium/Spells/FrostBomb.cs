using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;


namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class FrostBomb : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Frost Bomb", "Frigus Flamma",
            21005, // Gump ID
            9301,  // Button ID
            false, // Reagents check (not needed here)
            Reagent.BlackPearl, Reagent.SpidersSilk, Reagent.Nightshade
        );

        public override SpellCircle Circle => SpellCircle.Second;
        public override double CastDelay => 0.2;
        public override double RequiredSkill => 30.0;
        public override int RequiredMana => 25;

        public FrostBomb(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            SpellHelper.Turn(Caster, p);
            SpellHelper.GetSurfaceTop(ref p);

            Point3D loc = new Point3D(p);
            Map map = Caster.Map;

            if (map == null || !map.CanFit(loc, 16, false, false))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
                return;
            }

            if (CheckSequence())
            {
                Effects.PlaySound(loc, map, 0x10B); // Sound effect for frost bomb explosion
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x375A, 10, 30, 1152, 0, 5029, 0); // Frost explosion visual effect

                foreach (Mobile m in map.GetMobilesInRange(loc, 3)) // Range of 3 tiles
                {
                    if (Caster.CanBeHarmful(m, false))
                    {
                        Caster.DoHarmful(m);
                        m.Freeze(TimeSpan.FromSeconds(3.0)); // Freezes the target for 3 seconds
                        m.SendMessage("You are frozen by the chilling frost!");
                        m.PlaySound(0x204); // Sound effect when a target is hit by frost
                        AOS.Damage(m, Caster, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0); // Deals cold damage to the target
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private FrostBomb m_Owner;

            public InternalTarget(FrostBomb owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
