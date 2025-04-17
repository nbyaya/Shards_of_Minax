using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PigWhisperersTalisman : GoldBracelet
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PigWhisperersTalisman()
        {
            Weight = 1.0;
            Name = "Pig Whisperer's Talisman";
            Hue = 1150; // Golden hue for the bracelet

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusHits = 25;
            Attributes.RegenHits = 2;
            Attributes.DefendChance = 15;
            Attributes.Luck = 150;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0);

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 15;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public PigWhisperersTalisman(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a magical connection to pigs!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonPigTimer(pm);
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
                pm.SendMessage(37, "Your connection to pigs feels weaker.");
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
            list.Add("Summons Pigs");
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
                m_Timer = new SummonPigTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonPigTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPigTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Bracelet) is PigWhisperersTalisman))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Pig pig = new Pig
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    pig.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A pig appears to follow you!");
                }
            }
        }
    }
}
