using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class FlurryOfBlows : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Flurry of Blows", "Rapidus Ictus",
            21005,
            9301,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public FlurryOfBlows(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FlurryOfBlows m_Owner;

            public InternalTarget(FlurryOfBlows owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        m_Owner.Effect(target);
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        public void Effect(Mobile target)
        {
            // Play sound and visual effect for starting the Flurry of Blows
            Caster.PlaySound(0x51D);
            Caster.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Waist);

            // A series of rapid strikes
            List<int> delays = new List<int> { 0, 200, 400, 600, 800 };
            foreach (int delay in delays)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(delay), () => Strike(target));
            }
        }

        private void Strike(Mobile target)
        {
            if (!target.Alive || target.Deleted)
                return;

            // Each strike deals moderate damage and plays a visual effect
            int damage = Utility.RandomMinMax(5, 10);
            AOS.Damage(target, Caster, damage, 0, 100, 0, 0, 0);

            target.FixedParticles(0x37B9, 1, 13, 9911, 1153, 0, EffectLayer.Head);
            target.PlaySound(0x238); // Strike sound

            if (Utility.RandomDouble() < 0.2) // 20% chance for an additional hit
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(200), () => Strike(target));
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
