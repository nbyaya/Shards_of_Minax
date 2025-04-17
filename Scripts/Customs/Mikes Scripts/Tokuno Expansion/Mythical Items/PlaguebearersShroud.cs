using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PlaguebearersShroud : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PlaguebearersShroud()
        {
            Weight = 3.0;
            Name = "Plaguebearer's Shroud";
            Hue = 1175; // A sickly green hue representing pestilence

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusStr = 10;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 2;
            Attributes.RegenMana = 2;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 10;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 5;

            SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);

            Resistances.Poison = 20;
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public PlaguebearersShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command the forces of pestilence!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonPlagueBeastTimer(pm);
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
                pm.SendMessage(37, "The forces of pestilence recede from your control.");
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
            list.Add("Summons Plague Beasts");
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
                    m_Timer = new SummonPlagueBeastTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonPlagueBeastTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPlagueBeastTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is PlaguebearersShroud))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    PlagueBeast beast = new PlagueBeast
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    beast.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Plague Beast emerges from the mists of pestilence to serve you!");
                }
            }
        }
    }
}
