using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class FlavorfulDetection : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Flavorful Detection", "Scentia Detectio",
            21014,
            9310
        );

        public override SpellCircle Circle => SpellCircle.Third; // Adjust as needed

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 50.0; // Adjust as needed
        public override int RequiredMana => 20;

        public FlavorfulDetection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FlavorfulDetection m_Owner;

            public InternalTarget(FlavorfulDetection owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    m_Owner.Target(p);
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);
                Point3D loc = new Point3D(p);

                Effects.PlaySound(loc, Caster.Map, 0x1F5); // Add sound effect for flavor

                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x373A, 10, 15, 5022); // Add visual effect

                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(loc, 5);
                foreach (Mobile m in eable)
                {
                    if (m.Hidden && Caster.CanBeHarmful(m, false))
                    {
                        m.RevealingAction();
                        m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Head); // Visual effect on the revealed target
                        m.PlaySound(0x213); // Sound effect on the revealed target
                    }
                }

                eable.Free();
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
