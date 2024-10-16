using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class WardrobeOfHolding : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wardrobe of Holding", "Creoh Arca",
            21016,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; } // Adjust the circle as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 35; } }

        public WardrobeOfHolding(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WardrobeOfHolding m_Owner;

            public InternalTarget(WardrobeOfHolding owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    m_Owner.Target(p);
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                Effects.PlaySound(loc, map, 0x1F6);
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042);

                // Create the wardrobe item
                MagicWardrobe wardrobe = new MagicWardrobe(loc, map, Caster);
                wardrobe.MoveToWorld(loc, map);
            }

            FinishSequence();
        }

        private class MagicWardrobe : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public MagicWardrobe(Point3D loc, Map map, Mobile caster) : base(0x9A8) // ItemID for a wardrobe or similar
            {
                Movable = false;
                Hue = 0x489; // Magical color
                Name = "Magical Wardrobe of Holding";
                m_Caster = caster;

                MoveToWorld(loc, map);

                // Add magical visual effects
                Effects.PlaySound(Location, Map, 0x206);
                Effects.SendLocationParticles(this, 0x373A, 1, 15, 1153, 0, 0, 0);

                // Start the timer to remove the wardrobe after a duration
                m_Timer = new WardrobeTimer(this, TimeSpan.FromSeconds(60.0));
                m_Timer.Start();
            }

            public MagicWardrobe(Serial serial) : base(serial)
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

			public override void OnDoubleClick(Mobile from)
			{
				if (from == m_Caster || from.InRange(this, 2))
				{
					if (from.Backpack != null)
					{
						from.SendMessage("You access the magical wardrobe and find your stored items.");
						// Example: open the backpack (or any container) using the appropriate method.
						from.Use(from.Backpack); // This opens the backpack or container.
					}
				}
				else
				{
					from.SendMessage("You are too far away to access the magical wardrobe.");
				}
			}


            private class WardrobeTimer : Timer
            {
                private Item m_Item;

                public WardrobeTimer(Item item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (m_Item != null && !m_Item.Deleted)
                    {
                        m_Item.Delete();
                    }
                }
            }
        }
    }
}
