using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Engines.Quests.Samurai;

namespace Server.Items
{
    public class YoungNinjasShadowblade : Katana
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public YoungNinjasShadowblade()
        {
            Weight = 6.0;
            Name = "Young Ninja's Shadowblade";
            Hue = 1175; // Dark, shadowy color

            // Set attributes and bonuses
            Attributes.WeaponDamage = 25;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 10;
            Attributes.WeaponSpeed = 20;
            Attributes.Luck = 50;
            Attributes.SpellChanneling = 1; // Allows spellcasting while equipped
            Attributes.NightSight = 1; // Grants night sight

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Ninjitsu, 15.0);
            SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
            SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public YoungNinjasShadowblade(Serial serial) : base(serial)
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

                // Only start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonYoungNinjaTimer(pm);
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
            list.Add("Summons Young Ninjas");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances Ninjitsu and Stealth");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonYoungNinjaTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonYoungNinjaTimer : Timer
        {
            private Mobile m_Owner;

            public SummonYoungNinjaTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid, the item is not equipped, or autosummon is off
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is YoungNinjasShadowblade) || !AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    YoungNinja ninja = new YoungNinja
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ninja.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Young Ninja emerges from the shadows to serve you!");
                }
            }
        }
    }
}
