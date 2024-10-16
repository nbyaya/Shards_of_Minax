using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class EchoStrike : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Echo Strike", "Exco Strk",
            21004,
            9300
        );

        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public EchoStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (Caster.InRange(target, 1) && CheckSequence())
            {
                Caster.Direction = Caster.GetDirectionTo(target);
                Caster.RevealingAction();

                // First Strike
                SpellHelper.Damage(TimeSpan.Zero, target, Caster, 15 + Utility.Random(10));
                target.FixedParticles(0x376A, 1, 15, 0x13B5, EffectLayer.Waist);
                target.PlaySound(0x213);

                // Echo Wave Effect
                Point3D location = Caster.Location;
                Effects.SendLocationEffect(location, Caster.Map, 0x36BD, 20, 10, 0, 0); // Wave effect
                Effects.PlaySound(location, Caster.Map, 0x655);

                // Reveal hidden enemies within a range of 5 tiles
                foreach (Mobile mob in Caster.GetMobilesInRange(5))
                {
                    if (mob.Hidden && mob != Caster)
                    {
                        mob.RevealingAction();
                        mob.FixedParticles(0x376A, 1, 15, 0x13B5, EffectLayer.Head); // Shimmer effect
                        mob.PlaySound(0x482);
                        Caster.SendMessage("You reveal a hidden enemy!");
                    }
                }

                // Second Strike
                Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
                {
                    if (target.Alive && !target.Deleted)
                    {
                        SpellHelper.Damage(TimeSpan.Zero, target, Caster, 10 + Utility.Random(10));
                        target.FixedParticles(0x376A, 1, 15, 0x13B5, EffectLayer.Waist);
                        target.PlaySound(0x213);
                    }
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private EchoStrike m_Owner;

            public InternalTarget(EchoStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
