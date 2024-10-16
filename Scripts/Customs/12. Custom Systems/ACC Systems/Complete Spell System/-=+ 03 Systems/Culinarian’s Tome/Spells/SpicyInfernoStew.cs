using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class SpicyInfernoStew : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Spicy Inferno Stew", "Stew Vas Flam",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public SpicyInfernoStew(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SpicyInfernoStew m_Owner;

            public InternalTarget(SpicyInfernoStew owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                    m_Owner.Target(p);
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                // Summon a fiery stew pot
                InternalItem stew = new InternalItem(Caster, loc, map);
                stew.MoveToWorld(loc, map);

                // Effects and sound
                Effects.PlaySound(loc, map, 0x208);
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Mobile m_Caster;
            private Timer m_Timer;

			[Constructable]
            public InternalItem(Mobile caster, Point3D loc, Map map) : base(0x15FE) // Fiery stew pot graphic
            {
                Name = "Inferno Stew";
				Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                // Duration of the fiery aura effect
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
                private Item m_Item;
                private Mobile m_Caster;
                private DateTime m_End;

                public InternalTimer(Item item, Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
                {
                    Priority = TimerPriority.FiftyMS;
                    m_Item = item;
                    m_Caster = caster;
                    m_End = DateTime.Now + TimeSpan.FromSeconds(30.0); // Effect duration
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted || DateTime.Now > m_End)
                    {
                        m_Item.Delete();
                        Stop();
                        return;
                    }

                    // Apply fiery aura effect to nearby enemies
                    ArrayList list = new ArrayList();

                    foreach (Mobile m in m_Item.GetMobilesInRange(3))
                    {
                        if (m != m_Caster && m.Alive && !m.IsDeadBondedPet && m.AccessLevel == AccessLevel.Player)
                        {
                            list.Add(m);
                        }
                    }

                    for (int i = 0; i < list.Count; ++i)
                    {
                        Mobile m = (Mobile)list[i];

                        m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                        m.PlaySound(0x208);
                        m.Damage(Utility.RandomMinMax(5, 15), m_Caster); // Random fire damage
                    }
                }
            }
        }
    }
}
