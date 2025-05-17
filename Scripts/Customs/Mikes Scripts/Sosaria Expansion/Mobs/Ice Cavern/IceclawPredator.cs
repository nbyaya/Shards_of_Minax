using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an iceclaw predator corpse")]
    public class IceclawPredator : BaseCreature
    {
        private DateTime m_NextShatterHowl;
        private DateTime m_NextFrostPounce;
        private DateTime m_NextFrozenVeins;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IceclawPredator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Iceclaw Predator";
            Body = 127; // Same as Predator Hellcat
            Hue = 1152; // Unique cold-blue hue
            BaseSoundID = 0xBA;

            SetStr(950, 1100);
            SetDex(150, 200);
            SetInt(150, 220);

            SetHits(750, 950);
            SetStam(250, 300);
            SetMana(350, 450);

            SetDamage(25, 33);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Energy, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 45);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.1, 110.0);
            SetSkill(SkillName.Wrestling, 90.1, 110.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);
            SetSkill(SkillName.Necromancy, 80.0, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 60;

            Tamable = false;

            m_AbilitiesInitialized = false;
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.002) // 0.2% drop chance
            {
                PackItem(new IceclawTalisman());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextShatterHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 30));
                    m_NextFrostPounce = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 35));
                    m_NextFrozenVeins = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextShatterHowl)
                    ShatterHowl();

                if (DateTime.UtcNow >= m_NextFrostPounce)
                    FrostPounce();

                if (DateTime.UtcNow >= m_NextFrozenVeins)
                    FrozenVeins();
            }
        }

        private void ShatterHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Iceclaw Predator unleashes a bone-shattering howl! *");
            PlaySound(0x64F); // Ice shatter sound
            Effects.SendLocationEffect(Location, Map, 0x376A, 16, 10);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage(0x480, "The chilling howl rattles your bones!");
                    m.Freeze(TimeSpan.FromSeconds(2 + Utility.RandomDouble() * 3));
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 100, 0, 0, 0);
                }
            }

            m_NextShatterHowl = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void FrostPounce()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Iceclaw Predator pounces with glacial fury! *");
                PlaySound(0x5F9); // Frost impact sound


                if (Combatant is Mobile target)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                    target.SendMessage(0x480, "You are mauled by icy claws!");
                }

                m_NextFrostPounce = DateTime.UtcNow + TimeSpan.FromSeconds(35);
            }
        }

        private void FrozenVeins()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* Frost seeps into the ground... *");
            PlaySound(0x65F);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && Utility.RandomDouble() < 0.5)
                {
                    m.SendMessage(0x480, "Icy veins crawl up your legs!");
                    m.Stam -= Utility.RandomMinMax(10, 20);
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextFrozenVeins = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override int Hides => 15;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Feline;

        public IceclawPredator(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class IceclawTalisman : Item
    {
        [Constructable]
        public IceclawTalisman() : base(0x2F5E) // Frosty talisman graphic
        {
            Name = "Talisman of the Iceclaw";
            Hue = 1152;
            Weight = 1.0;
            LootType = LootType.Cursed;
        }

        public IceclawTalisman(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
