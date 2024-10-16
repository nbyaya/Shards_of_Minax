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
    public class SigilOfHealing : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sigil of Healing", "An Aesco Salutem",
            //SpellCircle.Fifth, // Placeholder for the spell circle if needed
            21004, 9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public SigilOfHealing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Effects.PlaySound(loc, Caster.Map, 0x1F2); // Play sound when sigil is created

                HealingPulseTile sigil = new HealingPulseTile();
                sigil.MoveToWorld(loc, Caster.Map);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SigilOfHealing m_Owner;

            public InternalTarget(SigilOfHealing owner) : base(10, true, TargetFlags.None)
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

        private class SigilOfHealingItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public SigilOfHealingItem(Mobile caster) : base(0x1F18) // ID for a sigil-like item
            {
                Movable = false;
                Visible = true;
                Name = "Sigil of Healing";
                Hue = 1150; // Light blue hue for healing

                m_Caster = caster;

                m_Timer = new HealTimer(this, m_Caster);
                m_Timer.Start();
            }

            public SigilOfHealingItem(Serial serial) : base(serial)
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

            private class HealTimer : Timer
            {
                private SigilOfHealingItem m_Sigil;
                private Mobile m_Caster;

                public HealTimer(SigilOfHealingItem sigil, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(2.0))
                {
                    Priority = TimerPriority.TwoFiftyMS;
                    m_Sigil = sigil;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Sigil.Deleted)
                    {
                        Stop();
                        return;
                    }

                    List<Mobile> targets = new List<Mobile>();

                    foreach (Mobile m in m_Sigil.GetMobilesInRange(3)) // Range of 3 tiles
                    {
                        if (m != null && m.Alive && m.Player && m.Hits < m.HitsMax)
                        {
                            targets.Add(m);
                        }
                    }

                    foreach (Mobile m in targets)
                    {
                        int healAmount = Utility.RandomMinMax(5, 10); // Heal 5-10 health each tick
                        m.Heal(healAmount);
                        m.FixedParticles(0x376A, 9, 32, 5005, 1153, 0, EffectLayer.Waist); // Healing effect
                        m.PlaySound(0x1F2); // Healing sound
                    }
                }
            }
        }
    }
}
