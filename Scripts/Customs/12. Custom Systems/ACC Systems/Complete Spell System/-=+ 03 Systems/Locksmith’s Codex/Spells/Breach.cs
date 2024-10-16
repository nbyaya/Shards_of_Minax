using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class Breach : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Breach", "In Vas Ort Grav",
            //SpellCircle.Fourth,
            21001,
            9301,
            false,
            Reagent.BlackPearl,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public Breach(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(ILockable target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                if (target.Locked)
                {
                    target.Locked = false;
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x307); // Sound of a lock being picked

                    // Small explosion effect and damage to nearby enemies
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x36BD, 20, 10, 1160, 0);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x11D); // Explosion sound

                    foreach (Mobile m in Caster.GetMobilesInRange(2))
                    {
                        if (m != Caster && m.Alive && m.CanBeHarmful(Caster))
                        {
                            Caster.DoHarmful(m);
                            int damage = Utility.RandomMinMax(5, 15);
                            m.Damage(damage);
                            m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
                            m.PlaySound(0x208);
                        }
                    }

                    // Fire effects for a flashy display
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3709, 30, 10, 1161, 0);
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3709, 30, 10, 1161, 0);
                }
                else
                {
                    Caster.SendLocalizedMessage(1042064); // That is already unlocked.
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Breach m_Owner;

            public InternalTarget(Breach owner) : base(1, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is ILockable)
                {
                    m_Owner.Target((ILockable)o);
                }
                else
                {
                    from.SendLocalizedMessage(501666); // You can't target that.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
