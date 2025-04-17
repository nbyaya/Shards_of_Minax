using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TurkeySummonersSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TurkeySummonersSash()
        {
            Weight = 1.0;
            Name = "Turkey Summoner's Sash";
            Hue = 1150; // A festive, turkey-like color (brownish-orange)

            // Set attributes and bonuses
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 5;
            Attributes.RegenHits = 2;
            Attributes.RegenStam = 2;
            Attributes.RegenMana = 2;
            Attributes.Luck = 50;
            Attributes.SpellDamage = 5;

            // Resistances
            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public TurkeySummonersSash(Serial serial) : base(serial)
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

                // Start summon timer if auto summon is enabled
                StopSummonTimer();
                m_Timer = new SummonTurkeyTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Only start the timer if autosummon is enabled
                {
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
            list.Add("Summons Turkeys");
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
                m_Timer = new SummonTurkeyTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if auto-summon is enabled after loading
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonTurkeyTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTurkeyTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is TurkeySummonersSash))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return; // If auto-summon is off, stop summoning.

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Turkey turkey = new Turkey
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    turkey.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Turkey emerges to aid you!");
                }
            }
        }
    }
}
