using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TigersClawSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TigersClawSash()
        {
            Weight = 1.0;
            Name = "Tiger's Claw Sash";
            Hue = 1150; // Tiger-stripe orange and black

            // Set attributes and bonuses
            Attributes.BonusDex = 15; // Increased agility for a thief-like feel
            Attributes.Luck = 150; // Luck for better loot and success
            Attributes.WeaponDamage = 20; // Increased damage for melee combat
            Attributes.AttackChance = 15; // Better chance to hit
            Attributes.DefendChance = 10; // Slightly better defense
            Attributes.NightSight = 1; // Always see in the dark

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Poison = 15; // Thieves often deal with poisons

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Stealing, 20.0); // Perfect for a thief
            SkillBonuses.SetValues(1, SkillName.Stealth, 20.0); // Enhanced stealth
            SkillBonuses.SetValues(2, SkillName.Ninjitsu, 15.0); // Ninja-like abilities

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem for leveling system (optional)
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public TigersClawSash(Serial serial) : base(serial)
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

                // Start summon timer if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonTigersClawThiefTimer(pm);
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
            list.Add("Summons TigersClawThief");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances thieving and stealth abilities");
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
                    m_Timer = new SummonTigersClawThiefTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTigersClawThiefTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTigersClawThiefTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid, the item is not equipped, or auto-summon is off
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is TigersClawSash) || !AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    TigersClawThief thief = new TigersClawThief
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    thief.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A TigersClawThief emerges to serve you!");
                }
            }
        }
    }
}
