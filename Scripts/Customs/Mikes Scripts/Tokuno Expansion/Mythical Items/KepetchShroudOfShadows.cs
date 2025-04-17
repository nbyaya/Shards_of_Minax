using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class KepetchShroudOfShadows : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public KepetchShroudOfShadows()
        {
            Weight = 1.0;
            Name = "Kepetch Shroud of Shadows";
            Hue = 1175; // A shadowy color

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusInt = 10;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 100;
            Attributes.NightSight = 1;
            SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
            SkillBonuses.SetValues(2, SkillName.Tactics, 15.0);

            Resistances.Physical = 12;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 15;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public KepetchShroudOfShadows(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The shadows empower you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonKepetchAmbusherTimer(pm);
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
                pm.SendMessage(37, "You feel less in tune with the shadows.");
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
            list.Add("Summons Kepetch Ambushers");
            list.Add("Increases maximum followers");
            list.Add("Grants stealth and hiding mastery");
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
                // Only restart the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonKepetchAmbusherTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonKepetchAmbusherTimer : Timer
        {
            private Mobile m_Owner;

            public SummonKepetchAmbusherTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is KepetchShroudOfShadows))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    KepetchAmbusher ambusher = new KepetchAmbusher
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ambusher.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Kepetch Ambusher materializes from the shadows to aid you!");
                }
            }
        }
    }
}
