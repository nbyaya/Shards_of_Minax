using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WildTigerSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WildTigerSash()
        {
            Weight = 1.0;
            Name = "Sash of the Wild Tiger";
            Hue = 1150; // A vibrant jungle green color

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 15;
            Attributes.WeaponDamage = 20;
            Attributes.Luck = 100;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Poison = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem for potential leveling system
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public WildTigerSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a primal connection to the wild, allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonWildTigerTimer(pm);
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
                pm.SendMessage(37, "Your connection to the wild fades, reducing your ability to command creatures.");
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
            list.Add("Summons Wild Tigers");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances combat and taming abilities");
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
                    m_Timer = new SummonWildTigerTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWildTigerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWildTigerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is WildTigerSash))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    WildTiger tiger = new WildTiger
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    tiger.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wild Tiger emerges from the jungle to serve you!");
                }
            }
        }
    }
}
