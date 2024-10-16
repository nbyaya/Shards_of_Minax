using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.ACC.CSS.Systems.MiningMagic;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class GeologistsInsight : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Geologists Insight", "In Korpus Terra",
            21008,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 25.0; } }
        public override int RequiredMana { get { return 15; } }

        public GeologistsInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Point3D loc = new Point3D(p.X, p.Y, p.Z);

                // Visual and Sound Effects
                Effects.PlaySound(loc, Caster.Map, 0x65A); // Earthy rumble sound
                Effects.SendLocationEffect(loc, Caster.Map, 0x36BD, 20, 10, 0, 0); // Dust cloud effect

                // Simulate geological stability check
                double chance = Caster.Skills[SkillName.Mining].Value / 100.0;
                bool stable = Utility.RandomDouble() < chance;

                if (stable)
                {
                    Caster.SendMessage("The geological structure is stable; it is safe to mine here.");
                    Effects.PlaySound(loc, Caster.Map, 0x64F); // Safe, calming sound
                    Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                }
                else
                {
                    Caster.SendMessage("The geological structure is unstable; mining here could cause a collapse.");
                    Effects.PlaySound(loc, Caster.Map, 0x2A4); // Dangerous, warning sound
                    Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x36FE, 10, 32, 5000);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private GeologistsInsight m_Owner;

            public InternalTarget(GeologistsInsight owner) : base(12, true, TargetFlags.None)
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
