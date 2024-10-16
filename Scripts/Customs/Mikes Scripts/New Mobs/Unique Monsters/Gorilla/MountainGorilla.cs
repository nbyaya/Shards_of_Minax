using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a mountain gorilla corpse")]
    public class MountainGorilla : BaseCreature
    {
        private DateTime m_NextGroundPound;
        private DateTime m_NextRockThrow;
        private DateTime m_NextRageMode;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MountainGorilla()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Mountain Gorilla";
            Body = 0x1D; // Gorilla body
            Hue = 1960; // Unique hue
			this.BaseSoundID = 0x9E;

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

            // Initialize ability timers to default values
            m_AbilitiesInitialized = false;
        }

        public MountainGorilla(Serial serial)
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
                    m_NextGroundPound = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRockThrow = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextRageMode = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGroundPound && Utility.RandomDouble() < 0.25) // 25% chance
                {
                    GroundPound();
                    m_NextGroundPound = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
                }

                if (DateTime.UtcNow >= m_NextRockThrow)
                {
                    RockThrow();
                    m_NextRockThrow = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown
                }

                if (DateTime.UtcNow >= m_NextRageMode)
                {
                    RageMode();
                    m_NextRageMode = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown
                }
            }
        }

        private void GroundPound()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Gorilla slams the ground, causing a shockwave! *");
            Effects.PlaySound(Location, Map, 0x307); // Shockwave sound effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    m.PlaySound(0x1DD); // Knockback sound effect
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }
        }

        private void RockThrow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Gorilla hurls a massive rock! *");
            Effects.PlaySound(Location, Map, 0x2D6); // Rock throw sound effect

            Mobile target = Combatant as Mobile;
            if (target != null && InRange(target, 15))
            {
                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                target.PlaySound(0x1DD); // Impact sound effect
                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            }
        }

        private void RageMode()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Gorilla enters a rage, increasing its power! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 30); // Rage effect
            PlaySound(0x5C5); // Rage sound effect

            // Increase damage and resistances temporarily
            SetDamage(20, 35);
            SetResistance(ResistanceType.Physical, 90, 110);
            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                SetDamage(15, 25);
                SetResistance(ResistanceType.Physical, 70, 90);
            });
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.3 > Utility.RandomDouble()) // 30% chance to trigger Thick Hide
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Gorilla's thick hide absorbs the blow! *");
                defender.SendMessage("The Mountain Gorilla's thick hide reduces the damage you deal!");

                // Reduce damage from physical attacks
                defender.Damage(Utility.RandomMinMax(5, 10), this);
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
