using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WraithlordsAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WraithlordsAmulet()
        {
            Weight = 1.0;
            Name = "Wraithlord's Amulet";
            Hue = 1109; // Dark spectral hue

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 15;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);

            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 15;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WraithlordsAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Wraithlord bolsters your command over the cursed.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonCursedTimer(pm);
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
                pm.SendMessage(37, "You feel the Wraithlord's power leave you.");
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
            list.Add("Summons Cursed");
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
                m_Timer = new SummonCursedTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonCursedTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCursedTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is WraithlordsAmulet))
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
                    Cursed wraith = new Cursed
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wraith.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Cursed rises to obey your will!");
                }
            }
        }
    }
}
