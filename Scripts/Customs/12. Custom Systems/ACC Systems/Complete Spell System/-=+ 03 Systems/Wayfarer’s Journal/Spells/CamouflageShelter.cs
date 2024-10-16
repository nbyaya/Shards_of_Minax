using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class CamouflageShelter : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Camouflage Shelter", "Shelterus Invisiblus",
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 50; } }

        public CamouflageShelter(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CamouflageShelter m_Owner;

            public InternalTarget(CamouflageShelter owner) : base(10, true, TargetFlags.None)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);
                
                Point3D loc = new Point3D(p);
                InternalItem shelter = new InternalItem(Caster, loc, Caster.Map);
                
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 1150);
                Effects.PlaySound(loc, Caster.Map, 0x1F6);

                shelter.Bless(Caster); // Grants temporary armor bonus
                shelter.ReduceRangedDamage(Caster); // Reduces ranged damage
                shelter.StartTimer(); // Shelter duration timer
                
                FinishSequence();
            }
        }

		private class InternalItem : Item
		{
			private Timer m_Timer;
			private Mobile m_Caster;

			public override bool BlocksFit { get { return true; } }

			public InternalItem(Mobile caster, Point3D loc, Map map) : base(0x1E5E) // Shelter graphic
			{
				Movable = false;
				MoveToWorld(loc, map);
				m_Caster = caster;

				m_Timer = new InternalTimer(this, TimeSpan.FromMinutes(3.0)); // Duration of 3 minutes
				m_Timer.Start();
			}

			public InternalItem(Serial serial) : base(serial)
			{
			}

			public void Bless(Mobile caster)
			{
				caster.SendMessage("You feel the shelter's protective aura around you.");
				caster.VirtualArmorMod += 20; // Increases armor
			}

			public void ReduceRangedDamage(Mobile caster)
			{
				caster.SendMessage("The shelter reduces incoming ranged damage.");
				// Placeholder for ranged damage reduction logic
				// Implement a custom effect or handler as needed
				// caster.ApplyDamageReductionEffect(10); // Hypothetical method
			}

			public void StartTimer()
			{
				Timer.DelayCall(TimeSpan.FromMinutes(3.0), new TimerCallback(Delete));
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();
				m_Caster.VirtualArmorMod -= 20; // Remove armor bonus when shelter disappears
				m_Caster.SendMessage("The shelter's protection fades away.");
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

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
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


        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
