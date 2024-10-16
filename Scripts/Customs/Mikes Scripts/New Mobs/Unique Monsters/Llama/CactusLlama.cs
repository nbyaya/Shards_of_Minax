using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cactus llama corpse")]
    public class CactusLlama : BaseCreature
    {
        private DateTime m_NextNeedleBarrage;
        private DateTime m_NextDesertCamouflage;
        private DateTime m_NextThornArmor;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CactusLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a cactus llama";
            Body = 0xDC; // Llama body
            Hue = 2157; // Unique hue for Cactus Llama
			this.BaseSoundID = 0x3F3;

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

        public CactusLlama(Serial serial)
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
                    m_NextNeedleBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextDesertCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextThornArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNeedleBarrage)
                {
                    NeedleBarrage();
                }

                if (DateTime.UtcNow >= m_NextDesertCamouflage)
                {
                    DesertCamouflage();
                }

                if (DateTime.UtcNow >= m_NextThornArmor)
                {
                    ThornArmor();
                }
            }
            else
            {
                // Handle Desert Camouflage when not in combat
                if (DateTime.UtcNow >= m_NextDesertCamouflage)
                {
                    DesertCamouflage();
                }
            }
        }

        private void NeedleBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cactus Llama releases a storm of needles! *");
            PlaySound(0x227); // Needle shoot sound

            // Visual effect
            Effects.SendLocationEffect(Location, Map, 0x373A, 20, 10); // Needle particles

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 0, 100, 0, 0);
                    m.SendMessage("You are struck by a flurry of needles from the Cactus Llama!");

                    // Apply a damage-over-time effect
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyNeedleDamage(m));
                }
            }

            m_NextNeedleBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for NeedleBarrage
        }

        private void ApplyNeedleDamage(Mobile target)
        {
            if (target.Alive && CanBeHarmful(target))
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0);
                target.SendMessage("The needles continue to pierce your skin!");
            }
        }

        private void DesertCamouflage()
        {
            if (Utility.RandomDouble() < 0.5) // 50% chance to activate camouflage
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cactus Llama blends perfectly with its surroundings! *");
                this.Hidden = true; // Hide the llama

                // Blinding flash effect
                Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10);
                PlaySound(0x2D8); // Blinding flash sound

                Timer.DelayCall(TimeSpan.FromSeconds(5), () => { this.Hidden = false; }); // Reveal after 5 seconds
            }

            m_NextDesertCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for DesertCamouflage
        }

        private void ThornArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cactus Llama's thorns damage those who strike it! *");

            // Reflective damage
            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(5, 15);
                    AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                    m.SendMessage("You are hurt by the Cactus Llama's thorny armor!");
                }
            }

            m_NextThornArmor = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ThornArmor
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (willKill && from != null)
            {
                // Additional thorn damage when killed
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cactus Llama's final thorny attack! *");
                AOS.Damage(from, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 100, 0); // Final thorn damage
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
