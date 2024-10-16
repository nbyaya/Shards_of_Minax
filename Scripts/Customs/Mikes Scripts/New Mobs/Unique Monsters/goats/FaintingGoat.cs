using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fainting goat corpse")]
    public class FaintingGoat : BaseCreature
    {
        private DateTime m_NextGaze;
        private DateTime m_NextStoneForm;
        private DateTime m_NextFainting;
        private DateTime m_NextCallAllies;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FaintingGoat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fainting goat";
            Body = 0xD1; // Goat body
            Hue = 1911; // Yellow hue
			BaseSoundID = 0x99;

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

        public FaintingGoat(Serial serial)
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
                    m_NextGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStoneForm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFainting = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCallAllies = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGaze)
                {
                    PerformGazeOfTerror();
                }

                if (DateTime.UtcNow >= m_NextStoneForm)
                {
                    ActivateStoneForm();
                }

                if (DateTime.UtcNow >= m_NextFainting)
                {
                    PerformFaintingFrenzy();
                }

                if (DateTime.UtcNow >= m_NextCallAllies)
                {
                    CallAllies();
                }
            }
        }

        private void PerformGazeOfTerror()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fainting Goat's gaze strikes fear into its enemies! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (m is PlayerMobile)
                    {
                        // Calculate a flee location within range
                        Point3D fleeLocation = new Point3D(
                            m.X + Utility.RandomMinMax(-10, 10),
                            m.Y + Utility.RandomMinMax(-10, 10),
                            m.Z
                        );

                        // Move the mobile to the new location
                        Timer.DelayCall(TimeSpan.FromSeconds(1), delegate
                        {
                            m.MoveToWorld(fleeLocation, m.Map);
                        });
                    }
                }
            }

            m_NextGaze = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown after using the ability
        }

        private void ActivateStoneForm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fainting Goat transforms into stone, becoming nearly invulnerable! *");

            SetResistance(ResistanceType.Physical, 70, 80);
            SetDamage(20, 25); // Increase the damage range by 5

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                SetResistance(ResistanceType.Physical, 30, 40);
                SetDamage(15, 20); // Reset the damage range to original
            });

            m_NextStoneForm = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown after using the ability
        }

        private void PerformFaintingFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fainting Goat faints and releases a burst of energy! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.PlaySound(0x1DD);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            Hits += 20; // Heal
            m_NextFainting = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown after using the ability
        }

        private void CallAllies()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fainting Goat calls upon its kin to assist in battle! *");

            for (int i = 0; i < 2; i++)
            {
                FaintingGoatKid kid = new FaintingGoatKid();
                kid.MoveToWorld(Location, Map);
            }

            m_NextCallAllies = DateTime.UtcNow + TimeSpan.FromMinutes(3); // Cooldown after using the ability
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

    public class FaintingGoatKid : BaseCreature
    {
        [Constructable]
        public FaintingGoatKid()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fainting goat kid";
            Body = 0xD1; // Goat body
            Hue = 0xF6C; // Yellow hue

            SetStr(50);
            SetDex(40);
            SetInt(10);

            SetHits(30);
            SetMana(0);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 100;
            Karma = 0;

            VirtualArmor = 10;

            Tamable = false;
        }

        public FaintingGoatKid(Serial serial)
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
