using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class Tranquilize : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tranquilize", "Calmu Clamo",
            //SpellCircle.Fourth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public Tranquilize(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                // Visual and sound effects
                Effects.PlaySound(loc, map, 0x1F7);
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5025);

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (Caster.CanBeHarmful(m) && m.Combatant != Caster)
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    Caster.DoHarmful(m);
                    m.SendMessage("You feel a wave of tranquility wash over you, reducing your combat effectiveness!");
                    m.Say("I feel... so calm...");

                    // Reducing attack speed and movement speed
                    m.AddStatMod(new StatMod(StatType.Dex, "TranquilizeDex", -(int)(m.Dex * 0.25), TimeSpan.FromSeconds(10.0)));
                    m.AddStatMod(new StatMod(StatType.Str, "TranquilizeStr", -(int)(m.Str * 0.25), TimeSpan.FromSeconds(10.0)));

                    // Visual effect on target
                    m.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist);
                    m.PlaySound(0x1FB);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Tranquilize m_Owner;

            public InternalTarget(Tranquilize owner) : base(12, true, TargetFlags.Harmful)
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
