using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class Cleave : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cleave",                      // Name
            "Cleave!",                     // Description
                                                        21004,
                                                        9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public Cleave(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (m == Caster || !Caster.CanBeHarmful(m, false) || !Caster.InLOS(m))
                        continue;

                    if (Caster.Direction == GetDirectionTo(Caster.Location, m.Location))
                        targets.Add(m);
                }

                if (targets.Count > 0)
                {
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x309); // Play cleave sound
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 25, 0, 2, 9962, 0); // Flashy effect

                    foreach (Mobile target in targets)
                    {
                        Caster.DoHarmful(target);
                        double damage = Caster.Skills[SkillName.Swords].Value / 2; // Damage based on caster's skill
                        AOS.Damage(target, Caster, (int)damage, 100, 0, 0, 0, 0); // Deal physical damage
                    }
                }
            }

            FinishSequence();
        }

        private static Direction GetDirectionTo(Point3D from, Point3D to)
        {
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;

            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            if (adx >= ady * 3)
                return dx > 0 ? Direction.East : Direction.West;
            else if (ady >= adx * 3)
                return dy > 0 ? Direction.South : Direction.North;
            else if (dx > 0)
                return dy > 0 ? Direction.Down : Direction.Right;
            else
                return dy > 0 ? Direction.Left : Direction.Up;
        }

        private class InternalTarget : Target
        {
            private Cleave m_Owner;

            public InternalTarget(Cleave owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                    m_Owner.Target((IPoint3D)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
