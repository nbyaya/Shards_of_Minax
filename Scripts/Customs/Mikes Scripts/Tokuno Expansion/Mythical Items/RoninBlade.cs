using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Engines.Quests.Samurai;

namespace Server.Items
{
    public class RoninBlade : Katana
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RoninBlade()
        {
            Weight = 6.0;
            Name = "Ronin's Blade";
            Hue = 1175; // A unique dark-steel color for aesthetic

            // Set attributes and bonuses
            Attributes.BonusDex = 10;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.AttackChance = 10;
            Attributes.WeaponSpeed = 20;
            Attributes.WeaponDamage = 25;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Bushido, 20.0); // Thematic with Ronin
            SkillBonuses.SetValues(1, SkillName.Parry, 15.0);

            // Weapon-specific attributes
            WeaponAttributes.HitLowerAttack = 25;
            WeaponAttributes.HitLightning = 15;
            WeaponAttributes.HitColdArea = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public RoninBlade(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you can command more warriors now!");

                // Check if autosummon is enabled before starting the summon timer
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonRoninTimer(pm);
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
                pm.SendMessage(37, "You feel like you cannot command as many warriors as before.");
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
            list.Add("Summons Young Ronin");
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
                // Check if autosummon is enabled before starting the summon timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonRoninTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRoninTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRoninTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summons every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is RoninBlade))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    YoungRonin ronin = new YoungRonin
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ronin.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Young Ronin emerges to fight for you!");
                }
            }
        }
    }
}
