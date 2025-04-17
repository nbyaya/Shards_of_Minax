using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ArchmageMantle : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ArchmageMantle()
        {
            Weight = 2.0;
            Name = "Archmage's Mantle";
            Hue = 1153; // A mystical blue hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 5;
            Attributes.BonusInt = 25;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 15;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public ArchmageMantle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an overwhelming surge of magical power, granting you greater command!");

                // Only start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonEvilMageLordTimer(pm);
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
                pm.SendMessage(37, "The mystical energies fade, and your power feels diminished.");
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
            list.Add("Summons Evil Mage Lords");
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
                // Only start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonEvilMageLordTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonEvilMageLordTimer : Timer
        {
            private Mobile m_Owner;

            public SummonEvilMageLordTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is ArchmageMantle))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and there is room for followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    EvilMageLord mage = new EvilMageLord
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Evil Mage Lord emerges to serve you!");
                }
            }
        }
    }
}
