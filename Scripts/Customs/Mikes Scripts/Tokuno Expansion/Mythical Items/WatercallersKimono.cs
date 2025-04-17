using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WatercallersKimono : FancyDress
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WatercallersKimono()
        {
            Weight = 2.0;
            Name = "Watercaller's Kimono";
            Hue = 1365; // Aqua blue color for water theme

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 8;

            Resistances.Physical = 8;
            Resistances.Fire = 6;
            Resistances.Cold = 12;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WatercallersKimono(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an affinity with aquatic creatures!");

                // Only start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer
                    StopSummonTimer();
                    m_Timer = new SummonKappaTimer(pm);
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
                pm.SendMessage(37, "Your connection to the water realm fades.");
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
            list.Add("Summons Kappa to aid you");
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
                // Start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonKappaTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonKappaTimer : Timer
        {
            private Mobile m_Owner;

            public SummonKappaTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is WatercallersKimono))
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
                    Kappa kappa = new Kappa
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    kappa.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Kappa emerges from the water to serve you!");
                }
            }
        }
    }
}
