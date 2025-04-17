using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FerretSummonersCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FerretSummonersCloak()
        {
            Weight = 2.0;
            Name = "Ferret Summoner's Cloak";
            Hue = 1153; // A soft grey-brown hue to represent ferrets.

            // Set attributes and bonuses
            Attributes.BonusDex = 25;
            Attributes.BonusStam = 15;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;

            SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);

            Resistances.Physical = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FerretSummonersCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the presence of ferrets at your side!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonFerretTimer(pm);
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
                pm.SendMessage(37, "You feel the ferrets scatter away.");
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
            list.Add("Summons Ferrets");
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
                m_Timer = new SummonFerretTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonFerretTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFerretTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FerretSummonersCloak))
                {
                    Stop();
                    return;
                }

                // Check if auto summon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Ferret ferret = new Ferret
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ferret.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A playful ferret scurries to your side!");
                }
            }
        }
    }
}
