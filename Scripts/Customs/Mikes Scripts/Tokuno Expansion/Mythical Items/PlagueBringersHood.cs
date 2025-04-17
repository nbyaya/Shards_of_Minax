using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PlagueBringersHood : LeatherCap
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PlagueBringersHood()
        {
            Weight = 1.0;
            Name = "Plague Bringer's Hood";
            Hue = 1175; // Sickly green hue

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusStam = 15;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;

            PhysicalBonus = 10;
            PoisonBonus = 20;
            EnergyBonus = 5;

            SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);

            ArmorAttributes.SelfRepair = 5;
            ArmorAttributes.MageArmor = 1;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public PlagueBringersHood(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the plague's influence granting you dominion over more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonPlagueRatTimer(pm);
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
                pm.SendMessage(37, "The plague's influence fades, and your dominion weakens.");
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
            list.Add("Summons Plague Rats");
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
                // Only start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonPlagueRatTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonPlagueRatTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPlagueRatTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is PlagueBringersHood))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers and autosummon is enabled
                if (m_Owner.Followers < m_Owner.FollowersMax && AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    PlagueRat rat = new PlagueRat
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    rat.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Plague Rat scurries forth to serve you!");
                }
            }
        }
    }
}
