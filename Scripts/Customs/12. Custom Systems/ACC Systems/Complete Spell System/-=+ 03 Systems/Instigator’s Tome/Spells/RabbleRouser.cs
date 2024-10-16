using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class RabbleRouser : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rabble-Rouser", "Incite Chaos",
            21005,
            9400
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RabbleRouser(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private RabbleRouser m_Owner;

            public InternalTarget(RabbleRouser owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            SpellHelper.Turn(Caster, p);
            SpellHelper.GetSurfaceTop(ref p);

            Point3D loc = new Point3D(p);
            Map map = Caster.Map;

            if (map == null)
                return;

            Effects.PlaySound(loc, map, 0x3E9); // Play a dramatic sound effect
            Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052); // Create a visual effect

            List<Mobile> toAffect = new List<Mobile>();

            foreach (Mobile m in map.GetMobilesInRange(loc, 5)) // 5-tile radius
            {
                if (m != Caster && m is BaseCreature && Caster.CanBeHarmful(m, false))
                {
                    toAffect.Add(m);
                }
            }

            if (toAffect.Count > 0)
            {
                foreach (Mobile m in toAffect)
                {
                    Caster.DoHarmful(m);
                    m.Combatant = Utility.RandomList(toAffect.ToArray()); // Randomly set the combatant to another creature in the area
                    m.Say("*becomes enraged*");
                    m.FixedParticles(0x3728, 1, 13, 9912, 117, 3, EffectLayer.Head);
                    m.PlaySound(0x1F1);

                    // Apply the temporary aggression effect
                    new AggressionEffect(m).Start();
                }
            }

            FinishSequence();
        }

        private class AggressionEffect : Timer
        {
            private Mobile m_Mobile;

            public AggressionEffect(Mobile m) : base(TimeSpan.FromSeconds(0.0), TimeSpan.FromSeconds(1.0), 10) // Lasts for 10 seconds
            {
                m_Mobile = m;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Mobile == null || m_Mobile.Deleted || !m_Mobile.Alive)
                {
                    Stop();
                    return;
                }

                // Visual effect on each tick
                m_Mobile.FixedParticles(0x3728, 1, 13, 9912, 117, 3, EffectLayer.Head);

                // Optional: Handle any stopping logic directly here if needed
                // or use a different mechanism if necessary
            }

            // Optionally, you can handle any stopping logic here in case OnTick does not handle it
        }
    }
}
