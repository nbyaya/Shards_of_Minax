using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an emperor cobra corpse")]
    public class EmperorCobra : BaseCreature
    {
        private static readonly int[] ImperialSnakeTypes = new int[] { 0x15, 52 }; // Body IDs for different snakes

        private DateTime m_NextVenomousBite;
        private DateTime m_NextHoodExpansion;
        private DateTime m_NextKingsCommand;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EmperorCobra()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Emperor Cobra";
            Body = 0x15; // Giant Serpent body
            Hue = 1777; // Unique hue for Emperor Cobra
            BaseSoundID = 219;

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

        public EmperorCobra(Serial serial)
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
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHoodExpansion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextKingsCommand = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVenomousBite)
                {
                    VenomousBite();
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for VenomousBite
                }

                if (DateTime.UtcNow >= m_NextHoodExpansion)
                {
                    HoodExpansion();
                    m_NextHoodExpansion = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for HoodExpansion
                }

                if (DateTime.UtcNow >= m_NextKingsCommand)
                {
                    KingsCommand();
                    m_NextKingsCommand = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for KingsCommand
                }
            }
        }

        private void VenomousBite()
        {
            if (Combatant == null)
                return;

            Mobile target = Combatant as Mobile;

            if (target == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Emperor Cobra strikes with a venomous bite! *");
            target.SendMessage("You have been bitten by the Emperor Cobra's venomous bite!");

            // Apply poison effect
            target.ApplyPoison(this, Poison.Deadly);

            // Reduce dexterity
            target.Dex -= 10;

            // Visual effect for bite
            target.FixedParticles(0x36BD, 10, 30, 5052, EffectLayer.LeftFoot);
        }

        private void HoodExpansion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Emperor Cobra's hood flares up! *");
            FixedEffect(0x376A, 10, 16);

            // Increase resistance temporarily
            this.SetResistance(ResistanceType.Physical, 60, 70);
            this.SetResistance(ResistanceType.Poison, 90, 100);

            // Reduce attack speed of nearby enemies
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m.Combatant != null)
                {
                    m.SendMessage("You feel your movements slow as the Emperor Cobra expands its hood!");
                    m.SendMessage("You are intimidated by the Emperor Cobra!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.SetResistance(ResistanceType.Physical, 40, 50);
                this.SetResistance(ResistanceType.Poison, 80, 100);
            });
        }

        private void KingsCommand()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Emperor Cobra summons its loyal followers! *");
            FixedEffect(0x376A, 10, 16);

            // Summon snake minions
            for (int i = 0; i < 3; i++)
            {
                int body = ImperialSnakeTypes[Utility.Random(ImperialSnakeTypes.Length)];
                ImperialSnake minion = new ImperialSnake { Body = body, Hue = 1155 };
                minion.MoveToWorld(GetSpawnPosition(5), Map);

                minion.Combatant = Combatant;
            }

            // Apply fear effect
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m.Combatant != null)
                {
                    m.SendMessage("You are overwhelmed with fear as the Emperor Cobra commands its followers!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

    public class ImperialSnake : BaseCreature
    {
        [Constructable]
        public ImperialSnake()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a snake";
            Body = 0x15; // Default snake body
            Hue = 1155; // Same hue as Emperor Cobra
            BaseSoundID = 219;

            SetStr(50, 70);
            SetDex(30, 50);
            SetInt(20, 30);

            SetHits(50, 70);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 70);

            SetSkill(SkillName.Poisoning, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 50.0);
            SetSkill(SkillName.Wrestling, 30.0, 50.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 10;
        }

        public ImperialSnake(Serial serial)
            : base(serial)
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
