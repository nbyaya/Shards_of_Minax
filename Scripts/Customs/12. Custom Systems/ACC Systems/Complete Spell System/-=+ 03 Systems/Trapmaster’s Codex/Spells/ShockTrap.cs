using System;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Network;
using System.Collections;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class ShockTrap : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shock Trap", "Voltus Arcanus",
            21001,
            9200,
            false,
            Reagent.MandrakeRoot,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public ShockTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);
                Point3D loc = new Point3D(p.X, p.Y, p.Z);

                Effects.SendLocationEffect(loc, Caster.Map, 0x36B0, 30, 10, 1153, 0); // Visual effect for the trap
                Effects.PlaySound(loc, Caster.Map, 0x29A); // Electric sound effect

                new ShockTrapItem(Caster, loc, Caster.Map);

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private ShockTrap m_Owner;

            public InternalTarget(ShockTrap owner) : base(12, true, TargetFlags.None)
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

        private class ShockTrapItem : Item
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public ShockTrapItem(Mobile caster, Point3D loc, Map map) : base(0x1B72) // Trap ItemID
            {
                Movable = false;
                Visible = true;
                MoveToWorld(loc, map);
                m_Caster = caster;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();
            }

            public ShockTrapItem(Serial serial) : base(serial)
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

            public override bool OnMoveOver(Mobile m)
            {
                if (m == m_Caster)
                    return true;

                if (m is BaseCreature || m.Player)
                {
                    Effects.SendLocationEffect(Location, Map, 0x36B0, 30, 10, 1153, 0); // Shock visual
                    Effects.PlaySound(Location, Map, 0x29A); // Electric shock sound

                    if (m.Alive && Utility.RandomDouble() < 0.5) // 50% chance to stun
                    {
                        m.Freeze(TimeSpan.FromSeconds(2.0)); // Stun effect for 2 seconds
                        m.SendMessage("You are stunned by the shock trap!");
                    }
                    else
                    {
                        AOS.Damage(m, m_Caster, Utility.RandomMinMax(15, 30), 0, 0, 0, 100, 0); // Deals electrical damage
                        m.SendMessage("You are shocked by the trap!");
                    }

                    this.Delete();
                }

                return true;
            }

            private class InternalTimer : Timer
            {
                private ShockTrapItem m_Item;

                public InternalTimer(ShockTrapItem item, TimeSpan duration) : base(duration)
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
    }
}
