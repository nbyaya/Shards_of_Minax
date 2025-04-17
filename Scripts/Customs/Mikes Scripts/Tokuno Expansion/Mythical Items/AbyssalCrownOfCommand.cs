using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AbyssalCrownOfCommand : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AbyssalCrownOfCommand()
        {
            Weight = 5.0;
            Name = "Abyssal Crown of Command";
            Hue = 1175; // Dark Abyssal color

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 20;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AbyssalCrownOfCommand(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more dark beings.");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonAbysmalHorrorTimer(pm);
                m_Timer.Start();
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The power to command dark beings fades away.");
            }

            // Stop the summon timer
            StopSummonTimer();
        }

        private void StopSummonTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Abysmal Horrors");
            list.Add("Increases maximum followers");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_BonusFollowers);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_BonusFollowers = reader.ReadInt();

            // Reinitialize timer if equipped on restart
            if (Parent is Mobile mob)
            {
                m_Timer = new SummonAbysmalHorrorTimer(mob);
                m_Timer.Start();
            }
        }

		private class SummonAbysmalHorrorTimer : Timer
		{
			private Mobile m_Owner;

			public SummonAbysmalHorrorTimer(Mobile owner)
				: base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summons every 15 seconds
			{
				m_Owner = owner;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				// Stop if owner is invalid or the item is no longer equipped
				if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is AbyssalCrownOfCommand))
				{
					Stop();
					return;
				}

				// Check if the player has autosummon enabled
				if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
					return;

				if (m_Owner.Followers < m_Owner.FollowersMax)
				{
					AbysmalHorror horror = new AbysmalHorror
					{
						Controlled = true,
						ControlMaster = m_Owner
					};

					horror.MoveToWorld(m_Owner.Location, m_Owner.Map);
					m_Owner.SendMessage(38, "An Abysmal Horror is summoned to serve you!");
				}
			}
		}

    }
}
