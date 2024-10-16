using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class GlyphOfDestruction : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Glyph of Destruction", "Exs Por",
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.SulfurousAsh,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public GlyphOfDestruction(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private GlyphOfDestruction m_Owner;

            public InternalTarget(GlyphOfDestruction owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D point)
                {
                    m_Owner.Target(point);
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

            SpellHelper.GetSurfaceTop(ref p);

            if (CheckSequence())
            {
                Point3D loc = new Point3D(p);

                // Create the glyph
                Glyph glyph = new Glyph(Caster, loc, Caster.Map);
                glyph.MoveToWorld(loc, Caster.Map);

                Caster.SendMessage("You have created a Glyph of Destruction!");

                Effects.PlaySound(loc, Caster.Map, 0x1F3); // Explosive sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052); // Red explosions

                FinishSequence();
            }
        }

        private class Glyph : Item
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public Glyph(Mobile caster, Point3D loc, Map map) : base(0x1F13)
            {
                Movable = false;
                m_Caster = caster;
                MoveToWorld(loc, map);

                m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(OnActivate));
            }

            private void OnActivate()
            {
                if (Deleted)
                    return;

                ArrayList list = new ArrayList();

                foreach (Mobile m in GetMobilesInRange(2))
                {
                    if (m is BaseCreature && m != m_Caster)
                    {
                        list.Add(m);
                    }
                }

                if (list.Count > 0)
                {
                    foreach (Mobile m in list)
                    {
                        int damage = Utility.RandomMinMax(20, 40);

                        m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                        m.PlaySound(0x307); // Explosion sound
                        AOS.Damage(m, m_Caster, damage, 100, 0, 0, 0, 0);

                        m_Caster.SendMessage($"The glyph deals {damage} damage to {m.Name}!");
                    }
                }

                Delete();
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();
                if (m_Timer != null)
                    m_Timer.Stop();
            }

            public Glyph(Serial serial) : base(serial)
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
        }
    }
}
