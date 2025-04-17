using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PlaguelordShroud : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PlaguelordShroud()
        {
            Weight = 3.0;
            Name = "Plaguebearer's Shroud";
            Hue = 1425; // A sickly green hue to represent disease and decay

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;

            Resistances.Poison = 15; // Strong resistance to poison
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Energy = 5;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increased max followers for stronger summoning capability
        }

        public PlaguelordShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an unholy connection to the PlagueSpawn!");

                // Only start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonPlagueSpawnTimer(pm);
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
                pm.SendMessage(37, "The plague's hold on you diminishes.");
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
            list.Add("Summons PlagueSpawn");
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
                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonPlagueSpawnTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonPlagueSpawnTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPlagueSpawnTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Spawns every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is PlaguelordShroud))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Summon the PlagueSpawn if there's room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    PlagueSpawn spawn = new PlagueSpawn
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    spawn.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A PlagueSpawn emerges from the shadows to serve you!");
                }
            }
        }
    }
}
