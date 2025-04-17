using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class InfernosEmbraceCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public InfernosEmbraceCloak()
        {
            Weight = 1.0;
            Name = "Inferno's Embrace Cloak";
            Hue = 1359; // Fiery hue, adjust as needed.

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 4;
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.LowerManaCost = 10;

            Resistances.Physical = 5;
            Resistances.Fire = 20; // Strong fire resistance
            Resistances.Cold = -5; // Slight penalty to balance
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public InfernosEmbraceCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the fiery power of control surge through you!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonFireElementalTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
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
                pm.SendMessage(37, "You feel the fiery power of control fade.");
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
            list.Add("Summons Fire Elementals");
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
                m_Timer = new SummonFireElementalTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                    m_Timer.Start();
            }
        }

        private class SummonFireElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFireElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust spawn rate as needed
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is InfernosEmbraceCloak))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FireElemental elemental = new FireElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Fire Elemental emerges to serve you!");
                }
            }
        }
    }
}
