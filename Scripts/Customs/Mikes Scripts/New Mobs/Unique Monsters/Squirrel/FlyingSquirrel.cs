using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a flying squirrel corpse")]
    public class FlyingSquirrel : BaseCreature
    {
        private DateTime m_NextGlideAttack;
        private DateTime m_NextSonicSqueak;
        private DateTime m_NextWingGust;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FlyingSquirrel()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a flying squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2436; // Gray hue with unique color

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

        public FlyingSquirrel(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextGlideAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                    m_NextSonicSqueak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25));
                    m_NextWingGust = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGlideAttack)
                {
                    GlideAttack();
                }

                if (DateTime.UtcNow >= m_NextSonicSqueak)
                {
                    SonicSqueak();
                }

                if (DateTime.UtcNow >= m_NextWingGust)
                {
                    WingGust();
                }
            }
        }

        private void GlideAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flying Squirrel glides through the air with a fierce dive attack! *");
            PlaySound(0x208); // Gliding sound

            // Create a streak of motion effect
            Effects.SendLocationEffect(Location, Map, 0x37B9, 20, 10); // Streak effect

            if (Combatant != null)
            {
                AOS.Damage(Combatant, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0); // High damage
                ((Mobile)Combatant).SendMessage("You are struck by the Flying Squirrel's powerful dive attack!");
                // Knockback effect
                ((Mobile)Combatant).MoveToWorld(new Point3D(((Mobile)Combatant).X + Utility.Random(2, 4), ((Mobile)Combatant).Y + Utility.Random(2, 4), ((Mobile)Combatant).Z), Map);
            }

            m_NextGlideAttack = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for GlideAttack
        }

        private void SonicSqueak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flying Squirrel emits a deafening squeak! *");
            PlaySound(0x1C6); // Loud squeak sound

            // Create a loud sound wave effect
            Effects.PlaySound(Location, Map, 0x207); // Sound wave effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.SendMessage("You are disoriented by the Flying Squirrel's sonic squeak!");
                    // Temporary confusion effect (e.g., reduce skills or prevent actions)
                    m.SendMessage("You feel a wave of confusion washing over you!");

                    // Reduce skills temporarily
                    m.Skills[SkillName.Tactics].Base -= 10;
                    m.Skills[SkillName.Wrestling].Base -= 10;
                }
            }

            m_NextSonicSqueak = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for SonicSqueak
        }

        private void WingGust()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flying Squirrel creates a gust of wind that pushes you back! *");
            PlaySound(0x1D4); // Wind gust sound

            // Create a gust of wind effect
            Effects.SendLocationEffect(Location, Map, 0x375A, 20, 10); // Gust effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    // Push back effect
                    m.MoveToWorld(new Point3D(m.X + Utility.Random(2, 4), m.Y + Utility.Random(2, 4), m.Z), Map);
                    m.SendMessage("You are pushed back by the gust of wind!");
                }
            }

            m_NextWingGust = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for WingGust
        }

        private void Dodge()
        {
            // Make the squirrel dodge an attack
            if (Utility.RandomDouble() < 0.25) // 25% chance to dodge
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flying Squirrel nimbly dodges your attack! *");
                PlaySound(0x1C3); // Dodge sound
                // Reduce damage taken
                this.VirtualArmor += 10;
            }
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);

            // Trigger dodge chance on taking damage
            Dodge();
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
