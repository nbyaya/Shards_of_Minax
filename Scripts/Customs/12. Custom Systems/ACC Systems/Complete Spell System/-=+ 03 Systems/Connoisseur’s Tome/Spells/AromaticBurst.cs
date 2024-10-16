using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class AromaticBurst : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Aromatic Burst", "Pungentis Aer",
            21006,
            9302,
            false,
            Reagent.Garlic,
            Reagent.Nightshade
        );
		
        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public AromaticBurst(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                Effects.PlaySound(loc, Caster.Map, 0x658); // Play a sound for the cloud

                Effects.SendLocationEffect(loc, Caster.Map, 0x3728, 20, 10, 0, 0); // Display a cloud visual effect
                Caster.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Waist); // Show particles on the caster

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (Caster.CanBeHarmful(m, false) && Caster.InLOS(m))
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];

                    Caster.DoHarmful(m);

                    m.Paralyze(TimeSpan.FromSeconds(5.0)); // Paralyze for 5 seconds

                    Poison poison = Poison.GetPoison(3); // Apply Level 3 Poison
                    m.ApplyPoison(Caster, poison);

                    m.PlaySound(0x22F); // Play a sound on the target
                    m.FixedParticles(0x374A, 10, 30, 5037, EffectLayer.Waist); // Display particles on the target
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AromaticBurst m_Owner;

            public InternalTarget(AromaticBurst owner) : base(12, true, TargetFlags.Harmful)
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
