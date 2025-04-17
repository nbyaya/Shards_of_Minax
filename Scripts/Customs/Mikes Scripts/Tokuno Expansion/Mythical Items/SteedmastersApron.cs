using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SteedmastersApron : HalfApron
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SteedmastersApron()
        {
            Weight = 2.0;
            Name = "Steedmaster's Apron";
            Hue = 1153; // Shiny silver hue to match the theme of Silver Steeds

            // Set attributes and bonuses
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.RegenStam = 3;
            Attributes.LowerManaCost = 10;
            Attributes.ReflectPhysical = 5;
            Attributes.Luck = 150;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 8;
            Resistances.Energy = 8;

            // Skill Bonus
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem for compatibility
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SteedmastersApron(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel capable of commanding more powerful steeds!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Only start if autosummon is on
                {
                    m_Timer = new SummonSilverSteedTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding powerful steeds.");
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
            list.Add("Summons Silver Steeds");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled on equip
                {
                    m_Timer = new SummonSilverSteedTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSilverSteedTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSilverSteedTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SteedmastersApron))
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
                    SilverSteed steed = new SilverSteed
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    steed.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A majestic Silver Steed appears to serve you!");
                }
            }
        }
    }

    public class SilverSteed : BaseCreature
    {
        public SilverSteed()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Silver Steed";
            Body = 0xC8; // Horse body
            Hue = 1153; // Silver hue
            BaseSoundID = 0xA8;

            SetStr(120);
            SetDex(120);
            SetInt(20);

            SetHits(150);
            SetStam(120);
            SetMana(0);

            SetDamage(8, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 3000;
            Karma = 3000;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 90.0;
        }

        public SilverSteed(Serial serial) : base(serial)
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
