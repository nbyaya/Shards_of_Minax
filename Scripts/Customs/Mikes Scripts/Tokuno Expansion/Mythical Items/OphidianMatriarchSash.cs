using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OphidianMatriarchSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OphidianMatriarchSash()
        {
            Weight = 1.0;
            Name = "Sash of the Ophidian Matriarch";
            Hue = 1367; // Ophidian-like hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusDex = 10;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.Luck = 150;

            Resistances.Poison = 15;
            Resistances.Physical = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OphidianMatriarchSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the wisdom of the Ophidian Matriarch guide you, allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonOphidianMatriarchTimer(pm);
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
                pm.SendMessage(37, "The presence of the Ophidian Matriarch fades, and your command over creatures diminishes.");
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
            list.Add("Summons Ophidian Matriarchs");
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
                m_Timer = new SummonOphidianMatriarchTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonOphidianMatriarchTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOphidianMatriarchTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is OphidianMatriarchSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    OphidianMatriarch matriarch = new OphidianMatriarch
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    matriarch.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ophidian Matriarch emerges to serve you!");
                }
            }
        }
    }
}
