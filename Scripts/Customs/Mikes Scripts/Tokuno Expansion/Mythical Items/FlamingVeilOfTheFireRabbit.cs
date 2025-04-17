using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FlamingVeilOfTheFireRabbit : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FlamingVeilOfTheFireRabbit()
        {
            Weight = 1.0;
            Name = "Flaming Veil of the Fire Rabbit";
            Hue = 1359; // Fiery red-orange hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            Resistances.Fire = 15;
            Resistances.Physical = 5;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public FlamingVeilOfTheFireRabbit(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel attuned to fiery companions!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonFireRabbitTimer(pm);
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
                pm.SendMessage(37, "You feel less attuned to fiery companions.");
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
            list.Add("Summons Fire Rabbits");
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
                m_Timer = new SummonFireRabbitTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonFireRabbitTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFireRabbitTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(8.0), TimeSpan.FromSeconds(8.0)) // Summons every 8 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FlamingVeilOfTheFireRabbit))
                {
                    Stop();
                    return;
                }

                // Check if auto summon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FireRabbit rabbit = new FireRabbit
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    rabbit.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Fire Rabbit appears to serve you!");
                }
            }
        }
    }
}
