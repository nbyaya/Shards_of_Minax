using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GargoyleShadesShroud : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GargoyleShadesShroud()
        {
            Weight = 1.0;
            Name = "Gargoyle Shade's Shroud";
            Hue = 1175; // Dark grey with a hint of shadow

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;
            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 8;

            // Attach XmlLevelItem for leveling mechanics
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GargoyleShadesShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "Shadows gather around you, bolstering your command over creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonGargoyleShadeTimer(pm);
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
                pm.SendMessage(37, "The shadows fade, and your control weakens.");
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
            list.Add("Summons Gargoyle Shades");
            list.Add("Increases maximum followers");
            list.Add("A cloak shrouded in eternal twilight.");
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
                m_Timer = new SummonGargoyleShadeTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGargoyleShadeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGargoyleShadeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is GargoyleShadesShroud))
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
                    GargoyleShade shade = new GargoyleShade
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    shade.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Gargoyle Shade emerges from the shadows to serve you!");
                }
            }
        }
    }
}
