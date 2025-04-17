using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PharaohsGoldenCrown : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PharaohsGoldenCrown()
        {
            Weight = 5.0;
            Name = "Pharaoh's Golden Crown";
            Hue = 2125; // Golden hue for a regal look

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 3;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public PharaohsGoldenCrown(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the might of the pharaoh coursing through you, empowering you to command more servants!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) 
                {
                    m_Timer = new SummonMummyTimer(pm);
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
                pm.SendMessage(37, "The might of the pharaoh fades, and your power wanes.");
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
            list.Add("Summons Mummies to serve you");
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
                    m_Timer = new SummonMummyTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonMummyTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMummyTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is PharaohsGoldenCrown))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Mummy minion = new Mummy
                    {
                        Controlled = true,
                        ControlMaster = m_Owner,
                        Name = "Pharaoh's Mummy"
                    };

                    minion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Mummy rises from the sands to serve you!");
                }
            }
        }
    }
}
