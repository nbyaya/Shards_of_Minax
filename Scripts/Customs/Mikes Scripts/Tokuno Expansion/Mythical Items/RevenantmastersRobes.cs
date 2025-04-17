using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RevenantmastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RevenantmastersRobes()
        {
            Weight = 2.0;
            Name = "Revenantmaster's Robes";
            Hue = 1109; // Ghostly gray

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 15;
            Attributes.BonusHits = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 7;
            Attributes.RegenHits = 3;
            Attributes.WeaponDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 3;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.Luck = 150;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 15;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
            SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);
            SkillBonuses.SetValues(3, SkillName.Meditation, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public RevenantmastersRobes(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more minions!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonRevenantTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding minions.");
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
            list.Add("Summons Revenants");
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
                    m_Timer = new SummonRevenantTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRevenantTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRevenantTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is RevenantmastersRobes))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Skeleton revenant = new Skeleton
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    revenant.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Revenant rises from the shadows to serve you!");
                }
            }
        }
    }
}
