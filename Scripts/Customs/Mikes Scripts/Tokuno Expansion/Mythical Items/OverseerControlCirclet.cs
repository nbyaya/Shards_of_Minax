using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OverseerControlCirclet : Circlet
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OverseerControlCirclet()
        {
            Weight = 2.0;
            Name = "Overseer's Control Circlet";
            Hue = 1175; // Metallic dark hue

            // Set attributes and bonuses
            Attributes.BonusInt = 25;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 10;
            EnergyBonus = 15;

            SkillBonuses.SetValues(0, SkillName.Tinkering, 20.0);
            SkillBonuses.SetValues(1, SkillName.Mysticism, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OverseerControlCirclet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more minions of technology!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StartSummonTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding mechanical minions.");
            }

            // Stop the summon timer
            StopSummonTimer();
        }

        private void StartSummonTimer(PlayerMobile pm)
        {
            StopSummonTimer(); // Ensure we stop any existing timer before starting a new one
            m_Timer = new SummonExodusOverseerTimer(pm);
            m_Timer.Start();
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
            list.Add("Summons Exodus Overseers");
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
            if (Parent is PlayerMobile pm)
            {
                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StartSummonTimer(pm);
                }
            }
        }

        private class SummonExodusOverseerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonExodusOverseerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is OverseerControlCirclet))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and there is room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ExodusOverseer overseer = new ExodusOverseer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    overseer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Exodus Overseer materializes to serve you!");
                }
            }
        }
    }
}
