using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class PiercingShot : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Piercing Shot", "Arcus Incisus",
            // SpellCircle.Fourth, // You can define the spell circle if needed
            21005,
            9400,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public PiercingShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PiercingShot m_Owner;

            public InternalTarget(PiercingShot owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    SpellHelper.Turn(from, point);

                    if (!from.CanSee(point))
                    {
                        from.SendLocalizedMessage(500237); // Target can not be seen.
                        return;
                    }

                    if (SpellHelper.CheckTown(point, from) && m_Owner.CheckSequence())
                    {
                        Point3D targetLocation = new Point3D(point);
                        Map map = from.Map;

                        if (map == null)
                            return;

                        List<Mobile> targets = new List<Mobile>();
                        IPooledEnumerable eable = map.GetMobilesInRange(targetLocation, 12); // Range to detect mobiles

                        foreach (Mobile m in eable)
                        {
                            if (from.CanBeHarmful(m) && from != m && SpellHelper.ValidIndirectTarget(from, m) && m is BaseCreature)
                            {
                                targets.Add(m);
                            }
                        }

                        eable.Free();

                        if (targets.Count > 0)
                        {
                            from.SendMessage("You fire a piercing shot!");

                            foreach (Mobile m in targets)
                            {
                                from.DoHarmful(m);
                                double damage = Utility.RandomMinMax(10, 20); // Random damage for each target
                                m.Damage((int)damage, from);

                                m.PlaySound(0x22C); // Sound effect for hit
                                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Blood spray effect

                                // Apply bleeding effect
                                m.SendMessage("You are bleeding from the piercing shot!");
                                BleedEffect.ApplyBleeding(m, from, Utility.RandomMinMax(3, 5)); // Apply a bleeding effect for 3-5 ticks
                            }
                        }

                        Effects.SendLocationEffect(targetLocation, map, 0x11A6, 30, 10, 1161, 0); // Visual effect on impact
                        Effects.PlaySound(targetLocation, map, 0x1F7); // Sound effect on impact
                    }

                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public static class BleedEffect
    {
        public static void ApplyBleeding(Mobile target, Mobile caster, int ticks)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            Timer timer = new BleedTimer(target, caster, ticks);
            timer.Start();
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Caster;
            private int m_Ticks;

            public BleedTimer(Mobile target, Mobile caster, int ticks) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Caster = caster;
                m_Ticks = ticks;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                if (m_Ticks > 0)
                {
                    m_Target.SendMessage("You suffer from bleeding!");
                    m_Target.PlaySound(0x19D); // Bleeding sound
                    m_Target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Blood spray effect

                    int bleedDamage = Utility.RandomMinMax(1, 3); // Random bleed damage
                    m_Target.Damage(bleedDamage, m_Caster);

                    m_Ticks--;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
