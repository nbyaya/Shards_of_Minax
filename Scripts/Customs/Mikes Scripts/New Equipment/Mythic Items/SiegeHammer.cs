using System;
using Server;
using Server.Items;
using Server.Multis;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
    public class SiegeHammer : Item
    {
        private Timer m_Timer;

        [Constructable]
        public SiegeHammer() : base(0x13E3)
        {
            Weight = 5.0;
            Name = "a siege hammer";
        }

        public SiegeHammer(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640); // The item must be in your backpack to use it.
                return;
            }

            from.SendMessage("Which house sign do you wish to demolish?");
            from.Target = new InternalTarget(this);
        }

		public void BeginSiege(Mobile from, BaseHouse house)
		{
				if (from.Map == Map.Malas)
			    {
					from.SendMessage("The siege hammer cannot be used in Malas.");
					return; // Exit the method if the player is in Malas.
				}
			// Get the current time in the server's local time zone.
			DateTime now = DateTime.Now;

			// Define the start and end time for the siege to be allowed.
			TimeSpan startTime = new TimeSpan(18, 0, 0); // 6 PM
			TimeSpan endTime = new TimeSpan(20, 0, 0); // 8 PM

			// Check if the current time is within the allowed time range.
			if (now.TimeOfDay >= startTime && now.TimeOfDay <= endTime)
			{
				if (house != null)
				{
					// Rest of your existing BeginSiege logic...
					if (m_Timer != null)
					m_Timer.Stop();

					m_Timer = new SiegeTimer(from, house, this);
					m_Timer.Start();
					from.SendMessage("You begin to siege the house, do not move!");
				}
			}
			else
			{
				// Send a message to the player if it is not the correct time.
				from.SendMessage("The siege hammer can only be used between 6 PM and 8 PM.");
			}
		}

        public void EndSiege(Mobile from)
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;
            from.SendMessage("You have stopped the siege.");
        }

		private class InternalTarget : Target
			{
				private SiegeHammer m_Hammer;

				public InternalTarget(SiegeHammer hammer) : base(-1, false, TargetFlags.None)
				{
					m_Hammer = hammer;
				}

				protected override void OnTarget(Mobile from, object targeted)
				{
					if (m_Hammer.Deleted)
						return;

					if (targeted is HouseSign && ((HouseSign)targeted).Owner != null) // Changed from Structure to Owner
					{
						BaseHouse house = ((HouseSign)targeted).Owner; // Changed from Structure to Owner

						if (house.IsOwner(from))
						{
							m_Hammer.BeginSiege(from, house);
						}
						else
						{
							m_Hammer.BeginSiege(from, house);
						}
					}
					else
					{
						from.SendLocalizedMessage(501265); // That is not a valid target.
					}
				}
			}

        private class SiegeTimer : Timer
        {
            private Mobile m_From;
            private BaseHouse m_House;
            private SiegeHammer m_Hammer;
            private DateTime m_End;

            public SiegeTimer(Mobile from, BaseHouse house, SiegeHammer hammer) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_House = house;
                m_Hammer = hammer;
                Priority = TimerPriority.OneSecond;
                m_End = DateTime.Now + TimeSpan.FromMinutes(30.0);
            }

            protected override void OnTick()
            {
                if (m_Hammer.Deleted || m_From.Deleted || m_House.Deleted || !m_From.Alive)
                {
                    m_Hammer.EndSiege(m_From);
                    m_Hammer.Delete();
                }
                else if (m_End <= DateTime.Now)
                {
                    m_From.SendMessage("The house has been successfully demolished.");
                    m_House.Delete();
                    m_Hammer.Delete();
                }
                else if (!m_From.InRange(m_House.Sign, 0))
                {
                    m_From.SendMessage("You have moved away from the siege!");
                    m_Hammer.EndSiege(m_From);
                    m_Hammer.Delete();
                }
            }
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
