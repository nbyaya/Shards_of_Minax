using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class ZenBlast : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Zen Blast", "Zenat Ruum",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Adjusted circle based on mana and effect
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 35; } }

        public ZenBlast(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ZenBlast m_Owner;

            public InternalTarget(ZenBlast owner) : base(10, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                    m_Owner.Target(point);
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                // Create flashy visual and sound effects
                Effects.SendLocationEffect(loc, Caster.Map, 0x36BD, 20, 10, 1153, 0); // Area effect explosion
                Effects.PlaySound(loc, Caster.Map, 0x307); // Thunder-like sound

                // Damage and apply debuff to enemies in range
                ArrayList targets = new ArrayList();
                foreach (Mobile m in Caster.GetMobilesInRange(3)) // 3-tile radius
                {
                    if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoHarmful(m);
                    SpellHelper.Damage(this, m, Utility.RandomMinMax(20, 40)); // Random damage between 20 and 40

                    if (Utility.RandomDouble() < 0.4) // 40% chance to slow attack speed
                    {
                        m.SendMessage("You feel your movements slow down!");
                        m.AddStatMod(new StatMod(StatType.Dex, "ZenBlastDexDebuff", -10, TimeSpan.FromSeconds(10.0))); // Reduces Dex by 10 for 10 seconds
                        m.FixedParticles(0x3779, 10, 15, 5008, EffectLayer.Head);
                        m.PlaySound(0x1FB); // Debuff sound effect
                    }
                }
            }

            FinishSequence();
        }
    }
}
