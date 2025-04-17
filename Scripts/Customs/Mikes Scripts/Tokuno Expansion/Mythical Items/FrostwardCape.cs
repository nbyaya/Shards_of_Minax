using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostwardCape : FurCape
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostwardCape()
        {
            Weight = 2.0;
            Name = "Frostward Cape";
            Hue = 1152; // Icy blue color, change as desired

            // Set attributes and bonuses
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 15;
            Resistances.Poison = 8;
            Resistances.Energy = 8;

            SkillBonuses.SetValues(0, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(1, SkillName.Magery, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostwardCape(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an icy power coursing through you, enhancing your control over summoned creatures.");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonFrostOozeTimer(pm);
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
                pm.SendMessage(37, "The icy power leaves you, reducing your control over summoned creatures.");
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
            list.Add("Summons Frost Oozes");
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
                    m_Timer = new SummonFrostOozeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFrostOozeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFrostOozeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FrostwardCape))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FrostOoze ooze = new FrostOoze
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ooze.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Frost Ooze emerges from the icy depths to serve you!");
                }
            }
        }
    }
}
