using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SashOfTheMilitiaCommander : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SashOfTheMilitiaCommander()
        {
            Weight = 1.0;
            Name = "Sash of the Pirate Commander";
            Hue = 1153; // A noble blue hue

            // Attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusStr = 15;
            Attributes.BonusStam = 20;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.NightSight = 1;
            SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(1, SkillName.Swords, 10.0);

            Resistances.Physical = 8;
            Resistances.Fire = 7;
            Resistances.Cold = 7;
            Resistances.Poison = 7;
            Resistances.Energy = 7;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SashOfTheMilitiaCommander(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Increase follower capacity
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more militia!");

                // Start summon timer only if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonPirateTimer(pm);
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
                pm.SendMessage(37, "Your ability to command has diminished.");
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
            list.Add("Summons Pirates");
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
                // Start the timer again if auto-summon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonPirateTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonPirateTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPirateTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0)) // Summon interval
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is SashOfTheMilitiaCommander))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Pirate fighter = new Pirate
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    fighter.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Militia Fighter answers your call to arms!");
                }
            }
        }
    }
}
