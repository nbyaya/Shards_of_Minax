using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class InfernalRiderCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public InfernalRiderCloak()
        {
            Weight = 1.0;
            Name = "Infernal Rider's Cloak";
            Hue = 1358; // A fiery red-orange hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusDex = 15;
            Attributes.BonusInt = 10;
            Attributes.RegenMana = 3;
            Attributes.RegenStam = 2;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 15;
            Attributes.CastSpeed = 1;
            Attributes.NightSight = 1;

            Resistances.Physical = 10;
            Resistances.Fire = 15;
            Resistances.Cold = 5;
            Resistances.Poison = 10;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public InfernalRiderCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a fiery presence empowering your command over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonHellSteedTimer(pm);
                    m_Timer.Start();
                }
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The fiery presence fades, weakening your command over creatures.");
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
            list.Add("Summons Hell Steeds");
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
                // Check if auto summon is enabled on reload
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonHellSteedTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonHellSteedTimer : Timer
        {
            private Mobile m_Owner;

            public SummonHellSteedTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is InfernalRiderCloak))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    HellSteed steed = new HellSteed
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    steed.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Hell Steed rises from the flames to serve you!");
                }
            }
        }
    }
}
