using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class FlameTrap : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Flame Trap", "Ignis Tether",
            21001,
            9200,
            false,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public FlameTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FlameTrap m_Owner;

            public InternalTarget(FlameTrap owner) : base(12, true, TargetFlags.None)
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x208); // Flame sound effect

                Point3D loc = new Point3D(p);

                // Create the trap item at the specified location
                FlameTrapItem trap = new FlameTrapItem(Caster);
                trap.MoveToWorld(loc, Caster.Map);

                FinishSequence();
            }
        }

        private class FlameTrapItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public FlameTrapItem(Mobile caster) : base(0x376A) // Invisible flame trap item ID
            {
                Movable = false;
                Visible = false;
                m_Caster = caster;

                m_Timer = new TrapTimer(this, caster);
                m_Timer.Start();
            }

            public FlameTrapItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                m_Timer = new TrapTimer(this, null);
                m_Timer.Start();
            }

            private class TrapTimer : Timer
            {
                private FlameTrapItem m_Trap;
                private Mobile m_Caster;

                public TrapTimer(FlameTrapItem trap, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
                {
                    m_Trap = trap;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Trap.Deleted)
                    {
                        Stop();
                        return;
                    }

                    List<Mobile> mobilesInRange = new List<Mobile>();

                    foreach (Mobile m in m_Trap.GetMobilesInRange(0))
                    {
                        if (m != m_Caster && m is Mobile && m.Alive && !m.IsDeadBondedPet && m.AccessLevel == AccessLevel.Player)
                        {
                            mobilesInRange.Add(m);
                        }
                    }

                    if (mobilesInRange.Count > 0)
                    {
                        foreach (Mobile target in mobilesInRange)
                        {
                            if (target != null && !target.Deleted && target.Alive && !target.IsDeadBondedPet)
                            {
                                AOS.Damage(target, m_Caster, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0); // Fire damage
                                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Fire burst effect
                                target.PlaySound(0x208); // Flame sound effect
                            }
                        }

                        m_Trap.Delete(); // Destroy the trap after triggering
                        Stop();
                    }
                }
            }
        }
    }
}
