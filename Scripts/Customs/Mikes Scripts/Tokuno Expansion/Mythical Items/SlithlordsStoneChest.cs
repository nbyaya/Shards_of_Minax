using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SlithlordsStoneChest : ChainChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SlithlordsStoneChest()
        {
            Weight = 10.0;
            Name = "Slithlord's Stone Chest";
            Hue = 1166; // A dark, reptilian green color

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusDex = 10;
            Attributes.BonusHits = 20;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 15;
            Attributes.AttackChance = 10;
            Attributes.Luck = 100;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 15;
            FireBonus = 10;
            PoisonBonus = 20; // Sliths are often associated with poison

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(1, SkillName.Wrestling, 15.0); // Sliths are melee-focused

            // Follower bonus
            m_BonusFollowers = 2; // Grants +2 followers

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SlithlordsStoneChest(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you could command more creatures now!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonSlithTimer(pm);
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
                pm.SendMessage(37, "You feel like you cannot command as many creatures as before.");
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
            list.Add("Summons Sliths to fight for you");
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
                // Check if autosummon is enabled before restarting the timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSlithTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSlithTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSlithTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is SlithlordsStoneChest))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Slith slith = new Slith
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    slith.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Slith emerges to serve you!");
                }
            }
        }
    }
}
