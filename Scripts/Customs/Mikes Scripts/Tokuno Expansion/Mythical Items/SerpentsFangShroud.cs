using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SerpentsFangShroud : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SerpentsFangShroud()
        {
            Weight = 2.0;
            Name = "Serpent's Fang Shroud";
            Hue = 1175; // Poison green color

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusStam = 20;
            Attributes.RegenStam = 3;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.WeaponSpeed = 15;
            Attributes.WeaponDamage = 20;
            Attributes.Luck = 50;
            Attributes.CastRecovery = 1;
            Attributes.CastSpeed = 1;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 15;
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Ninjitsu, 10.0);
            SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
            SkillBonuses.SetValues(2, SkillName.Fencing, 10.0);

            // Follower bonus
            m_BonusFollowers = 1;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SerpentsFangShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a venomous power allowing you to command more creatures!");

                // Only start the summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSerpentsFangAssassinTimer(pm);
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
                pm.SendMessage(37, "You feel the venomous power fade, limiting your command over creatures.");
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
            list.Add("Summons Serpent's Fang Assassins");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check autosummon on load as well
                {
                    m_Timer = new SummonSerpentsFangAssassinTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSerpentsFangAssassinTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSerpentsFangAssassinTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is SerpentsFangShroud))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SerpentsFangAssassin assassin = new SerpentsFangAssassin
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    assassin.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Serpent's Fang Assassin emerges to serve you!");
                }
            }
        }
    }
}
