using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Nagash's corpse")]
    public class Nagash : BaseCreature
    {
        private DateTime m_NextRaiseDead;
        private DateTime m_NextAuraOfDecay;
        private DateTime m_NextNecroticWave;
        private DateTime m_NextGraveSummon;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Nagash()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Nagash, the Undying";
            Body = 78; // AncientLich body
            Hue = 2096; // Unique rotting green hue
			BaseSoundID = 412;

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

            m_AbilitiesInitialized = false;
        }

        public Nagash(Serial serial)
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
                    // Initialize the ability timers
                    Random rand = new Random();
                    m_NextRaiseDead = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextAuraOfDecay = DateTime.UtcNow + TimeSpan.FromSeconds(5);
                    m_NextNecroticWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                    m_NextGraveSummon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRaiseDead)
                {
                    RaiseDead();
                }

                if (DateTime.UtcNow >= m_NextAuraOfDecay)
                {
                    AuraOfDecay();
                }

                if (DateTime.UtcNow >= m_NextNecroticWave)
                {
                    NecroticWave();
                }

                if (DateTime.UtcNow >= m_NextGraveSummon)
                {
                    GraveSummon();
                }
            }
        }

        private void RaiseDead()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nagash raises the dead to serve him! *");
            PlaySound(0x20D); // Raise dead sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature creature && !creature.Alive && creature.Corpse != null)
                {
                    if (Utility.RandomDouble() < 0.5) // 50% chance
                    {
                        BaseCreature undead;
                        if (Utility.RandomBool())
                        {
                            undead = new SkeletonWarrior(); // Example of different undead types
                        }
                        else
                        {
                            undead = new Lich(); // Example of different undead types
                        }
                        undead.Followers = 0;
                        undead.SetHits(creature.Hits);
                        undead.MoveToWorld(creature.Location, creature.Map);
                        undead.Combatant = Combatant;

                        creature.Delete();
                    }
                }
            }

            m_NextRaiseDead = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Raise Dead
        }

        private void AuraOfDecay()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The aura of decay surrounds Nagash! *");
            PlaySound(0x20D); // Decay sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0); // Pure energy damage
                    m.SendMessage("You are afflicted by the aura of decay!");
                    Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 10, 10); // Rotting green aura effect

                    // Apply debuff
                    m.SendMessage("You feel your strength waning!");
                    m.Damage(0); // Prevent healing while in aura
                    // Add your debuff application here
                }
            }

            m_NextAuraOfDecay = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Cooldown for Aura of Decay
        }

		private void NecroticWave()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nagash unleashes a wave of necrotic energy! *");
			PlaySound(0x20D); // Necrotic sound effect

			foreach (Mobile m in GetMobilesInRange(10))
			{
				if (m != this && m.Alive && !m.IsDeadBondedPet)
				{
					AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0); // Necrotic damage
					m.SendMessage("You are hit by a necrotic wave!");
					m.SendMessage("Your resistances have been weakened!");

					// Weaken resistances (Example implementation)
					// You might want to adjust this to apply a debuff effect more suited to your needs
					// Custom implementation or external debuff application might be required
					ApplyResistanceDebuff(m); // Custom method to handle debuff

					Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 20, 10); // Wave effect
				}
			}

			m_NextNecroticWave = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Necrotic Wave
		}

		// Example method for applying a resistance debuff
		private void ApplyResistanceDebuff(Mobile m)
		{
			// Example: Reduce resistances by 10%
			m.SendMessage("Your resistances are reduced by the necrotic wave!");

			// Use this method to apply a custom debuff or modify resistances
			// You might need to implement a custom debuff class or logic based on your requirements
		}


        private void GraveSummon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nagash summons a skeletal champion from the grave! *");
            PlaySound(0x20D); // Summon sound effect

            BaseCreature champion = new SkeletalChampion(); // Example skeletal champion
            champion.MoveToWorld(Location, Map);
            champion.Combatant = Combatant;

            m_NextGraveSummon = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Grave Summon
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
    
    // Example additional undead types
    public class SkeletonWarrior : BaseCreature
    {
        [Constructable]
        public SkeletonWarrior() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Skeleton Warrior";
            Body = 50;
            Hue = 0x4B3; // Example hue

            SetStr(100, 150);
            SetDex(80, 100);
            SetInt(50, 70);

            SetHits(100, 120);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Swords, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.MagicResist, 30.0, 50.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 20;
        }

        public SkeletonWarrior(Serial serial) : base(serial)
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

    public class SkeletalChampion : BaseCreature
    {
        [Constructable]
        public SkeletalChampion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Skeletal Champion";
            Body = 50;
            Hue = 0x4B3; // Example hue

            SetStr(200, 250);
            SetDex(100, 120);
            SetInt(100, 120);

            SetHits(300, 350);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.MagicResist, 50.0, 70.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;
        }

        public SkeletalChampion(Serial serial) : base(serial)
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
