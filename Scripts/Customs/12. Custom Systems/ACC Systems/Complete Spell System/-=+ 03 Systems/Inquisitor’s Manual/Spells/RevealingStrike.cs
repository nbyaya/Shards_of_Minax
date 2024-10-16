using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class RevealingStrike : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Revealing Strike", "Ex Luminar",
            21004, // Animation
            9300,  // Effect
            false,
            Reagent.MandrakeRoot,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RevealingStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x208); // Sound effect for spell cast

                Point3D location = new Point3D(p);

                Map map = Caster.Map;
                if (map == null)
                    return;

                // Create a burst effect at the target location
                Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044, 0, 0, 0);

                // Reveal hidden mobiles in the area
                IPooledEnumerable mobilesInRange = map.GetMobilesInRange(location, 3); // Radius of effect
                ArrayList toReveal = new ArrayList();

                foreach (Mobile m in mobilesInRange)
                {
                    if (m.Hidden && m != Caster)
                        toReveal.Add(m);
                }

                mobilesInRange.Free();

                for (int i = 0; i < toReveal.Count; ++i)
                {
                    Mobile m = (Mobile)toReveal[i];
                    m.RevealingAction();
                    m.SendMessage("You have been revealed by a powerful strike!");
                    
                    // Additional damage to revealed enemies
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, Caster);
                    
                    // Visual effects on revealed enemies
                    m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Head);
                    m.PlaySound(0x213);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private RevealingStrike m_Owner;

            public InternalTarget(RevealingStrike owner) : base(12, true, TargetFlags.Harmful)
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
