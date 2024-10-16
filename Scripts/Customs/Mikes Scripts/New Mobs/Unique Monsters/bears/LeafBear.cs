using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a leaf bear corpse")]
    public class LeafBear : BaseCreature
    {
        private DateTime m_NextVineWhip;
        private DateTime m_NextHealingGrasp;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LeafBear()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a leaf bear";
            Body = 211; // BlackBear body
            BaseSoundID = 0xA3;
            Hue = 1191; // Forest green hue

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
            SetResistance(ResistanceType.Poison, 100);
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

        public LeafBear(Serial serial) : base(serial) { }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextVineWhip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHealingGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVineWhip)
                {
                    VineWhip();
                }

                if (DateTime.UtcNow >= m_NextHealingGrasp)
                {
                    HealingGrasp();
                }
            }
        }

        private void VineWhip()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vine Whip *");
                PlaySound(0x233); // Whip sound
                FixedEffect(0x376A, 10, 20, 1166, 0); // Green effect

                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);

                target.SendLocalizedMessage(1072221); // You have been slowed by the vine whip!
                target.Paralyze(TimeSpan.FromSeconds(3));

                m_NextVineWhip = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for VineWhip
            }
        }

        private void HealingGrasp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Healing Grasp *");
            PlaySound(0x202); // Heal sound
            FixedEffect(0x376A, 10, 20, 2724, 0); // Green healing effect

            int healAmount = Utility.RandomMinMax(20, 30);
            Hits = Math.Min(Hits + healAmount, HitsMax);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature creature && creature.Controlled && creature.ControlMaster == this.ControlMaster)
                {
                    creature.Hits = Math.Min(creature.Hits + healAmount, creature.HitsMax);
                    creature.FixedEffect(0x376A, 10, 20, 2724, 0);
                }
            }

            m_NextHealingGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for HealingGrasp
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
