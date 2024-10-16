using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class ScryingEye : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Scrying Eye", "Oculum Luminis",
            //SpellCircle.Third,
            21004,
            9300,
            false,
            Reagent.Nightshade,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public ScryingEye(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D ip)
        {
            if (!Caster.CanSee(ip))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(ip, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, ip);

                // Declare a Point3D variable and set it to the value of ip
                Point3D p = new Point3D(ip);

                // Pass the IPoint3D as ref
                SpellHelper.GetSurfaceTop(ref ip);

                Effects.PlaySound(p, Caster.Map, 0x1FE); // Play sound effect for summoning
                Effects.SendLocationParticles(EffectItem.Create(p, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5025); // Visual effect

                InternalItem eye = new InternalItem(p, Caster.Map, Caster);

                Caster.SendMessage("A magical eye appears and begins scouting the area.");
            }

            FinishSequence();
        }

        [DispellableField]
        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x2235) // ItemID for the magical eye
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                Effects.PlaySound(loc, map, 0x1FE); // Sound effect for eye movement
                Visible = true;

                m_Timer = new InternalTimer(this, caster);
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

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;
                private Mobile m_Caster;
                private DateTime m_End;
                private const double DurationSeconds = 30.0; // Duration of the scouting eye

                public InternalTimer(InternalItem item, Mobile caster) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
                {
                    m_Item = item;
                    m_Caster = caster;
                    m_End = DateTime.Now + TimeSpan.FromSeconds(DurationSeconds);
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                        return;

                    if (DateTime.Now >= m_End)
                    {
                        m_Item.Delete();
                        Stop();
                        m_Caster.SendMessage("The magical eye vanishes.");
                    }
                    else
                    {
                        List<Mobile> targets = new List<Mobile>();

                        foreach (Mobile m in m_Item.GetMobilesInRange(10))
                        {
                            if (m.Hidden && m.Player)
                                targets.Add(m);
                        }

                        foreach (Mobile m in targets)
                        {
                            m.CantWalk = true; // Immobilize hidden players briefly
                            m.Hidden = false; // Reveal hidden players
                            m.SendMessage("You have been revealed by a scrying eye!");
                            m.FixedParticles(0x373A, 10, 15, 5025, EffectLayer.Waist); // Visual effect on revealed players
                        }
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private ScryingEye m_Owner;

            public InternalTarget(ScryingEye owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                {
                    // Use IPoint3D directly
                    m_Owner.Target(p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
