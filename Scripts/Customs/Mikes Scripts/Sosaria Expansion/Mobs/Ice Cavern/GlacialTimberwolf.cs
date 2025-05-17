using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a glacial timberwolf corpse")]
    public class GlacialTimberwolf : BaseCreature
    {
        private DateTime m_NextHowl;
        private DateTime m_NextFrostBite;
        private DateTime m_NextShatterLeap;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlacialTimberwolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Glacial Timberwolf";
            Body = 225; // Same as TimberWolf
            BaseSoundID = 0xE5;
            Hue = 1152; // Icy blue hue

            SetStr(450, 550);
            SetDex(150, 200);
            SetInt(80, 120);

            SetHits(600, 750);
            SetStam(200, 300);
            SetMana(100, 200);

            SetDamage(15, 25);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 75, 90);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 125.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;
            Tamable = false;

            m_AbilitiesInitialized = false;
        }

        public GlacialTimberwolf(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextHowl = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    m_NextFrostBite = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                    m_NextShatterLeap = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextHowl)
                    ArcticHowl();

                if (DateTime.UtcNow >= m_NextFrostBite)
                    FrostBite();

                if (DateTime.UtcNow >= m_NextShatterLeap)
                    ShatterLeap();
            }
        }

        private void ArcticHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Glacial Timberwolf unleashes a freezing howl!*");
            PlaySound(0x64B); // Wind/howl sound

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && !m.Hidden)
                {
                    if (m is Mobile target)
                    {
                        target.Freeze(TimeSpan.FromSeconds(2));
                        target.SendMessage(0x480, "The chilling howl paralyzes you!");
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                    }
                }
            }

            m_NextHowl = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void FrostBite()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*Frost crystals erupt from the wolf's fangs!*");
                PlaySound(0x10B); // Cold attack

                AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 0, 100, 0, 0);
                target.Stam -= Utility.RandomMinMax(10, 20);
                target.SendMessage(0x480, "You are bitten by fangs of pure ice!");
            }

            m_NextFrostBite = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void ShatterLeap()
        {
            if (Combatant is Mobile target && this.InRange(target.Location, 10))
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The wolf leaps, shattering the ground with frozen fury!*");
                PlaySound(0x229);

                this.Location = target.Location; // Teleport to target

                foreach (Mobile m in GetMobilesInRange(2))
                {
                    if (m != this && m.Alive && !m.IsDeadBondedPet)
                    {
                        AOS.Damage(m, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);
                        m.SendMessage(0x480, "You are struck by the shock of shattering ice!");
                        m.Freeze(TimeSpan.FromSeconds(1));
                    }
                }
            }

            m_NextShatterLeap = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 3);

            if (Utility.RandomDouble() < 0.05) // 5% rare drop
                PackItem(new IcyFang());
        }

        public override int Meat => 3;
        public override int Hides => 8;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Canine;

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

    public class IcyFang : Item
    {
        [Constructable]
        public IcyFang() : base(0x1B76) // Dagger model or pick something icy
        {
            Name = "Icebound Fang";
            Hue = 1152;
            LootType = LootType.Blessed;
            Movable = true;
        }

        public IcyFang(Serial serial) : base(serial) { }

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
