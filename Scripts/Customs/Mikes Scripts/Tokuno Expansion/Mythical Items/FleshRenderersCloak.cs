using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FleshRenderersCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FleshRenderersCloak()
        {
            Weight = 1.0;
            Name = "FleshRenderer's Cloak";
            Hue = 1175; // Dark, ominous hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 5;
            Attributes.BonusStr = 25;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.NightSight = 1;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 5;
            
            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 15;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FleshRenderersCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the dark power of command coursing through you.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) 
                {
                    m_Timer = new SummonFleshRendererTimer(pm);
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
                pm.SendMessage(37, "The dark power fades from your grasp.");
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
            list.Add("Summons FleshRenderers");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonFleshRendererTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFleshRendererTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFleshRendererTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FleshRenderersCloak))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FleshRenderer fleshRenderer = new FleshRenderer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    fleshRenderer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A FleshRenderer manifests from the shadows to serve you!");
                }
            }
        }
    }
}
