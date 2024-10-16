using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a wolf spider corpse")]
    public class GiantWolfSpider : BaseCreature
    {
        private DateTime m_NextSilkLasso;
        private DateTime m_NextCamouflage;
        private DateTime m_NextHunterInstinct;
        private DateTime m_NextPoisonCloud;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GiantWolfSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wolf spider";
            Body = 28; // Using GiantSpider body
            Hue = 1794; // Unique hue
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

        public GiantWolfSpider(Serial serial)
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
                    m_NextSilkLasso = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHunterInstinct = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSilkLasso)
                {
                    SilkLasso();
                }

                if (DateTime.UtcNow >= m_NextCamouflage)
                {
                    Camouflage();
                }

                if (DateTime.UtcNow >= m_NextHunterInstinct)
                {
                    HunterInstinct();
                }

                if (DateTime.UtcNow >= m_NextPoisonCloud)
                {
                    PoisonCloud();
                }
            }
        }

        private void SilkLasso()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Throws a web to ensnare you! *");
            FixedEffect(0x376A, 10, 16);

            if (Combatant != null && !Combatant.Deleted && Combatant.Alive)
            {
                Mobile mobCombatant = Combatant as Mobile;
                if (mobCombatant != null)
                {
                    mobCombatant.SendMessage("You are ensnared by the spider's web and pulled closer!");
                    mobCombatant.MoveToWorld(Location, Map);
                }
            }

            m_NextSilkLasso = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown for next use
        }

        private void Camouflage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider vanishes into thin air! *");
            Hue = 0; // Make it blend with the environment

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                Hue = 1109; // Restore original hue
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider reappears! *");
            });

            m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown for next use
        }

        private void HunterInstinct()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider's senses sharpen, preparing for the hunt! *");
            SetDamage(10, 25); // Increase damage

            // Assuming you have a property or method to change speed, you might do something like this
            // speedMultiplier is a placeholder; replace with actual property or logic if available
            double speedMultiplier = 1.5; 
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                SetDamage(8, 20); // Restore original damage
                // Restore speed to normal (you need to adjust this based on your actual implementation)
            });

            m_NextHunterInstinct = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown for next use
        }

        private void PoisonCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider releases a toxic cloud of poison! *");
            FixedEffect(0x3728, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are enveloped in a cloud of poison!");
                    m.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Head);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.ApplyPoison(this, Poison.Greater);
                }
            }

            m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set cooldown for next use
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize to reinitialize random intervals
        }
    }
}
