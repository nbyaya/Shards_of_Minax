using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LeafWovenCirclet : Circlet
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LeafWovenCirclet()
        {
            Weight = 1.0;
            Name = "Leaf-Woven Circlet";
            Hue = 0x483; // Greenish color to fit the nature theme

            // Set attributes and bonuses
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 15;
            Attributes.NightSight = 1;
            Attributes.SpellDamage = 10;
            Attributes.CastRecovery = 2;
            Attributes.CastSpeed = 1;
            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);
            SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public LeafWovenCirclet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a mystical bond with nature, allowing you to command more creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonInsaneDryadTimer(pm);
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
                pm.SendMessage(37, "Your bond with nature weakens, reducing your control over creatures.");
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
            list.Add("Summons Insane Dryads");
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
                // Check if autosummon is enabled before starting the timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonInsaneDryadTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonInsaneDryadTimer : Timer
        {
            private Mobile m_Owner;

            public SummonInsaneDryadTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is LeafWovenCirclet))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    InsaneDryad dryad = new InsaneDryad
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dryad.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Insane Dryad materializes to serve you!");
                }
            }
        }
    }
}
