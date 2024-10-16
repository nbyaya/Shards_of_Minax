using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class EnsnaringNet : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ensnaring Net", "Ensnar Net",
            //SpellCircle.Fourth,
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public EnsnaringNet(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EnsnaringNet m_Owner;

            public InternalTarget(EnsnaringNet owner) : base(10, true, TargetFlags.None)
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

                Effects.PlaySound(loc, Caster.Map, 0x1FB); // Play net throw sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042); // Net visual effect

                Map map = Caster.Map;

                if (map == null)
                    return;

                IPooledEnumerable eable = map.GetMobilesInRange(loc, 3); // Area of effect radius
                foreach (Mobile m in eable)
                {
                    if (m is BaseCreature || m is PlayerMobile)
                    {
                        m.SendMessage("You have been ensnared by a magical net!"); // Message to affected players
                        m.FixedParticles(0x376A, 1, 62, 9910, 2, 2, EffectLayer.Waist); // Visual effect on the affected target
                        m.PlaySound(0x204); // Play sound for the effect

                        // Applying slow effect
                        m.Paralyzed = true;
                        Timer.DelayCall(TimeSpan.FromSeconds(3.0), () => m.Paralyzed = false); // 3 seconds of paralysis

                        // Reduce the target's Dexterity temporarily
                        int oldDex = m.Dex; // Save current Dexterity
                        m.Dex = (int)(oldDex * 0.5); // Reduce Dexterity by 50%
                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => m.Dex = oldDex); // Restore Dexterity after 10 seconds
                    }
                }

                eable.Free();
            }

            FinishSequence();
        }
    }
}
