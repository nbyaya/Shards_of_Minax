using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class MirageWard : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mirage Ward", "An Lor Xen Ylem",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 30;

        public MirageWard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                SpellHelper.Turn(Caster, p);

                IPoint3D surfaceTop = loc;  // Create a temporary IPoint3D variable
                SpellHelper.GetSurfaceTop(ref surfaceTop);  // Pass it by reference

                loc = new Point3D(surfaceTop);  // Convert back to Point3D after modification

                Effects.PlaySound(loc, map, 0x20E); // Play a mystical sound effect
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052); // Show visual effect

                InternalItem ward = new InternalItem(loc, map, Caster);
                ward.MoveToWorld(loc, map);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;
            private static readonly TimeSpan WardDuration = TimeSpan.FromSeconds(30.0); // Set duration of the ward

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x3946)
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                m_Timer = new WardTimer(this, m_Caster);
                m_Timer.Start();

                // Start a timer to delete the item after the ward duration
                Timer.DelayCall(WardDuration, Delete);
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0); // version
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

            private class WardTimer : Timer
            {
                private InternalItem m_Ward;
                private Mobile m_Caster;

                public WardTimer(InternalItem ward, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
                {
                    m_Ward = ward;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Ward.Deleted)
                    {
                        Stop();
                        return;
                    }

                    foreach (Mobile mob in m_Ward.GetMobilesInRange(3))
                    {
                        if (mob.Hidden && mob.AccessLevel == AccessLevel.Player)
                        {
                            mob.RevealingAction();
                            mob.SendMessage("You have been revealed by the Mirage Ward!");
                            Effects.SendTargetParticles(mob, 0x376A, 1, 13, 9944, 3, 3, EffectLayer.Head, 0); // Ensure to pass all required parameters
                        }

                        if (mob.Alive && mob is PlayerMobile player && player.Alive && !mob.Hidden)
                        {
                            player.SendMessage("You feel a surge of speed as you pass through the Mirage Ward!");
                            player.AddStatMod(new StatMod(StatType.Dex, "MirageWardSpeedBoost", 10, TimeSpan.FromSeconds(10)));
                        }
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private MirageWard m_Owner;

            public InternalTarget(MirageWard owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
