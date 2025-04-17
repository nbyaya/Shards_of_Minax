using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HeadlessHorrorHelm : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public HeadlessHorrorHelm()
        {
            Weight = 5.0;
            Name = "Headless Horror Helm";
            Hue = 1175; // Dark grayish-green to reflect a "headless horror" theme

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 3;
            Attributes.BonusStr = 20;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.ReflectPhysical = 15;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;

            SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 10;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public HeadlessHorrorHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power to command more creatures!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonHeadlessHorrorTimer(pm);
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
                pm.SendMessage(37, "You feel your control over minions weaken.");
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
            list.Add("Summons Headless Ones");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonHeadlessHorrorTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonHeadlessHorrorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonHeadlessHorrorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid, the item is not equipped, or autosummon is off
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is HeadlessHorrorHelm) || !AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    HeadlessOne headless = new HeadlessOne
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    headless.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Headless One emerges from the shadows to serve you!");
                }
            }
        }
    }
}
