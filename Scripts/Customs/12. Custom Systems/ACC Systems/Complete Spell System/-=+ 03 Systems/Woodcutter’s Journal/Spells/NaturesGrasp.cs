using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class NaturesGrasp : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Nature’s Grasp", "Terra Vinclis",
            21015,
            9315,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 25; } }

        public NaturesGrasp(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                Effects.PlaySound(loc, map, 0x229); // Play vine sound effect
                Effects.SendLocationEffect(loc, map, 0x373A, 20, 10, 1153); // Adjusted parameters for SendLocationEffect

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in map.GetMobilesInRange(loc, 3))
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

				foreach (Mobile m in targets)
				{
					Caster.DoHarmful(m);
					m.SendMessage("You are entangled by nature's grasp!");

					m.Paralyzed = true; // Paralyze the target
					Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
					{
						if (m.Paralyzed)
						{
							m.Paralyzed = false;
							m.SendMessage("You break free from the vines!");
						}
					});

					// Updated SendTargetParticles call with EffectLayer.Head
					Effects.SendTargetParticles(m, 0x373A, 1, 20, 1153, 7, 9502, EffectLayer.Head, 0x100); // Green vine effect
				}

            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private NaturesGrasp m_Owner;

            public InternalTarget(NaturesGrasp owner) : base(12, true, TargetFlags.None)
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
