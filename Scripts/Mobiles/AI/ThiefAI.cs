#region References
using System;

using Server.Items;
#endregion

namespace Server.Mobiles
{
	public class ThiefAI : BaseAI
	{
		private Item m_toDisarm;

		public ThiefAI(BaseCreature m)
			: base(m)
		{ }

		public override bool DoActionWander()
		{
			m_Mobile.DebugSay("I have no combatant");

			if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
			{
				m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;
			}
			else
			{
				base.DoActionWander();
			}

			return true;
		}

		public override bool DoActionCombat()
		{
			var c = m_Mobile.Combatant as Mobile;

			if (c == null || c.Deleted || c.Map != m_Mobile.Map)
			{
				m_Mobile.DebugSay("My combatant is gone, so my guard is up");
				Action = ActionType.Guard;
				return true;
			}

			if (WalkMobileRange(c, 1, true, m_Mobile.RangeFight, m_Mobile.RangeFight))
			{
				if (!DirectionLocked)
					m_Mobile.Direction = m_Mobile.GetDirectionTo(c);

				if (m_toDisarm == null || m_toDisarm.Deleted)
				{
					m_toDisarm = c.FindItemOnLayer(Layer.OneHanded) ?? c.FindItemOnLayer(Layer.TwoHanded);
				}

				if (m_toDisarm != null && m_toDisarm.IsChildOf(m_Mobile.Backpack))
				{
					// Already disarmed and stolen
					m_toDisarm = null;
				}

				// Attempt to take disarmed weapon
				if (m_toDisarm != null && m_toDisarm.IsChildOf(c.Backpack))
				{
					if (CanStealItem(m_toDisarm))
					{
						m_Mobile.DebugSay("Directly stealing weapon from backpack.");
						StealItemDirectly(m_toDisarm, c);
						m_toDisarm = null;
					}
				}
				else
				{
					var cpack = c.Backpack;

					if (cpack != null)
					{
						var targets = new Type[]
						{
							typeof(Bandage),
							typeof(Nightshade),
							typeof(BlackPearl),
							typeof(MandrakeRoot)
						};

						foreach (var t in targets)
						{
							var item = cpack.FindItemByType(t);

							if (item != null && CanStealItem(item))
							{
								m_Mobile.DebugSay($"Directly stealing {item.Name ?? item.GetType().Name} from backpack.");
								StealItemDirectly(item, c);
								break;
							}
						}
					}
				}
			}
			else
			{
				m_Mobile.DebugSay("I should be closer to {0}", c.Name);
			}

			// Flee if low health
			if (!m_Mobile.Controlled && !m_Mobile.Summoned && m_Mobile.CanFlee)
			{
				if (m_Mobile.Hits < m_Mobile.HitsMax * 0.2)
				{
					if (Utility.Random(100) <= Math.Max(10, 10 + c.Hits - m_Mobile.Hits))
					{
						m_Mobile.DebugSay("I am going to flee from {0}", c.Name);
						Action = ActionType.Flee;
					}
				}
			}

			return true;
		}

		private bool CanStealItem(Item item)
		{
			if (item == null || item.Deleted)
				return false;

			if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
				return false;

			if (!item.Movable)
				return false;

			return true;
		}

		private void StealItemDirectly(Item item, Mobile from)
		{
			if (item == null || from == null || item.Deleted)
				return;

			if (item.ParentEntity is Container parent && parent.Items.Contains(item))
			{
				parent.Items.Remove(item);
				m_Mobile.AddToBackpack(item);

				m_Mobile.DebugSay("I stole {0} from {1}.", item.Name ?? item.GetType().Name, from.Name);
			}
		}


		public override bool DoActionGuard()
		{
			if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
			{
				m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;
			}
			else
			{
				base.DoActionGuard();
			}

			return true;
		}
	}
}