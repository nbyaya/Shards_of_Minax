using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class JuggernautsMightyGirdle : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public JuggernautsMightyGirdle()
        {
            Weight = 1.0;
            Name = "Juggernaut's Mighty Girdle";
            Hue = 1359; // Metallic dark color to match a Juggernaut theme

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 30;
            Attributes.RegenHits = 5;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 15;

            SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);

            Resistances.Physical = 15;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public JuggernautsMightyGirdle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power to control mighty constructs!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonJuggernautTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
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
                pm.SendMessage(37, "The power to control mighty constructs fades away.");
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
            list.Add("Summons Juggernauts");
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
                m_Timer = new SummonJuggernautTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                    m_Timer.Start();
            }
        }

        private class SummonJuggernautTimer : Timer
        {
            private Mobile m_Owner;

            public SummonJuggernautTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is JuggernautsMightyGirdle))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Juggernaut juggernaut = new Juggernaut
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    juggernaut.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A mighty Juggernaut is summoned to serve you!");
                }
            }
        }
    }
}
