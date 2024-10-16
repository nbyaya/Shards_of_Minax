using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an eclipse reindeer corpse")]
    public class EclipseReindeer : BaseCreature
    {
        private DateTime m_NextEclipseShroud;
        private DateTime m_NextLunarStrike;
        private DateTime m_NextDarkNova;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EclipseReindeer()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an Eclipse Reindeer";
            Body = 0xEA; // GreatHart body
            Hue = 1982; // Dark, shifting hue

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

            // Initialize ability cooldowns
            m_AbilitiesInitialized = false;
        }

        public EclipseReindeer(Serial serial)
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
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEclipseShroud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLunarStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextEclipseShroud)
                {
                    EclipseShroud();
                }

                if (DateTime.UtcNow >= m_NextLunarStrike)
                {
                    LunarStrike();
                }

                if (DateTime.UtcNow >= m_NextDarkNova)
                {
                    DarkNova();
                }
            }
        }

        private void EclipseShroud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eclipse Reindeer shrouds itself in darkness! *");
            FixedEffect(0x374A, 10, 16);

            // Reduce damage taken and reflect some damage
            int damageReflection = Utility.RandomMinMax(10, 15);
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Eclipse Reindeer reflects some of the damage back at you!");
                    AOS.Damage(m, this, damageReflection, 0, 100, 0, 0, 0);
                }
            }

            m_NextEclipseShroud = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void LunarStrike()
        {
            if (DateTime.UtcNow.Hour >= 20 || DateTime.UtcNow.Hour < 6) // Nighttime condition
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eclipse Reindeer strikes with lunar power! *");
                FixedEffect(0x3779, 10, 16);

                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive)
                    {
                        int damage = Utility.RandomMinMax(20, 30);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }
            }

            m_NextLunarStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DarkNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eclipse Reindeer releases a Dark Nova! *");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(25, 35);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("The Dark Nova burns you with dark energy!");
                }
            }

            m_NextDarkNova = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Eclipse Reindeer rams with dark force! *");
                defender.PlaySound(0x4B);
                defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(5, 10);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
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
