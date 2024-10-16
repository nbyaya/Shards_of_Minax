using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Kel'Thuzad's corpse")]
    public class KelThuzad : BaseCreature
    {
        private DateTime m_NextFrostboltVolley;
        private DateTime m_NextChainsOfTheDamned;
        private DateTime m_NextSummonMinions;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public KelThuzad()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Kel'Thuzad";
            Body = 78; // Ancient Lich body
            Hue = 2098; // Unique frosty blue hue
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
            PackNecroReg(100, 200);

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public KelThuzad(Serial serial)
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

        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return true;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }

        public override int GetIdleSound()
        {
            return 0x19D;
        }

        public override int GetAngerSound()
        {
            return 0x175;
        }

        public override int GetDeathSound()
        {
            return 0x108;
        }

        public override int GetAttackSound()
        {
            return 0xE2;
        }

        public override int GetHurtSound()
        {
            return 0x28B;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextFrostboltVolley = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextChainsOfTheDamned = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostboltVolley)
                {
                    FrostboltVolley();
                }

                if (DateTime.UtcNow >= m_NextChainsOfTheDamned)
                {
                    ChainsOfTheDamned();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }

                // Phase change when health is low
                if (Hits < HitsMax * 0.3)
                {
                    if (Utility.RandomDouble() < 0.05) // 5% chance every think cycle
                    {
                        PhaseChange();
                    }
                }
            }
        }

        private void FrostboltVolley()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kel'Thuzad unleashes a volley of frostbolts! *");
            PlaySound(0x3D3); // Frostbolt sound effect

            // Launch frostbolts at multiple targets
            foreach (Mobile target in GetMobilesInRange(10))
            {
                if (target != this && target.Alive && !target.IsDeadBondedPet && Utility.RandomDouble() < 0.5) // 50% chance
                {
                    // Apply frostbolt damage and slow effect
                    int damage = Utility.RandomMinMax(20, 40);
                    AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
                    target.Freeze(TimeSpan.FromSeconds(3)); // Slow effect

                    target.SendMessage("You are struck by a frostbolt and slowed!");
                }
            }

            m_NextFrostboltVolley = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for FrostboltVolley
        }

		private void ChainsOfTheDamned()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kel'Thuzad summons chains to bind his enemies! *");
			PlaySound(0x3D5); // Chain sound effect

			// Create chains of the damned effect
			foreach (Mobile target in GetMobilesInRange(7))
			{
				if (target != this && target.Alive && !target.IsDeadBondedPet)
				{
					int damage = Utility.RandomMinMax(15, 30);
					AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);

					target.SendMessage("You are ensnared by chains and take damage!");

					// Adjusted effect call
					Effects.SendTargetEffect(target, 0x10D, 16); // Chains effect

					Timer.DelayCall(TimeSpan.FromSeconds(1), () => target.SendMessage("The chains fade away."));
				}
			}

			m_NextChainsOfTheDamned = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for ChainsOfTheDamned
		}


        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kel'Thuzad summons undead minions to aid him in battle! *");
            PlaySound(0x3D6); // Summon sound effect

            // Summon undead minions
            for (int i = 0; i < 2; i++)
            {
                BaseCreature minion = new UndeadMinion();
                minion.MoveToWorld(Location, Map);
                minion.Combatant = Combatant;
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for SummonMinions
        }

        private void PhaseChange()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Kel'Thuzad enters a new phase, growing more powerful! *");
            PlaySound(0x3D4); // Phase change sound effect

            // Change abilities or stats to make the fight more challenging
            SetDamage(20, 35); // Increase damage
            SetResistance(ResistanceType.Cold, 80, 90); // Increase cold resistance
            SetResistance(ResistanceType.Poison, 60, 70); // Increase poison resistance

            // New abilities or enhanced versions of existing abilities
            m_NextFrostboltVolley = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Increase frequency of FrostboltVolley
            m_NextChainsOfTheDamned = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Increase frequency of ChainsOfTheDamned
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

            m_AbilitiesInitialized = false; // Reset the flag
        }
    }
}
