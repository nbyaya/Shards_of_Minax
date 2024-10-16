using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class DispelMagic : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Dispel Magic", "An Ort",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public DispelMagic(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                // Visual and Sound Effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FB); // Magic sound
                Caster.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Dispel visual effect

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5)) // Area of effect is 5 tiles around the caster
                {
                    if (m != null && m.Alive)
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    // Remove summoned creatures
                    if (m is BaseCreature)
                    {
                        BaseCreature creature = (BaseCreature)m;
                        if (creature.Summoned)
                        {
                            creature.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Head); // Dispel visual effect
                            creature.Delete();
                        }
                    }

                    // Remove temporary buffs or effects

                    // Additional Visual Effect
                    m.FixedParticles(0x3735, 1, 15, 9902, EffectLayer.Waist);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private DispelMagic m_Owner;

            public InternalTarget(DispelMagic owner) : base(12, false, TargetFlags.None)
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
