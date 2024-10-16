using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class SmokeBomb : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Smoke Bomb", "In Nux",
            //SpellCircle.First,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public SmokeBomb(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                Effects.SendLocationEffect(loc, Caster.Map, 0x3728, 20, 10, 1153, 0); // Smoke effect
                Effects.PlaySound(loc, Caster.Map, 0x22F); // Smoke bomb sound

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m is PlayerMobile && Caster.CanBeHarmful(m))
                    {
                        Caster.DoHarmful(m);

                        m.SendMessage("You are enveloped in smoke and your accuracy is reduced!");
                        m.PlaySound(0x1ED); // Sound effect when affected

                        // Applying a temporary debuff to reduce accuracy
                        int duration = 10; // Duration in seconds
                        m.AddStatMod(new StatMod(StatType.Dex, "SmokeDebuff", -20, TimeSpan.FromSeconds(duration)));
                        m.AddStatMod(new StatMod(StatType.Int, "SmokeDebuff", -20, TimeSpan.FromSeconds(duration)));
                    }
                }

                // Brief invisibility effect for the caster
                Caster.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
                {
                    Caster.Hidden = false;
                    Caster.SendMessage("The smoke clears, and you become visible again.");
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SmokeBomb m_Owner;

            public InternalTarget(SmokeBomb owner) : base(12, true, TargetFlags.Harmful)
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
