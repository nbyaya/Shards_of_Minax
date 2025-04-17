using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TurkeyTamerSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TurkeyTamerSash()
        {
            Weight = 1.0;
            Name = "Turkey Tamer's Sash";
            Hue = 1254; // Festive hue, tweak as needed

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Allows summoning more turkeys!
        }

        public TurkeyTamerSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a festive presence empowering your commands!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))  // Check if autosummon is enabled
                {
                    m_Timer = new SummonGiantTurkeyTimer(pm);
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
                pm.SendMessage(37, "The festive presence fades from your control.");
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
            list.Add("Summons Giant Turkeys");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))  // Check if autosummon is enabled
                {
                    m_Timer = new SummonGiantTurkeyTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGiantTurkeyTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGiantTurkeyTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is TurkeyTamerSash))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GiantTurkey turkey = new GiantTurkey
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    turkey.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Giant Turkey struts forward to serve you!");
                }
            }
        }
    }
}
