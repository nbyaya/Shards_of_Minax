using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a metallic windsteed corpse")]
    public class MetallicWindsteed : BaseMount
    {
        private DateTime m_NextSteelTempest;
        private DateTime m_NextWindSlash;
        private DateTime m_NextAegisOfSteel;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MetallicWindsteed()
            : base("Metallic Windsteed", 0xE2, 0x3EA0, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xE2;
            ItemID = 0x3EA0;
            Hue = 2088; // Metallic hue
			BaseSoundID = 0xA8;

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

        public MetallicWindsteed(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSteelTempest = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWindSlash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAegisOfSteel = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSteelTempest)
                {
                    SteelTempest();
                }

                if (DateTime.UtcNow >= m_NextWindSlash)
                {
                    WindSlash();
                }

                if (DateTime.UtcNow >= m_NextAegisOfSteel)
                {
                    AegisOfSteel();
                }
            }
        }

        private void SteelTempest()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A tempest of steel shards surrounds you! *");
            FixedEffect(0x376A, 10, 15);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    m.FixedEffect(0x374A, 10, 15);
                    m.SendLocalizedMessage(1111628); // The steel shards cut and confuse you!
                }
            }

            m_NextSteelTempest = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void WindSlash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A sharp wind cuts through the air! *");
            FixedEffect(0x37CC, 10, 15);

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    m.FixedEffect(0x374A, 10, 15);

                    if (Utility.RandomDouble() < 0.3)
                    {
                        Item weapon = m.Weapon as Item;
                        if (weapon != null && weapon.IsChildOf(m.Backpack))
                        {
                            m.Backpack.DropItem(weapon);
                            m.SendLocalizedMessage(1112365); // You have been disarmed!
                        }
                    }
                }
            }

            m_NextWindSlash = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void AegisOfSteel()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Metallic Windsteed's hide shimmers with steel! *");
            FixedEffect(0x373A, 10, 15);

            ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, 20);
            AddResistanceMod(mod);

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                RemoveResistanceMod(mod);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The steel reinforcement fades. *");
            });

            m_NextAegisOfSteel = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
        }
    }
}
