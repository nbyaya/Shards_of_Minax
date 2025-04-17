using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ShadowmastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ShadowmastersRobes()
        {
            Weight = 2.0;
            Name = "Shadowmasters Robes";
            Hue = 1109; // Dark shadowy color

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 15;
            Attributes.BonusHits = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 6;
            Attributes.DefendChance = 12;
            Attributes.AttackChance = 12;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);
            SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public ShadowmastersRobes(Serial serial) : base(serial)
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

                // Check if autosummon is enabled and start the summon timer
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
                pm.SendMessage(37, "You feel like you cannot command as many creatures as before.");
            }

            // Stop the summon timer
            StopSummonTimer();
        }

        private void StartSummonTimer(PlayerMobile pm)
        {
            StopSummonTimer(); // Ensure any existing timer is stopped before starting a new one.
            m_Timer = new SummonShadowIronElementalTimer(pm);
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
            list.Add("Summons Shadow Iron Elementals");
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
                    m_Timer = new SummonShadowIronElementalTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonShadowIronElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonShadowIronElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is ShadowmastersRobes))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ShadowIronElemental elemental = new ShadowIronElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Shadow Iron Elemental emerges to serve you!");
                }
            }
        }
    }
}
