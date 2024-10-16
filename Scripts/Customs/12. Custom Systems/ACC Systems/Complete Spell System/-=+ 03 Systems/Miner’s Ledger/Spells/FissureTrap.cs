using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class FissureTrap : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fissure Trap", "Crusta Fracta",
            //SpellCircle.Sixth,
            21016,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public FissureTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);
                Effects.PlaySound(loc, Caster.Map, 0x207); // Earthquake sound
                Effects.SendLocationEffect(loc, Caster.Map, 0x36BD, 20, 10, 1166, 0); // Earth fissure effect

                InternalItem fissure = new InternalItem(loc, Caster.Map, Caster);
                fissure.MoveToWorld(loc, Caster.Map);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x122A) // Fissure graphic
            {
                Movable = false;
                Hue = 1153; // Dark earth color
                MoveToWorld(loc, map);

                m_Caster = caster;

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
                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }

            public override void OnDelete()
            {
                base.OnDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (m.Alive && m.AccessLevel == AccessLevel.Player && m != m_Caster)
                {
                    m.SendMessage("You feel the ground crack beneath you!");

                    // Slow effect: reduce dexterity temporarily
                    m.Dex -= 10;
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => m.Dex += 10);

                    // Damage effect: moderate physical damage
                    AOS.Damage(m, m_Caster, Utility.RandomMinMax(10, 20), 100, 0, 0, 0, 0);
                    m.PlaySound(0x1F1); // Sound effect for stepping on the fissure
                }

                return true;
            }

            private class InternalTimer : Timer
            {
                private Item m_Item;

                public InternalTimer(Item item) : base(TimeSpan.FromSeconds(30.0))
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
            private FissureTrap m_Owner;

            public InternalTarget(FissureTrap owner) : base(10, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                    m_Owner.Target(point);
                else
                    from.SendMessage("That is not a valid location.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
