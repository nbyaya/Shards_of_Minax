using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostwraithCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostwraithCloak()
        {
            Weight = 5.0;
            Name = "Frostwraith Cloak";
            Hue = 1152; // Frosty blue hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 5;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.CastRecovery = 2;
            Attributes.CastSpeed = 1;
            Resistances.Cold = 20;
            Resistances.Physical = 10;

            // Attach XmlLevelItem for leveling
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostwraithCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(1153, "A cold presence strengthens your command over creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonIceFiendTimer(pm);
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
                pm.SendMessage(37, "The chilling power fades, and your command wanes.");
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
            list.Add("Summons Ice Fiends to aid you");
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
                m_Timer = new SummonIceFiendTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonIceFiendTimer : Timer
        {
            private Mobile m_Owner;

            public SummonIceFiendTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FrostwraithCloak))
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
                    IceFiend iceFiend = new IceFiend
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    iceFiend.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(1153, "An Ice Fiend materializes, drawn to your chilling aura!");
                }
            }
        }
    }
}
