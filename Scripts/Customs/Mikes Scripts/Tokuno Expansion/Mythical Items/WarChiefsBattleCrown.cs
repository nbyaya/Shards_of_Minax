using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WarChiefsBattleCrown : OrcHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WarChiefsBattleCrown()
        {
            Weight = 5.0;
            Name = "War Chief's Battle Crown";
            Hue = 1175; // Orcish hue, adjust as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusHits = 20;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.WeaponDamage = 10;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(1, SkillName.Swords, 10.0);

            ArmorAttributes.SelfRepair = 3;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WarChiefsBattleCrown(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the commanding presence of a war chief!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonOrcCaptainTimer(pm);
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
                pm.SendMessage(37, "You feel your command over the horde fade.");
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
            list.Add("Summons Orc Captains");
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
                    m_Timer = new SummonOrcCaptainTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOrcCaptainTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOrcCaptainTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is WarChiefsBattleCrown))
                {
                    Stop();
                    return;
                }

                // Ensure autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    OrcCaptain orc = new OrcCaptain
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    orc.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Orc Captain rallies to your command!");
                }
            }
        }
    }
}
