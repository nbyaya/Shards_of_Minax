#region References
using System;
using System.Collections;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Mobiles;
#endregion

namespace Server.Items
{
	public abstract class BasePoisonBomb : BasePotion
	{
		private static readonly bool LeveledExplosion = true; // Should explosion potions explode other nearby potions?
		private static readonly bool InstantExplosion = true; // Should explosion potions explode on impact?
		private const int ExplosionRange = 6; // How long is the blast radius?
		private Timer m_Timer;
		private ArrayList m_Users;

		public BasePoisonBomb(PotionEffect effect)
			: base(0xF0D, effect)
		{ }

		public BasePoisonBomb(Serial serial)
			: base(serial)
		{ }
		
		public abstract int MinDamage { get; }
		public abstract int MaxDamage { get; }

		public override bool RequireFreeHand { get { return false; } }

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

		public virtual object FindParent(Mobile from)
		{
			Mobile m = HeldBy;

			if (m != null && m.Holding == this)
			{
				return m;
			}

			object obj = RootParent;

			if (obj != null)
			{
				return obj;
			}

			if (Map == Map.Internal)
			{
				return from;
			}

			return this;
		}

		public override void Drink(Mobile from)
		{
			if (Core.AOS && (from.Paralyzed || from.Frozen || (from.Spell != null && from.Spell.IsCasting)))
			{
				from.SendLocalizedMessage(1062725); // You can not use a potion while paralyzed.
				return;
			}

			ThrowTarget targ = from.Target as ThrowTarget;
			Stackable = false; // Scavenged explosion potions won't stack with those ones in backpack, and still will explode.

			if (targ != null && targ.Potion == this)
			{
				return;
			}

			from.RevealingAction();

			if (m_Users == null)
			{
				m_Users = new ArrayList();
			}

			if (!m_Users.Contains(from))
			{
				m_Users.Add(from);
			}

			from.Target = new ThrowTarget(this);

			if (m_Timer == null)
			{
				from.SendLocalizedMessage(500236); // You should throw it now!

				m_Timer = Timer.DelayCall(
					TimeSpan.FromSeconds(1.0),
					TimeSpan.FromSeconds(1.25),
					5,
					new TimerStateCallback(Detonate_OnTick),
					new object[] {from, 3}); // 3.6 seconds explosion delay
			}
		}

		public void Explode(Mobile from, bool direct, Point3D loc, Map map)
		{
			if (Deleted)
			{
				return;
			}

			Consume();

			for (int i = 0; m_Users != null && i < m_Users.Count; ++i)
			{
				Mobile m = (Mobile)m_Users[i];
				ThrowTarget targ = m.Target as ThrowTarget;

				if (targ != null && targ.Potion == this)
				{
					Target.Cancel(m);
				}
			}

			if (map == null)
			{
				return;
			}

			Effects.PlaySound(loc, map, 0x231);

			// Add poison cloud effect
			Effects.SendLocationEffect(loc, map, 0x11A6, 10, 1, 1167, 0); // Poison cloud visual effect

			IPooledEnumerable eable = LeveledExplosion
										  ? map.GetObjectsInRange(loc, ExplosionRange)
										  : (IPooledEnumerable)map.GetMobilesInRange(loc, ExplosionRange);
			ArrayList toExplode = new ArrayList();

			foreach (object o in eable)
			{
				if (o is Mobile &&
					(from == null || (SpellHelper.ValidIndirectTarget(from, (Mobile)o) && from.CanBeHarmful((Mobile)o, false) && from.InLOS((Mobile)o))))
				{
					toExplode.Add(o);
				}
				else if (o is BasePoisonBomb && o != this)
				{
					toExplode.Add(o);
				}
			}

			eable.Free();

			int min = Scale(from, MinDamage);
			int max = Scale(from, MaxDamage);

			for (int i = 0; i < toExplode.Count; ++i)
			{
				object o = toExplode[i];

				if (o is Mobile)
				{
					Mobile m = (Mobile)o;

					if (from != null)
					{
						from.DoHarmful(m);
					}

					int damage = Utility.RandomMinMax(min, max);

					if (!Core.AOS && damage > 40)
					{
						damage = 40;
					}

					AOS.Damage(m, from, damage, 0, 0, 0, 100, 0, Server.DamageType.SpellAOE);

					// Inflict lethal poison
					m.ApplyPoison(from, Poison.Lethal);
				}
				else if (o is BasePoisonBomb)
				{
					BasePoisonBomb pot = (BasePoisonBomb)o;

					pot.Explode(from, false, pot.GetWorldLocation(), pot.Map);
				}
			}
			
			TriggerFlamestrikeWave(loc, map);
		}
		
