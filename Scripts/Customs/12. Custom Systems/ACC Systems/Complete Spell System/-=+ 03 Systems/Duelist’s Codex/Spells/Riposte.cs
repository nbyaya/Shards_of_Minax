using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Riposte : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Riposte", "Riposte",
                                                        // SpellCircle.Fourth,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public Riposte(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanSee(target) && Caster.InRange(target, 1) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Play visual effects and sound
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FA); // Sound effect for the strike
                Caster.FixedParticles(0x3779, 1, 30, 0x13B5, 1153, 0, EffectLayer.Waist); // Flashy visual effect

                // Apply damage to all creatures within 1 tile
                ArrayList targets = new ArrayList();
                foreach (Mobile m in Caster.GetMobilesInRange(1))
                {
                    if (Caster.CanBeHarmful(m, false) && m != Caster)
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];
                    Caster.DoHarmful(m);
                    double damage = Utility.RandomMinMax(20, 40); // Random damage between 20 to 40
                    m.Damage((int)damage, Caster);
                }
            }
            else
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Riposte m_Owner;

            public InternalTarget(Riposte owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
