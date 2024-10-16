using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class GlyphOfSilence : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Glyph of Silence", "Silencio Glyphus",
            21004, // Effect sound ID
            9300,  // Glyph visual ID
            Reagent.BlackPearl, 
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 30; } }

        public GlyphOfSilence(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Map map = Caster.Map;

                Effects.PlaySound(loc, map, 0x213); // Silence area sound effect

                InternalItem glyph = new InternalItem(loc, map, Caster);
                glyph.MoveToWorld(loc, map);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private DateTime m_End;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1F13) // Use an appropriate item ID for the glyph
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(20.0)); // Glyph lasts for 20 seconds
                m_Timer.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(20.0);
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
                writer.Write(m_End - DateTime.Now);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                TimeSpan duration = reader.ReadTimeSpan();

                m_Timer = new InternalTimer(this, duration);
                m_Timer.Start();

                m_End = DateTime.Now + duration;
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

                public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }

            public override bool OnMoveOver(Mobile m)
            {
                base.OnMoveOver(m);
                if (m.Player || m is BaseCreature)
                {
                    m.SendMessage("You feel a sudden silence around you, preventing you from casting spells!");

                    // Start a custom silence effect
                    new SilenceTimer(m, TimeSpan.FromSeconds(10.0)).Start();
                }
                return false; // Indicate that the item does not block movement
            }
        }

        private class SilenceTimer : Timer
        {
            private Mobile m_Mobile;

            public SilenceTimer(Mobile mobile, TimeSpan duration) : base(duration)
            {
                m_Mobile = mobile;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    // Remove the silence effect
                    m_Mobile.SendMessage("The silence around you fades, and you can cast spells again.");
                }
            }
        }

        private class InternalTarget : Target
        {
            private GlyphOfSilence m_Owner;

            public InternalTarget(GlyphOfSilence owner) : base(12, true, TargetFlags.None)
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
