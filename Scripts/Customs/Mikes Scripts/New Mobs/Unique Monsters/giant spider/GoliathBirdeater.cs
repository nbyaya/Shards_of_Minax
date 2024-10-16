using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a goliath birdeater corpse")]
    public class GoliathBirdeater : BaseCreature
    {
        private DateTime m_NextCrushingBite;
        private DateTime m_NextTerrifyingDisplay;
        private DateTime m_NextWebBarrage;
        private DateTime m_NextPoisonCloud;

        private bool m_AbilitiesInitialized; // Flag to check if abilities are initialized

        [Constructable]
        public GoliathBirdeater()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a goliath birdeater";
            Body = 28; // Same as GiantSpider
            Hue = 1791; // Unique hue for Goliath Birdeater
			BaseSoundID = 0x388;

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

        public GoliathBirdeater(Serial serial)
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
                    Random rand = new Random();
                    m_NextCrushingBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextTerrifyingDisplay = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWebBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCrushingBite)
                {
                    CrushingBite();
                }

                if (DateTime.UtcNow >= m_NextTerrifyingDisplay)
                {
                    TerrifyingDisplay();
                }

                if (DateTime.UtcNow >= m_NextWebBarrage)
                {
                    WebBarrage();
                }

                if (DateTime.UtcNow >= m_NextPoisonCloud)
                {
                    PoisonCloud();
                }

                // Change tactics based on health
                if (Hits < HitsMax * 0.3) // If health is below 30%
                {
                    // Enrage mode: Increase damage
                    SetDamage(DamageMin * 2, DamageMax * 2);
                    // For speed, adjust as needed. Example: decrease cooldowns or increase attack frequency.
                }
            }
        }

        private void CrushingBite()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Goliath Birdeater delivers a crushing bite! *");

            if (Combatant != null && !Combatant.Deleted)
            {
                int damage = Utility.RandomMinMax(30, 50);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
                Combatant.PlaySound(0x1F2);
                Combatant.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
            }

            m_NextCrushingBite = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void TerrifyingDisplay()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Goliath Birdeater performs a terrifying display! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    if (Utility.RandomDouble() < 0.6) // 60% chance to flee
                    {
                        // Simulate fleeing behavior
                        m.SendMessage("You are terrified by the Goliath Birdeater's display!");
                        // Additional logic to move away from GoliathBirdeater
                    }
                }
            }

            m_NextTerrifyingDisplay = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        private void WebBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Goliath Birdeater launches a web barrage! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("You are hit by the Goliath Birdeater's web!");
                    m.Freeze(TimeSpan.FromSeconds(5)); // Extended slow effect
                    m.Damage(15, this); // Increased damage
                }
            }

            m_NextWebBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(50);
        }

        private void PoisonCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Goliath Birdeater releases a cloud of poison! *");

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are engulfed by a cloud of poison!");
                    m.ApplyPoison(this, Poison.Greater); // Apply greater poison
                    m.Damage(10, this); // Additional poison damage
                }
            }

            m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
