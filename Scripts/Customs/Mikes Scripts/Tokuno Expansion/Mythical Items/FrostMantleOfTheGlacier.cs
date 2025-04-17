using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostMantleOfTheGlacier : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostMantleOfTheGlacier()
        {
            Weight = 2.0;
            Name = "FrostMantle of the Glacier";
            Hue = 1152; // Icy blue color

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.LowerManaCost = 10;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 10;
            Attributes.Luck = 100;

            Resistances.Cold = 20;
            Resistances.Physical = 10;
            Resistances.Fire = 5; // Slight vulnerability to fire fits the frost theme

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostMantleOfTheGlacier(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The chill of the glacier empowers you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonFrostMiteTimer(pm);
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
                pm.SendMessage(37, "The frost fades, and you feel less capable of commanding creatures.");
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
            list.Add("Summons FrostMites");
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
                    m_Timer = new SummonFrostMiteTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFrostMiteTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFrostMiteTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FrostMantleOfTheGlacier))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Summon if there's room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FrostMite mite = new FrostMite
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mite.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A FrostMite scuttles forth to serve you!");
                }
            }
        }
    }
}
