using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DragonWolfAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DragonWolfAmulet()
        {
            Weight = 1.0;
            Name = "Amulet of the DragonWolf";
            Hue = 1153; // Mystical blue hue for a dragon-themed item

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusDex = 15;
            Attributes.BonusMana = 30;
            Attributes.RegenMana = 3;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 8;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 12;
            Resistances.Cold = 10;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DragonWolfAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The spirit of the DragonWolf empowers your command over creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonDragonWolfTimer(pm);
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
                pm.SendMessage(37, "You feel the presence of the DragonWolf fade.");
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
            list.Add("Summons DragonWolves");
            list.Add("Increases maximum followers by 2");
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
                m_Timer = new SummonDragonWolfTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonDragonWolfTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDragonWolfTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is DragonWolfAmulet))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    DragonWolf dragonWolf = new DragonWolf
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragonWolf.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A DragonWolf emerges from the ether to serve you!");
                }
            }
        }
    }

    // Define the DragonWolf creature
    public class DragonWolf : BaseCreature
    {
        public DragonWolf() : base(AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "DragonWolf";
            Body = 34; // Use a suitable creature body
            Hue = 1157; // Dragon-like color

            SetStr(200);
            SetDex(150);
            SetInt(100);

            SetHits(300);
            SetMana(100);

            SetDamage(15, 20);

            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 85.0);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 40;
        }

        public DragonWolf(Serial serial) : base(serial)
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