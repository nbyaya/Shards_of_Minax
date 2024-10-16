using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class CaveCollapse : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cave Collapse", "Kal Vas Xen An Lor",
            // SpellCircle.Fourth,
            21015,
            9300,
            false,
            Reagent.SulfurousAsh,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public CaveCollapse(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                // Create visual and sound effects for the cave collapse
                Effects.SendLocationEffect(loc, map, 0x36BD, 20, 10, 0, 0); // Dust effect
                Effects.PlaySound(loc, map, 0x207); // Earthquake sound

                // Apply a temporary blocking effect and potential damage
                List<Mobile> toDamage = new List<Mobile>();

                foreach (Mobile m in map.GetMobilesInRange(loc, 3))
                {
                    if (m is BaseCreature && !m.Blessed && Caster.CanBeHarmful(m, false))
                    {
                        toDamage.Add(m);
                    }
                }

                foreach (Mobile m in toDamage)
                {
                    Caster.DoHarmful(m);
                    AOS.Damage(m, Caster, Utility.RandomMinMax(20, 40), 100, 0, 0, 0, 0);
                    m.SendMessage("You are hit by falling rocks from the cave-in!");
                }

                InternalItem debris = new InternalItem(loc, map);
                debris.MoveToWorld(loc, map);

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;

            public InternalItem(Point3D loc, Map map) : base(0x36B0) // Random rock debris
            {
                Movable = false;
                MoveToWorld(loc, map);

                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }

            public InternalItem(Serial serial) : base(serial)
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
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;

                public InternalTimer(InternalItem item) : base(TimeSpan.FromSeconds(10.0))
                {
                    m_Item = item;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }

        private class InternalTarget : Target
        {
            private CaveCollapse m_Owner;

            public InternalTarget(CaveCollapse owner) : base(12, true, TargetFlags.None)
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
