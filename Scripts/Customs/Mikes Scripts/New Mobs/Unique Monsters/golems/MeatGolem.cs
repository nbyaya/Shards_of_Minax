using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a meat golem corpse")]
    public class MeatGolem : BaseCreature
    {
        private DateTime m_NextGraspOfTheGrave;
        private DateTime m_NextCorruptedRegeneration;
        private DateTime m_NextRottingStrike;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MeatGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a Meat Golem";
            Body = 752; // Golem body
            Hue = 1937; // Unique hue for the Flesh Golem
			BaseSoundID = 357;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public MeatGolem(Serial serial) : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextGraspOfTheGrave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCorruptedRegeneration = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextRottingStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGraspOfTheGrave)
                {
                    GraspOfTheGrave();
                }

                if (DateTime.UtcNow >= m_NextCorruptedRegeneration)
                {
                    CorruptedRegeneration();
                }

                if (DateTime.UtcNow >= m_NextRottingStrike)
                {
                    RottingStrike();
                }
            }
        }

        private void GraspOfTheGrave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground shakes as it grasps for you! *");
            FixedEffect(0x3728, 10, 16); // Ground shaking effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (InLOS(m))
                    {
                        m.Freeze(TimeSpan.FromSeconds(4));
                        m.SendMessage("You are ensnared by the grasping ground!");
                    }
                }
            }

            m_NextGraspOfTheGrave = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown
        }

        private void CorruptedRegeneration()
        {
            if (Hits < HitsMax)
            {
                Heal(20); // Heals a fixed amount
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flesh Golem's wounds close up slowly! *");
            }

            m_NextCorruptedRegeneration = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void RottingStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flesh Golem's strike is toxic! *");
            FixedEffect(0x36BD, 10, 16); // Toxic strike effect

            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                if (target != null)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
                    target.SendMessage("You are struck by a rotting, toxic blow!");
                    target.SendMessage("Your resistance to poison is reduced!");

                    target.Paralyze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextRottingStrike = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Additional drops or special effects on death
            if (Utility.RandomDouble() < 0.05)
            {
                c.DropItem(new GolemHeart());
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flesh Golem collapses and leaves behind a heart of darkness! *");
            }
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class GolemHeart : Item
    {
        [Constructable]
        public GolemHeart() : base(0x1B1B)
        {
            Name = "Heart of Darkness";
            Hue = 0x2A; // Dark hue
            Weight = 1.0;
        }

        public GolemHeart(Serial serial) : base(serial)
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
