using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SerpentineWarlordsRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SerpentineWarlordsRobe()
        {
            Weight = 2.0;
            Name = "Serpentine Warlord's Robe";
            Hue = 58; // Greenish hue to match the Serpentine Dragon theme

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.BonusInt = 10;
            Attributes.BonusHits = 20;
            Attributes.BonusStam = 20;
            Attributes.BonusMana = 20;
            Attributes.RegenHits = 5;
            Attributes.RegenStam = 5;
            Attributes.RegenMana = 5;
            Attributes.WeaponDamage = 20;
            Attributes.DefendChance = 15;
            Attributes.AttackChance = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 15;
            Attributes.ReflectPhysical = 10;
            Attributes.SpellDamage = 12;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 20; // Increased poison resistance for thematic purposes
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(2, SkillName.Meditation, 15.0);
            SkillBonuses.SetValues(3, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(4, SkillName.Wrestling, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increased follower count
        }

        public SerpentineWarlordsRobe(Serial serial) : base(serial)
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
                StartSummonTimer(pm);
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
            // If autosummon is enabled, start the summon timer
            if (AutoSummonManager.IsAutoSummonEnabled(pm))
            {
                StopSummonTimer();
                m_Timer = new SummonSerpentineDragonTimer(pm);
                m_Timer.Start();
            }
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
            list.Add("Summons Serpentine Dragons");
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
            if (Parent is PlayerMobile pm)
            {
                StartSummonTimer(pm);
            }
        }

        // Method to handle when the autosummon toggle is changed
        public void OnAutoSummonToggle(PlayerMobile pm)
        {
            if (Parent != null && Parent == pm)
            {
                // Stop and restart the timer based on toggle
                StopSummonTimer();
                StartSummonTimer(pm);
            }
        }

        private class SummonSerpentineDragonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSerpentineDragonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Slower summon rate for balance
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is SerpentineWarlordsRobe))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SerpentineDragon dragon = new SerpentineDragon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Serpentine Dragon emerges to serve you!");
                }
            }
        }
    }
}
