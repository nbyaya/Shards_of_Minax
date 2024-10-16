using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class BeastlyResilience : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Beastly Resilience", "Veritas Resisto",
            //SpellCircle.Sixth,
            21005,
            9400,
            false,
            Reagent.Bloodmoss,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 16; } }

        public BeastlyResilience(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private BeastlyResilience m_Owner;

            public InternalTarget(BeastlyResilience owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                {
                    m_Owner.Target((IPoint3D)o);
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
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x1F7); // A suitable sound effect

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m.Player && m.Alive && m != Caster)
                    {
                        targets.Add(m);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];
                    ApplyEffect(m);
                }

                ApplyEffect(Caster); // Apply effect to the caster
            }

            FinishSequence();
        }

        private void ApplyEffect(Mobile m)
        {
            m.SendMessage("You feel a surge of resilience!");

            m.FixedParticles(0x373A, 1, 15, 9910, 1153, 3, EffectLayer.Waist); // Add flashy visual effect
            m.PlaySound(0x202); // Add sound effect

            m.Hits += Utility.RandomMinMax(5, 15); // Grant some health
            m.Mana += Utility.RandomMinMax(5, 15); // Grant some mana
            m.Stam += Utility.RandomMinMax(5, 15); // Grant some stamina

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => RemoveEffect(m));
        }

        private void RemoveEffect(Mobile m)
        {
            m.SendMessage("The effect of Beastly Resilience fades away.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
