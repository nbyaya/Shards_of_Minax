using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RidgebackMasterSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RidgebackMasterSash()
        {
            Weight = 1.0;
            Name = "Ridgeback Master Sash";
            Hue = 1157; // A rugged, earthy color for Ridgebacks

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 15;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 10;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 10;
            Attributes.Luck = 50;
            Attributes.SpellDamage = 8;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);
            SkillBonuses.SetValues(3, SkillName.Camping, 10.0);
            SkillBonuses.SetValues(4, SkillName.Tracking, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public RidgebackMasterSash(Serial serial) : base(serial)
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
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonRidgebackTimer(pm);
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
            list.Add("Summons Ridgebacks");
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
                // Start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonRidgebackTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRidgebackTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRidgebackTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is RidgebackMasterSash))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers and autosummon is enabled
                if (m_Owner.Followers < m_Owner.FollowersMax && AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Ridgeback ridgeback = new Ridgeback
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ridgeback.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A loyal Ridgeback emerges to serve you!");
                }
            }
        }
    }
}
