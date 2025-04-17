using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TigersClawHarness : LeatherChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TigersClawHarness()
        {
            Weight = 5.0;
            Name = "Tiger's Claw Harness";
            Hue = 1161; // Vibrant orange with stripes hinting at a tiger

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 15;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 8;
            PoisonBonus = 10;
            EnergyBonus = 10;

            // Skill Bonus
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public TigersClawHarness(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a powerful connection to fierce tigers!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonTigersClawMasterTimer(pm);
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
                pm.SendMessage(37, "The bond to the tigers weakens.");
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
            list.Add("Summons TigersClawMaster");
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
                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonTigersClawMasterTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTigersClawMasterTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTigersClawMasterTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is TigersClawHarness))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Summon if there is room for a new follower
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    TigersClawMaster tiger = new TigersClawMaster
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    tiger.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A TigersClawMaster emerges to fight by your side!");
                }
            }
        }
    }
}
