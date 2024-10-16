using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a wood golem corpse")]
    public class SummonedWoodGolem : BaseCreature
    {
        private DateTime m_NextWoodenBarrage;
        private DateTime m_NextRegrowth;
        private DateTime m_NextRootEntangle;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedWoodGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Wood Golem";
            Body = 752; // Golem body
            Hue = 1922; // Nature-inspired green hue
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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedWoodGolem(Serial serial)
            : base(serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextWoodenBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRegrowth = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRootEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWoodenBarrage)
                {
                    WoodenBarrage();
                }

                if (DateTime.UtcNow >= m_NextRegrowth)
                {
                    Regrowth();
                }

                if (DateTime.UtcNow >= m_NextRootEntangle)
                {
                    RootEntangle();
                }
            }
        }

        private void WoodenBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Wood Golem launches a Wooden Barrage! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    Effects.SendTargetParticles(m, 0x36D4, 10, 20, 0xFFFF, EffectLayer.Waist);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    m.SendMessage("You are hit by a barrage of sharp wooden shards!");
                }
            }

            m_NextWoodenBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void Regrowth()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Wood Golem draws strength from the earth and heals itself! *");

            Heal(30);

            m_NextRegrowth = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void RootEntangle()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Wood Golem causes roots to burst from the ground! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are entangled by the roots!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);
                }
            }

            m_NextRootEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Wood Golem's attacks are fierce and painful! *");
                defender.PlaySound(0x1FB);
                defender.FixedParticles(0x36D4, 10, 30, 0x3B2, EffectLayer.Head);

                int damage = Utility.RandomMinMax(5, 15);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
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
}
