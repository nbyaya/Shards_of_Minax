using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class LullabyOfSleep : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Lullaby of Sleep", "Somnum Melodia",
            //SpellCircle.Fifth,
            21004,
            9300,
            false,
            Reagent.Nightshade,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 60; } }

        public LullabyOfSleep(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Effects.PlaySound(loc, Caster.Map, 0x2C3); // Play a soothing lullaby sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x374A, 10, 30, 5052, 0, 0, 0x100); // Visual effect for the sleep spell

                ArrayList targets = new ArrayList();
                foreach (Mobile m in Caster.GetMobilesInRange(3)) // Effect area of 3 tiles
                {
                    if (Caster.CanBeHarmful(m, false) && m is BaseCreature && !((BaseCreature)m).Controlled)
                    {
                        targets.Add(m);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];
                    Caster.DoHarmful(m);
                    m.SendMessage("You feel drowsy as the lullaby takes effect."); // Message to affected player

                    m.Freeze(TimeSpan.FromSeconds(5.0)); // Freeze target for 5 seconds
                    m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Additional visual effect on target
                    m.PlaySound(0x1F7); // Play sleep sound on target
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private LullabyOfSleep m_Owner;

            public InternalTarget(LullabyOfSleep owner) : base(12, false, TargetFlags.Harmful)
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
