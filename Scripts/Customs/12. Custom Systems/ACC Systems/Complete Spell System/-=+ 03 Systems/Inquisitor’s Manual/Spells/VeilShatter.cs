using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class VeilShatter : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Veil Shatter", "Unveil Truth",
            //SpellCircle.Third,
            21004,
            9300,
            false,
            Reagent.Bloodmoss,
            Reagent.Garlic,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 40; } }

        public VeilShatter(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private VeilShatter m_Owner;

            public InternalTarget(VeilShatter owner) : base(12, true, TargetFlags.None)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                // Visual and Sound Effects
                Effects.PlaySound(loc, map, 0x1FE); // Dramatic Sound Effect
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044);

                // Reveal all hidden entities in range and negate stealth abilities
                int radius = 5;
                ArrayList targets = new ArrayList();

                foreach (Mobile m in map.GetMobilesInRange(loc, radius))
                {
                    if (m.Hidden && m != Caster)
                    {
                        m.RevealingAction();
                        m.SendMessage("The Veil Shatter reveals you from hiding!");

                        // Apply temporary stealth negation (use a context-based approach or timer to handle this)
                        m.Hidden = false; // Example, actual implementation may vary
                        m.PlaySound(0x653); // Sound to indicate reveal
                        m.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Head);
                    }
                }
            }

            FinishSequence();
        }
    }
}
