using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ClockworkAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ClockworkAmulet()
        {
            Name = "Clockwork Amulet of Time";
            Hue = 1153; // A mystical bluish hue
            Weight = 1.0;

            // Magical enhancements
            Attributes.BonusInt = 10;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 12;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 15;
            Attributes.NightSight = 1;
            Attributes.DefendChance = 10;
            Attributes.Luck = 100;

            // Resistance bonuses
            Resistances.Physical = 5;
            Resistances.Energy = 10;
            Resistances.Cold = 5;

            // Skill enhancements
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 15.0);

            // Attach to XmlLevelSystem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            m_BonusFollowers = 3; // Extra follower slots
        }

        public ClockworkAmulet(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The gears of time turn in your favor. More servants heed your call.");
                StartSummonTimer(pm);
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The ticking ceases. Your temporal influence fades.");
            }

            StopSummonTimer();
        }

        private void StartSummonTimer(Mobile owner)
        {
            StopSummonTimer(); // Just in case
            m_Timer = new SummonTimeDemonTimer(owner);
            m_Timer.Start();
        }

        private void StopSummonTimer()
        {
            m_Timer?.Stop();
            m_Timer = null;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Time Demons");
            list.Add("Increases maximum followers");
            list.Add("Pulses with ancient mechanical energy");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_BonusFollowers);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_BonusFollowers = reader.ReadInt();

            if (Parent is Mobile mob)
                StartSummonTimer(mob);
        }

        private class SummonTimeDemonTimer : Timer
        {
            private readonly Mobile m_Owner;

            public SummonTimeDemonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(20.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is ClockworkAmulet))
                {
                    Stop();
                    return;
                }

                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    BaseCreature summon = new TimeDemon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    summon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A rift opens as a Time Demon steps forth to obey your will.");
                }
            }
        }
    }
}
