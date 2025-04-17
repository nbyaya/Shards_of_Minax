using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LavaLordsEmbrace : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LavaLordsEmbrace()
        {
            Weight = 1.0;
            Name = "Lava Lord's Embrace";
            Hue = 1254; // Fiery red hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 30;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.CastSpeed = 1;
            Attributes.ReflectPhysical = 10;

            Resistances.Physical = 8;
            Resistances.Fire = 20; // Strong fire resistance
            Resistances.Cold = -5; // Weakness to cold
            Resistances.Poison = 5;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public LavaLordsEmbrace(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an intense fiery presence bolstering your command!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonLavaSerpentTimer(pm);
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
                pm.SendMessage(37, "The fiery presence fades, and your command diminishes.");
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
            list.Add("Summons Lava Serpents");
            list.Add("Increases maximum followers by 2");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonLavaSerpentTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonLavaSerpentTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLavaSerpentTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is LavaLordsEmbrace))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    LavaSerpent serpent = new LavaSerpent
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    serpent.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Lava Serpent rises from the molten depths to serve you!");
                }
            }
        }
    }
}
