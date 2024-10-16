using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class DeathsEmbrace : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Death's Embrace", "Ex Mortis Manus",
                                                        // SpellCircle.Eighth,
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.BatWing,
                                                        Reagent.GraveDust
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 20; } }

        public DeathsEmbrace(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Effects.PlaySound(loc, Caster.Map, 0x1FB); // Sound of a ghostly wail

                // Summon the spectral hand
                SpectralHand hand = new SpectralHand(Caster, loc, Caster.Map);
                hand.MoveToWorld(loc, Caster.Map);

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private DeathsEmbrace m_Owner;

            public InternalTarget(DeathsEmbrace owner) : base(10, true, TargetFlags.Harmful)
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

        private class SpectralHand : Item
        {
            private Timer m_Timer;
            private List<Mobile> m_Targets;
            private Mobile m_Caster;

            public SpectralHand(Mobile caster, Point3D loc, Map map) : base(0x204E) // Ghostly Hand ItemID
            {
                Movable = false;
                Hue = 0x482; // Spectral Hue
                MoveToWorld(loc, map);
                m_Caster = caster;
                m_Targets = new List<Mobile>();

                // Periodically apply effects to targets within range
                m_Timer = new InternalTimer(this, m_Caster, loc, map, m_Targets);
                m_Timer.Start();
            }

            public SpectralHand(Serial serial) : base(serial)
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
                private SpectralHand m_Hand;
                private Mobile m_Caster;
                private Point3D m_Location;
                private Map m_Map;
                private List<Mobile> m_Targets;
                private DateTime m_End;

                public InternalTimer(SpectralHand hand, Mobile caster, Point3D loc, Map map, List<Mobile> targets) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
                {
                    m_Hand = hand;
                    m_Caster = caster;
                    m_Location = loc;
                    m_Map = map;
                    m_Targets = targets;
                    m_End = DateTime.Now + TimeSpan.FromSeconds(10.0); // Duration of the effect
                }

                protected override void OnTick()
                {
                    if (DateTime.Now >= m_End || m_Hand.Deleted)
                    {
                        m_Hand.Delete();
                        Stop();
                        return;
                    }

                    ArrayList targets = new ArrayList();
                    foreach (Mobile m in m_Hand.GetMobilesInRange(3)) // Range of effect
                    {
                        if (m != m_Caster && m.Alive && !m.IsDeadBondedPet && m.AccessLevel == AccessLevel.Player)
                        {
                            targets.Add(m);
                        }
                    }

                    foreach (Mobile m in targets)
                    {
                        m_Targets.Add(m);

                        // Apply slow effect
                        m.SendMessage("A spectral hand grips you, slowing your movement!");
                        m.Paralyzed = true;
                        m.Freeze(TimeSpan.FromSeconds(1.5)); // Freeze target for 1.5 seconds

                        // Apply damage over time
                        m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                        m.PlaySound(0x205); // Sound of whispering death
                        int damage = Utility.RandomMinMax(5, 10);
                        AOS.Damage(m, m_Caster, damage, 0, 0, 0, 100, 0); // Pure cold damage
                    }
                }
            }
        }
    }
}
