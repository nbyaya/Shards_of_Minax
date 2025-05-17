using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a glacial construct corpse")]
    public class GlacialConstruct : BaseCreature
    {
        private DateTime m_NextShatterStrike;
        private DateTime m_NextIceNova;
        private DateTime m_NextFrozenHowl;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlacialConstruct()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6)
        {
            Name = "a Glacial Construct";
            Body = 752; // Same as golem
            Hue = 1150; // Frosted-blue hue
            BaseSoundID = 541;

            SetStr(1200, 1400);
            SetDex(90, 120);
            SetInt(300, 400);

            SetHits(1500, 2000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 20, 35);
            SetResistance(ResistanceType.Cold, 85, 100);
            SetResistance(ResistanceType.Poison, 50, 65);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.MagicResist, 110.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);
            SetSkill(SkillName.Anatomy, 80.0, 100.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;

            Tamable = false;

            m_AbilitiesInitialized = false;
        }

        public GlacialConstruct(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel => !Controlled;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool BardImmune => true;
        public override bool IsScaryToPets => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 8);
            PackItem(new IceCrystal(Utility.RandomMinMax(2, 5)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GlacialHeart()); // Rare drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextShatterStrike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                    m_NextFrozenHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextShatterStrike)
                    ShatterStrike();

                if (DateTime.UtcNow >= m_NextIceNova)
                    IceNova();

                if (DateTime.UtcNow >= m_NextFrozenHowl)
                    FrozenHowl();
            }
        }

        private void ShatterStrike()
        {
            if (Combatant is Mobile target)
            {
                target.SendMessage("The Glacial Construct smashes you with an earth-shattering strike!");
                AOS.Damage(target, this, Utility.RandomMinMax(35, 50), 100, 0, 0, 0, 0);
                target.Freeze(TimeSpan.FromSeconds(2.0));
                PlaySound(545);
            }

            m_NextShatterStrike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
        }

        private void IceNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The air around the Glacial Construct suddenly drops in temperature!*");
            PlaySound(562);
            Effects.PlaySound(Location, Map, 0x64F);
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);
                    if (Utility.RandomDouble() < 0.5)
                        m.Freeze(TimeSpan.FromSeconds(1.5));
                }
            }

            m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void FrozenHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Construct howls with unnatural resonance...*");
            PlaySound(320);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m is Mobile target)
                {
                    target.SendMessage("Your limbs stiffen as the icy howl pierces your soul!");
                    target.Stam -= Utility.RandomMinMax(20, 40);
                    target.Mana -= Utility.RandomMinMax(15, 30);
                    if (Utility.RandomDouble() < 0.25)
                        target.Paralyze(TimeSpan.FromSeconds(1.5));
                }
            }

            m_NextFrozenHowl = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        public override int GetAngerSound() => 541;
        public override int GetIdleSound() => 542;
        public override int GetDeathSound() => 545;
        public override int GetAttackSound() => 562;
        public override int GetHurtSound() => 320;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    // Example loot drop
    public class IceCrystal : Item
    {
        [Constructable]
        public IceCrystal() : base(0x1F19) // Crystal look
        {
            Name = "a shimmering ice crystal";
            Hue = 1150;
            Weight = 1.0;
        }

        public IceCrystal(Serial serial) : base(serial) { }

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

    public class GlacialHeart : Item
    {
        [Constructable]
        public GlacialHeart() : base(0x1F5E) // Orb item
        {
            Name = "the Frozen Heart of a Glacial Construct";
            Hue = 1150;
            Weight = 3.0;
        }

        public GlacialHeart(Serial serial) : base(serial) { }

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
