using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PlagueweaversShroud : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PlagueweaversShroud()
        {
            Weight = 1.0;
            Name = "Plagueweaver's Shroud";
            Hue = 1365; // A sickly green hue

            // Set attributes and bonuses
            Attributes.RegenHits = 5;
            Attributes.BonusMana = 25;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;

            Resistances.Physical = 5;
            Resistances.Poison = 15;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public PlagueweaversShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a wave of pestilence empower your ability to command creatures!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonPestilentBandageTimer(pm);
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
                pm.SendMessage(37, "The pestilent power leaves you, reducing your ability to command creatures.");
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
            list.Add("Summons Pestilent Bandages");
            list.Add("Increases maximum followers by 2");
            list.Add("Grants bonuses to Necromancy and Spirit Speak");
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
                    m_Timer = new SummonPestilentBandageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonPestilentBandageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPestilentBandageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is PlagueweaversShroud))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner) || m_Owner.Followers >= m_Owner.FollowersMax)
                    return;

                PestilentBandage bandage = new PestilentBandage
                {
                    Controlled = true,
                    ControlMaster = m_Owner
                };

                bandage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                m_Owner.SendMessage(38, "A Pestilent Bandage appears to serve you!");
            }
        }
    }
}
