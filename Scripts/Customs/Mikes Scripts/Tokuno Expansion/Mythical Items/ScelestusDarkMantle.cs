using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ScelestusDarkMantle : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ScelestusDarkMantle()
        {
            Weight = 1.0;
            Name = "Scelestus' Dark Mantle";
            Hue = 1175; // Shadowy black-purple

            // Set attributes and bonuses
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 8;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 15.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ScelestusDarkMantle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "A dark power empowers you to command more servants.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonMinionOfScelestusTimer(pm);
                m_Timer.Start();
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The dark power fades, reducing your control over minions.");
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
            list.Add("Summons Minions of Scelestus");
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
                m_Timer = new SummonMinionOfScelestusTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonMinionOfScelestusTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMinionOfScelestusTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is ScelestusDarkMantle))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    MinionOfScelestus minion = new MinionOfScelestus
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    minion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Minion of Scelestus emerges from the shadows to serve you!");
                }
            }
        }
    }
}
