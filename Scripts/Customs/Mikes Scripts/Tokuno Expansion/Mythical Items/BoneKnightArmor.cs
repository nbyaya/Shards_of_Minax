using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BoneKnightArmor : BoneChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public BoneKnightArmor()
        {
            Weight = 5.0;
            Name = "Bone Knight Armor";
            Hue = 1150; // Bone-themed color

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusStr = 20;
            Attributes.BonusStam = 20;
            Attributes.RegenHits = 3;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;

            SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 15;
            PoisonBonus = 10;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public BoneKnightArmor(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Bone Knight flows through you, increasing your command over minions!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonBoneKnightTimer(pm);
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
                pm.SendMessage(37, "The Bone Knight's power fades, and your command over minions diminishes.");
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
            list.Add("Summons Bone Knights");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonBoneKnightTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonBoneKnightTimer : Timer
        {
            private Mobile m_Owner;

            public SummonBoneKnightTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is BoneKnightArmor))
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
                    BoneKnight boneKnight = new BoneKnight
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    boneKnight.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Bone Knight emerges to serve you!");
                }
            }
        }
    }
}
