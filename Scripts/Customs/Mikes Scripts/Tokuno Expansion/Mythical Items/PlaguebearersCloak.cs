using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Engines.Quests.Samurai;

namespace Server.Items
{
    public class PlaguebearersCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PlaguebearersCloak()
        {
            Weight = 3.0;
            Name = "Plaguebearer's Cloak";
            Hue = 0x455; // Sickly green color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.Luck = 50;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;

            Resistances.Poison = 20;
            Resistances.Physical = 5;
            Resistances.Cold = 5;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 15.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public PlaguebearersCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The plague's influence empowers you to command more followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonDiseasedCatTimer(pm);
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
                pm.SendMessage(37, "The plague's influence fades, and you feel less control over your minions.");
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
            list.Add("Summons Diseased Cats");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonDiseasedCatTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonDiseasedCatTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDiseasedCatTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is PlaguebearersCloak))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    DiseasedCat cat = new DiseasedCat
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    cat.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Diseased Cat slinks out of the shadows to serve you!");
                }
            }
        }
    }
}
