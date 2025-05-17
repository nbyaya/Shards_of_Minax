using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frostbrand warrior corpse")]
    public class FrostbrandWarrior : BaseCreature
    {
        private DateTime m_NextIceNova;
        private DateTime m_NextFrostblink;
        private DateTime m_NextFrozenWail;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostbrandWarrior()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frostbrand Warrior";
            Body = 764; // Same as JukaWarrior
            Hue = 1150; // Unique icy blue tone
            BaseSoundID = 0x1AC;

            SetStr(500, 650);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(850, 1000);
            SetMana(500, 700);

            SetDamage(20, 28);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            m_AbilitiesInitialized = false;
        }

        public FrostbrandWarrior(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer => true;
        public override bool CanRummageCorpses => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.FilthyRich, 1);
            AddLoot(LootPack.Gems, 4);
            if (Utility.RandomDouble() < 0.005)
                PackItem(new IceRelic()); // Rare drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextFrostblink = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(5, 15));
                    m_NextFrozenWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextIceNova)
                    IceNova();

                if (DateTime.UtcNow >= m_NextFrostblink)
                    Frostblink();

                if (DateTime.UtcNow >= m_NextFrozenWail)
                    FrozenWail();
            }
        }

        private void IceNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* A freezing explosion erupts around the Frostbrand Warrior! *");
            PlaySound(0x10B); // Chilling sound effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 0, 100, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendMessage(0x480, "You are frozen by the Ice Nova!");
                }
            }

            m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void Frostblink()
        {
            if (Combatant != null && Utility.RandomDouble() < 0.75)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Frostbrand Warrior vanishes in a blink of cold mist! *");
                Effects.SendLocationEffect(Location, Map, 0x3728, 10); // Blink effect
                Point3D newLoc = new Point3D(Combatant.X + Utility.RandomMinMax(-1, 1), Combatant.Y + Utility.RandomMinMax(-1, 1), Combatant.Z);
                MoveToWorld(newLoc, Map);
                Effects.SendLocationEffect(newLoc, Map, 0x3728, 10);
            }

            m_NextFrostblink = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void FrozenWail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Frostbrand Warrior unleashes a chilling wail... *");
            PlaySound(0x1D0); // Wail sound

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage(0x480, "A wave of fear washes over you...");
                    m.Stam -= Utility.RandomMinMax(10, 20);
                    m.Mana -= Utility.RandomMinMax(10, 25);
                }
            }

            m_NextFrozenWail = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                AOS.Damage(defender, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                if (defender is Mobile target)
                {
                    target.Freeze(TimeSpan.FromSeconds(2));
                    target.SendMessage(0x480, "You are chilled to the bone by the warrior's touch!");
                }
            }
        }

        public override int GetIdleSound() => 0x1AC;
        public override int GetAngerSound() => 0x1CD;
        public override int GetHurtSound() => 0x1D0;
        public override int GetDeathSound() => 0x28D;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class IceRelic : Item
    {
        public IceRelic() : base(0xF15) // Example hue/item graphic
        {
            Hue = 1150;
            Name = "a relic of frozen legacy";
            Weight = 1.0;
            Movable = true;
        }

        public IceRelic(Serial serial) : base(serial) { }

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
