using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OrtanordsCrownOfFrost : NorseHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OrtanordsCrownOfFrost()
        {
            Name = "Ortanord's Crown of Frost";
            Hue = 1152; // Ice blue color
            Weight = 5.0;

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 10;

            SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);

            PhysicalBonus = 10;
            ColdBonus = 20;
            PoisonBonus = 10;
            EnergyBonus = 10;

            ArmorAttributes.SelfRepair = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OrtanordsCrownOfFrost(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(1152, "The icy power of Ortanord courses through you, bolstering your command over minions!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonOrtanordMinionTimer(pm);
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
                pm.SendMessage(37, "The icy power of Ortanord fades from you.");

                // Stop the summon timer
                StopSummonTimer();
            }
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
            list.Add("Summons Ortanord's Frozen Minions");
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
                    m_Timer = new SummonOrtanordMinionTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOrtanordMinionTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOrtanordMinionTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is OrtanordsCrownOfFrost))
                {
                    Stop();
                    return;
                }

                // Stop if autosummon is disabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FrozenMinion minion = new FrozenMinion
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    minion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(1152, "A frozen minion of Ortanord emerges to aid you!");
                }
            }
        }
    }

    public class FrozenMinion : BaseCreature
    {
        [Constructable]
        public FrozenMinion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Frozen Minion";
            Body = 751; // Frost-like elemental body
            Hue = 1152; // Ice blue

            SetStr(120, 150);
            SetDex(90, 120);
            SetInt(60, 80);

            SetHits(100, 120);
            SetDamage(10, 15);

            SetSkill(SkillName.Magery, 60.0, 80.0);
            SetSkill(SkillName.EvalInt, 60.0, 80.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);

            ControlSlots = 1;
            Fame = 1500;
            Karma = -1500;
        }

        public FrozenMinion(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
