using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a woodland charger corpse")]
    public class WoodlandCharger : BaseMount
    {
        private DateTime m_NextVineEntangle;
        private DateTime m_NextRegenerativeGrowth;
        private DateTime m_NextNaturesWrath;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public WoodlandCharger()
            : base("Woodland Charger", 0xE2, 0x3EA0, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xE2;
            ItemID = 0x3EA0;
            Hue = 2083; // Forest green hue
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

        public WoodlandCharger(Serial serial)
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
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextVineEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRegenerativeGrowth = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextNaturesWrath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVineEntangle)
                {
                    VineEntangle();
                }

                if (DateTime.UtcNow >= m_NextRegenerativeGrowth)
                {
                    RegenerativeGrowth();
                }

                if (DateTime.UtcNow >= m_NextNaturesWrath)
                {
                    NaturesWrath();
                }
            }
        }

        private void VineEntangle()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vines ensnare you! *");
            PlaySound(0x1E1);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1072131); // The vines entangle you!
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x1E1);

                    // Apply stat modification
                    ApplyStatMod(m, StatType.Dex, "VineEntangle", -20, TimeSpan.FromSeconds(10));
                }
            }

            m_NextVineEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ApplyStatMod(Mobile m, StatType type, string name, int amount, TimeSpan duration)
        {
            // Check if a stat mod with the same name already exists
            StatMod mod = m.GetStatMod(name);

            if (mod != null)
            {
                m.RemoveStatMod(name); // Remove existing mod
            }

            // Create and apply the new stat mod
            mod = new StatMod(type, name, amount, duration);
            m.AddStatMod(mod);

            // Schedule removal of the stat mod after the duration
            Timer.DelayCall(duration, () => 
            {
                if (m != null && m.Alive)
                {
                    m.RemoveStatMod(name);
                }
            });
        }

        private void RegenerativeGrowth()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Woodland Charger's wounds heal! *");
            PlaySound(0x1F2);
            FixedEffect(0x376A, 10, 16);

            int heal = Utility.RandomMinMax(30, 50);
            Heal(heal);

            m_NextRegenerativeGrowth = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void NaturesWrath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nature's Wrath bursts forth! *");
            PlaySound(0x22F);
            FixedEffect(0x37C4, 10, 36);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                    m.PlaySound(0x1E1);
                }
            }

            m_NextNaturesWrath = DateTime.UtcNow + TimeSpan.FromSeconds(25);
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
