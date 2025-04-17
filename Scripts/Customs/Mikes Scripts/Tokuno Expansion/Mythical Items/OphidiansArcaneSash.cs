using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OphidiansArcaneSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OphidiansArcaneSash()
        {
            Weight = 1.0;
            Name = "Ophidian's Arcane Sash";
            Hue = 1154; // A shimmering gold to reflect Ophidian magic

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 4;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 4;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.CastRecovery = 2;
            Attributes.CastSpeed = 1;

            SkillBonuses.SetValues(0, SkillName.Meditation, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(2, SkillName.Magery, 10.0);

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 15;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OphidiansArcaneSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The Ophidian magic enhances your ability to command followers!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonOphidianMageTimer(pm);
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
                pm.SendMessage(37, "The Ophidian magic fades, reducing your command over followers.");
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
            list.Add("Summons Ophidian Mages to assist you in battle");
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
                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonOphidianMageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOphidianMageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOphidianMageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is OphidiansArcaneSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    OphidianMage mage = new OphidianMage
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ophidian Mage manifests to aid you!");
                }
            }
        }
    }
}
