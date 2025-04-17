using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Engines.Quests.Samurai;

namespace Server.Items
{
    public class WraithboundMantle : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WraithboundMantle()
        {
            Weight = 1.0;
            Name = "Wraithbound Mantle";
            Hue = 1109; // Dark, spectral color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 5;
            Attributes.BonusInt = 25;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 12;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WraithboundMantle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The mantle binds wraiths to your will.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonCursedSoulTimer(pm);
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
                pm.SendMessage(37, "The wraiths are no longer bound to your command.");
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
            list.Add("Summons Cursed Souls");
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
                // Only start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonCursedSoulTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonCursedSoulTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCursedSoulTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Check if the autosummon is enabled for the owner before proceeding
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is WraithboundMantle) || !AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CursedSoul soul = new CursedSoul
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    soul.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Cursed Soul rises to serve you!");
                }
            }
        }
    }
}
