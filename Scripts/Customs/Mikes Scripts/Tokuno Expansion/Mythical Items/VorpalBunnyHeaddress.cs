using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class VorpalBunnyHeaddress : JesterHat
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public VorpalBunnyHeaddress()
        {
            Weight = 1.0;
            Name = "Vorpal Bunny Headdress";
            Hue = 1150; // Light blue, change as desired

            // Set attributes and bonuses
            Attributes.BonusDex = 5;
            Attributes.BonusMana = 10;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 150;
            Attributes.SpellDamage = 8;

            // Resistances
            Resistances.Physical = 5;
            Resistances.Fire = 3;
            Resistances.Cold = 3;
            Resistances.Poison = 3;
            Resistances.Energy = 3;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public VorpalBunnyHeaddress(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel attuned to summon whimsical creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonVorpalBunnyTimer(pm);
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
                pm.SendMessage(37, "The connection to your whimsical creatures fades.");
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
            list.Add("Summons Vorpal Bunnies");
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

            // Reinitialize timer if equipped on restart and autosummon is enabled
            if (Parent is Mobile mob)
            {
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonVorpalBunnyTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonVorpalBunnyTimer : Timer
        {
            private Mobile m_Owner;

            public SummonVorpalBunnyTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is VorpalBunnyHeaddress))
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
                    VorpalBunny bunny = new VorpalBunny
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    bunny.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Vorpal Bunny hops into existence to serve you!");
                }
            }
        }
    }
}
