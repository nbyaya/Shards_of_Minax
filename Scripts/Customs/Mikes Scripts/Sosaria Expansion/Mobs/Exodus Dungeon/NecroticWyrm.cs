using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a necrotic wyrm corpse")]
    public class NecroticWyrm : BaseCreature
    {
        private DateTime m_NextNecroticBreath;
        private DateTime m_NextSoulDrain;
        private DateTime m_NextBoneStorm;

        [Constructable]
        public NecroticWyrm()
            : base(AIType.AI_NecroMage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Necrotic Wyrm";
            Body = 104; // Skeletal Drake Body
            Hue = 2966; // Unnatural eerie green hue
            BaseSoundID = 0x488;


            SetStr(1000, 1200);
            SetDex(80, 120);
            SetInt(400, 600);

            SetHits(1200, 1500);
            SetMana(1000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 90.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (DateTime.UtcNow >= m_NextNecroticBreath)
                UseNecroticBreath();

            if (DateTime.UtcNow >= m_NextSoulDrain)
                UseSoulDrain();

            if (DateTime.UtcNow >= m_NextBoneStorm)
                UseBoneStorm();
        }

        private void UseNecroticBreath()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Necrotic Wyrm exhales a cloud of rotting death!*");
                PlaySound(0x56D);
                Effects.SendLocationEffect(target.Location, Map, 0x3709, 10, 1, Hue, 0);

                AOS.Damage(target, this, Utility.RandomMinMax(30, 60), 0, 0, 100, 0, 0);
                target.ApplyPoison(this, Poison.Lethal);
                target.SendMessage("You feel your flesh begin to rot from within!");

                m_NextNecroticBreath = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            }
        }

        private void UseSoulDrain()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Necrotic Wyrm drains the soul of its foe!*");
                PlaySound(0x1F9);

                int drain = Utility.RandomMinMax(20, 40);
                target.Mana -= drain;
                target.Stam -= drain;

                Hits += drain; // Heals the wyrm slightly
                target.SendMessage("You feel your life force slipping away...");
                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(60);
            }
        }

        private void UseBoneStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Necrotic Wyrm summons a cyclone of bones!*");
            PlaySound(0x64B);
            Effects.PlaySound(Location, Map, 0x307);
            Effects.SendLocationEffect(Location, Map, 0x376A, 20, 10, 0x480, 0);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);
                    m.SendMessage("You are shredded by whirling bone fragments!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Briefly freezes
                }
            }

            m_NextBoneStorm = DateTime.UtcNow + TimeSpan.FromSeconds(90);
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.005) // 0.5% drop chance
            {
                PackItem(new NecroticWyrmHeart());
            }
        }

        public override int TreasureMapLevel => 5;

        public NecroticWyrm(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NecroticWyrmHeart : Item
    {
        [Constructable]
        public NecroticWyrmHeart() : base(0x1CF0)
        {
            Name = "heart of the Necrotic Wyrm";
            Hue = 2966;
            Weight = 1.0;
        }

        public NecroticWyrmHeart(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