		private void TriggerFlamestrikeWave(Point3D center, Map map)
		{
			for (int radius = 0; radius <= ExplosionRange; radius++)
			{
				Timer.DelayCall(TimeSpan.FromMilliseconds(10 * radius), () =>
				{
					for (int x = -radius; x <= radius; x++)
					{
						for (int y = -radius; y <= radius; y++)
						{
							if (x * x + y * y <= radius * radius)
							{
								Point3D loc = new Point3D(center.X + x, center.Y + y, center.Z);
								Effects.SendLocationEffect(loc, map, 0x11A6, 30, 10, 2511, 0); // Poision Cloud with purple hue
							}
						}
					}
				});
			}
		}

		private void Detonate_OnTick(object state)
		{
			if (Deleted)
			{
				return;
			}

			var states = (object[])state;
			Mobile from = (Mobile)states[0];
			int timer = (int)states[1];

			object parent = FindParent(from);

			if (timer == 0)
			{
				Point3D loc;
				Map map;

				if (parent is Item)
				{
					Item item = (Item)parent;

					loc = item.GetWorldLocation();
					map = item.Map;
				}
				else if (parent is Mobile)
				{
					Mobile m = (Mobile)parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}

				Explode(from, true, loc, map);
				m_Timer = null;
			}
			else
			{
				if (parent is Item)
				{
					((Item)parent).PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
				}
				else if (parent is Mobile)
				{
					((Mobile)parent).PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
				}

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick(object state)
		{
			if (Deleted)
			{
				return;
			}

			var states = (object[])state;
			Mobile from = (Mobile)states[0];
			IPoint3D p = (IPoint3D)states[1];
			Map map = (Map)states[2];

			Point3D loc = new Point3D(p);

			if (InstantExplosion)
			{
				Explode(from, true, loc, map);
			}
			else
			{
				MoveToWorld(loc, map);
			}
		}

		private class ThrowTarget : Target
		{
			private readonly BasePoisonBomb m_Potion;

			public ThrowTarget(BasePoisonBomb potion)
				: base(12, true, TargetFlags.None)
			{
				m_Potion = potion;
			}

			public BasePoisonBomb Potion { get { return m_Potion; } }

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Potion.Deleted || m_Potion.Map == Map.Internal)
				{
					return;
				}

				IPoint3D p = targeted as IPoint3D;

				if (p == null)
				{
					return;
				}

				Map map = from.Map;

				if (map == null)
				{
					return;
				}

				SpellHelper.GetSurfaceTop(ref p);

				from.RevealingAction();

				IEntity to;

				to = new Entity(Serial.Zero, new Point3D(p), map);

				if (p is Mobile)
				{
					to = (Mobile)p;
				}

				Effects.SendMovingEffect(from, to, m_Potion.ItemID, 7, 0, false, false, m_Potion.Hue, 0);

				if (m_Potion.Amount > 1)
				{
					Mobile.LiftItemDupe(m_Potion, 1);
				}

				m_Potion.Internalize();
				Timer.DelayCall(
					TimeSpan.FromSeconds(1.0), new TimerStateCallback(m_Potion.Reposition_OnTick), new object[] {from, p, map});
			}
		}
	}
}
