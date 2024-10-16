using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Khufu corpse")]
    public class KhufuTheGreatBuilder : BaseCreature
    {
        private DateTime m_NextPyramidShield;
        private DateTime m_NextTombTrap;
        private DateTime m_NextCurseOfThePharaoh;
        private DateTime m_NextSandstorm;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public KhufuTheGreatBuilder()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Khufu the Great Builder";
            Body = 154; // Mummy body
            Hue = 2164; // Unique hue
			BaseSoundID = 471;

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

        public KhufuTheGreatBuilder(Serial serial)
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
                    m_NextPyramidShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextTombTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCurseOfThePharaoh = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPyramidShield)
                {
                    PyramidShield();
                }

                if (DateTime.UtcNow >= m_NextTombTrap)
                {
                    TombTrap();
                }

                if (DateTime.UtcNow >= m_NextCurseOfThePharaoh)
                {
                    CurseOfThePharaoh();
                }

                if (DateTime.UtcNow >= m_NextSandstorm)
                {
                    Sandstorm();
                }
            }
        }

        private void PyramidShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Khufu summons a grand pyramid shield! *");
            PlaySound(0x1F6); // Magical sound

            // Effect: A large pyramid appears around Khufu, gradually expanding
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Head);

            // Shield Buff: Increases damage resistance and reflects damage
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.VirtualArmor += 30; // Temporary damage reduction
                    if (Utility.RandomDouble() < 0.20) // 20% chance to reflect damage
                    {
                        m.SendMessage("You feel the pyramid shield reflecting some of your attack!");
                    }
                }
            }

            m_NextPyramidShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for PyramidShield
        }

        private void TombTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Khufu sets a series of tomb traps! *");
            PlaySound(0x1F6); // Magical sound

            // Place multiple traps at random locations within a certain range
            for (int i = 0; i < 3; i++)
            {
                Point3D trapLocation = Location;
                trapLocation.X += Utility.RandomMinMax(-5, 5);
                trapLocation.Y += Utility.RandomMinMax(-5, 5);

                if (Map.CanFit(trapLocation, 16, false, false))
                {
                    TrapItem trap = new TrapItem();
                    trap.MoveToWorld(trapLocation, Map);

                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => ExplodeTrap(trap));
                }
            }

            m_NextTombTrap = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for TombTrap
        }

        private void ExplodeTrap(TrapItem trap)
        {
            if (trap.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The tomb traps explode in a burst of energy! *");
            PlaySound(0x1F5); // Explosion sound

            Effects.SendLocationEffect(trap.Location, Map, 0x36BD, 20, 10); // Trap effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are caught in the tomb trap!");
                    m.SendMessage("You feel your strength and speed draining!");
                }
            }

            trap.Delete();
        }

		private void CurseOfThePharaoh()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Khufu casts the Curse of the Pharaoh! *");
			PlaySound(0x1F6); // Magical sound

			foreach (Mobile m in GetMobilesInRange(8))
			{
				if (m != this && m.Alive && !m.IsDeadBondedPet)
				{
					m.SendMessage("You feel a heavy curse weighing you down!");
					CurseOfPharaohDebuff(m); // Apply the curse debuff
				}
			}

			m_NextCurseOfThePharaoh = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for CurseOfThePharaoh
		}


        private void Sandstorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Khufu summons a sandstorm! *");
            PlaySound(0x1F6); // Magical sound

            // Effect: A sandstorm appears, reducing visibility and dealing damage
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Sandstorm effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are battered by the sandstorm!");
                    m.SendMessage("Your vision is obscured by the swirling sands!");
                }
            }

            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Sandstorm
        }

		private void CurseOfPharaohDebuff(Mobile m)
		{
			m.Str -= 10;
			m.Dex -= 10;
			m.Int -= 10;

			// You might want to add logic to reverse these effects after some time
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

    public class TrapItem : Item
    {
        public TrapItem() : base(0x1F4) // Example item ID for the trap
        {
            Movable = false;
        }

        public TrapItem(Serial serial) : base(serial)
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
