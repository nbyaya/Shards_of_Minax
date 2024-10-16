using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class MagicWard : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Magic Ward", "Sanctus Mana",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle => SpellCircle.Sixth;
        public override double CastDelay => 0.2;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 35;

        public MagicWard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Effects.PlaySound(loc, Caster.Map, 0x1F5); // Magic Ward sound effect
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Magic Ward visual effect

                InternalItem ward = new InternalItem(loc, Caster.Map, Caster);
                ward.MoveToWorld(loc, Caster.Map);

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x8E2) // Wall of Stone graphic
            {
                Movable = false;
                Hue = 1153; // Light blue hue for magical effect
                MoveToWorld(loc, map);

                m_Caster = caster;

                m_Timer = new RegenerationTimer(this, m_Caster);
                m_Timer.Start();

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), Delete); // Ward lasts 30 seconds
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class RegenerationTimer : Timer
            {
                private InternalItem m_Ward;
                private Mobile m_Caster;

                public RegenerationTimer(InternalItem ward, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
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

                    foreach (Mobile m in m_Ward.GetMobilesInRange(5))
                    {
                        if (m.Alive && m.Player)
                        {
                            m.Mana += 1 + (int)(m_Caster.Skills[SkillName.Magery].Value / 100); // Mana regeneration based on caster's Magery skill
                            m.FixedEffect(0x376A, 1, 32, 5008, 0); // Visual effect for mana regeneration
                        }
                    }
                }
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // Version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }

        private class InternalTarget : Target
        {
            private MagicWard m_Owner;

            public InternalTarget(MagicWard owner) : base(12, true, TargetFlags.None)
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
