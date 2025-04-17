using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OrcWarlordsPlateChest : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OrcWarlordsPlateChest()
        {
            Weight = 10.0;
            Name = "Orc Warlord's Plate Chest";
            Hue = 1109; // Orcish greenish metal

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusStr = 25;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 2;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 15;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OrcWarlordsPlateChest(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the commanding presence of an Orc Warlord!");

                // Only start the summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonOrcBruteTimer(pm);
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
                pm.SendMessage(37, "The Orc Warlord's power fades.");
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
            list.Add("Summons Orc Brutes");
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
                    m_Timer = new SummonOrcBruteTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOrcBruteTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOrcBruteTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is OrcWarlordsPlateChest))
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
                    OrcBrute brute = new OrcBrute
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    brute.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Orc Brute rallies to your command!");
                }
            }
        }
    }
}
