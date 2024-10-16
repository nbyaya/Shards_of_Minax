using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a spider monkey corpse")]
    public class SpiderMonkey : BaseCreature
    {
        private DateTime m_NextAerialAssault;
        private DateTime m_NextQuickReflexes;
        private DateTime m_NextSummonSpiders;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SpiderMonkey()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Spider Monkey";
            Body = 0x1D; // Gorilla body
            Hue = 1956; // Unique hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SpiderMonkey(Serial serial)
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
                    m_NextAerialAssault = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextQuickReflexes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonSpiders = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAerialAssault)
                {
                    AerialAssault();
                }

                if (DateTime.UtcNow >= m_NextQuickReflexes)
                {
                    QuickReflexes();
                }

                if (DateTime.UtcNow >= m_NextSummonSpiders)
                {
                    SummonSpiders();
                }
            }
        }

        private void AerialAssault()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Spider Monkey leaps down with deadly precision! *");
            PlaySound(0x1E0); // Swooping sound effect
            FixedEffect(0x376A, 10, 16); // Visual effect

            // Perform a dive attack with bonus damage
            if (Combatant != null && !Combatant.Deleted)
            {
                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
            }

            m_NextAerialAssault = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
        }

        private void QuickReflexes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Spider Monkey moves with blinding speed! *");
            // Temporarily increase dodge chance
            VirtualArmor += 15;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => VirtualArmor -= 15);

            m_NextQuickReflexes = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset cooldown
        }

        private void SummonSpiders()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Spider Monkey summons a horde of spiders! *");
            PlaySound(0x1E4); // Spider summon sound effect
            FixedEffect(0x373A, 10, 16); // Spider web effect

            // Summon spiders
            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
                GiantSpider spider = new GiantSpider(); // Use the base Spider class or create a custom spider class if needed
                spider.MoveToWorld(loc, Map);
                spider.Combatant = this;
            }

            m_NextSummonSpiders = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            // Apply dodge chance if active
            if (Utility.RandomDouble() < 0.15)
            {
                attacker.SendMessage("The Spider Monkey dodges your attack!");
                attacker.SendMessage("The attack missed!");
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
