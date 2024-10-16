using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a crystal golem corpse")]
    public class CrystalGolem : BaseCreature
    {
        private DateTime m_NextCrystalShard;
        private DateTime m_NextReflectiveAura;
        private DateTime m_NextPrismaticBurst;

        private bool m_AbilitiesInitialized; // Flag to check if abilities have been initialized

        [Constructable]
        public CrystalGolem()
            : base(AIType.AI_Melee, FightMode.Closest,10, 1, 0.4, 0.8)
        {
            Name = "a Crystal Golem";
            Body = 752; // Using the golem body as a base
            Hue = 1948; // Shimmering crystal hue
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

        public CrystalGolem(Serial serial)
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
                    m_NextCrystalShard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextReflectiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPrismaticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCrystalShard)
                {
                    CrystalShard();
                }

                if (DateTime.UtcNow >= m_NextReflectiveAura)
                {
                    ReflectiveAura();
                }

                if (DateTime.UtcNow >= m_NextPrismaticBurst)
                {
                    PrismaticBurst();
                }
            }
        }

        private void CrystalShard()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shoots a shard of crystal! *");

            Effects.SendLocationEffect(this.Location, this.Map, 0x373A, 10, 16, 1154);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player && m.InRange(this, 10))
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    m.SendMessage("A sharp crystal shard pierces you!");
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    if (Utility.RandomDouble() < 0.3) // 30% chance to inflict bleeding
                    {
                        m.SendMessage("You are bleeding from the shard wound!");
                        m.ApplyPoison(this, Poison.Lethal);
                    }
                }
            }

            m_NextCrystalShard = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Fixed cooldown
        }

        private void ReflectiveAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Activates Reflective Aura! *");

            Effects.SendLocationEffect(this.Location, this.Map, 0x374A, 10, 16, 1154);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Crystal Golem's Reflective Aura deflects some of your attack!");
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() => 
            {
                if (Combatant != null)
                {
                    // Additional logic for ReflectiveAura, if needed
                }
            }));

            m_NextReflectiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
        }

        private void PrismaticBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a Prismatic Burst! *");

            Effects.SendLocationEffect(this.Location, this.Map, 0x373A, 10, 16, 1154);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are blinded and disoriented by the prismatic burst!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Blinding effect
                    m.AddToBackpack(new CrystalShards()); // Adds a small item to the player's backpack as a bonus effect
                }
            }

            m_NextPrismaticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Fixed cooldown
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
