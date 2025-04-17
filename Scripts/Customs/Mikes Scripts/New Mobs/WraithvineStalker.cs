using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a wraithvine husk")]
    public class WraithvineStalker : BaseCreature
    {
        private DateTime m_NextVineSpawn;

        [Constructable]
        public WraithvineStalker() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wraithvine stalker";
            Body = 780; // Using the "reaper" body for a twisted, plant-like undead look
            BaseSoundID = 442; // Eerie, ghostly sounds
            Hue = 1157; // A sickly greenish-gray hue

            SetStr(250, 300);
            SetDex(80, 120);
            SetInt(100, 150);

            SetHits(200, 250);
            SetMana(100);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 45;

            m_NextVineSpawn = DateTime.UtcNow;

            PackItem(new Bandage(Utility.RandomMinMax(5, 10))); // Some loot
            PackItem(new Vines()); // Thematic loot
            if (Utility.RandomDouble() < 0.05) // 5% chance for rare drop
                PackItem(new WraithvineSeed()); // Custom rare item
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);
        }

        public override bool BleedImmune => true; // Plant-like, no blood
        public override Poison PoisonImmune => Poison.Greater; // Resistant to poison
        public override bool CanRummageCorpses => true;

        public override void OnThink()
        {
            base.OnThink();

            // Custom ability: Spawn vine minions periodically
            if (DateTime.UtcNow >= m_NextVineSpawn && Combatant != null)
            {
                if (Utility.RandomDouble() < 0.3) // 30% chance per tick
                {
                    SpawnVineMinions();
                    m_NextVineSpawn = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
            }
        }

        private void SpawnVineMinions()
        {
            int vineCount = Utility.RandomMinMax(1, 2);
            for (int i = 0; i < vineCount; i++)
            {
                WraithvineTendril tendril = new WraithvineTendril();
                tendril.MoveToWorld(Location, Map);
                tendril.Combatant = Combatant;
                Effects.SendLocationEffect(tendril.Location, tendril.Map, 0x3728, 10, 10); // Visual effect for spawning
                PlaySound(0x1F3); // Creepy plant sound
            }
            Emote("*The wraithvine stalker writhes, and thorny tendrils burst from the ground!*");
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new WraithvineFragment()); // Guaranteed custom loot on death
        }

        public WraithvineStalker(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextVineSpawn = DateTime.UtcNow;
        }
    }

    // Minion class for the vine tendrils
    [CorpseName("a severed tendril")]
    public class WraithvineTendril : BaseCreature
    {
        [Constructable]
        public WraithvineTendril() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wraithvine tendril";
            Body = 8; // Vine-like body (snake-like for simplicity)
            Hue = 1157; // Matching hue with the stalker

            SetStr(50, 80);
            SetDex(60, 90);
            SetInt(20, 40);

            SetHits(40, 60);
            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 60);

            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 20;
        }

        public override bool BleedImmune => true;

        public WraithvineTendril(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Custom loot items (you can expand these further)
    public class WraithvineSeed : Item
    {
        [Constructable]
        public WraithvineSeed() : base(0x0F2D) // Seed graphic
        {
            Name = "wraithvine seed";
            Hue = 1157;
            Weight = 0.1;
        }

        public WraithvineSeed(Serial serial) : base(serial)
        {
        }

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

    public class WraithvineFragment : Item
    {
        [Constructable]
        public WraithvineFragment() : base(0x0C95) // Bone graphic as a placeholder
        {
            Name = "wraithvine fragment";
            Hue = 1157;
            Weight = 1.0;
        }

        public WraithvineFragment(Serial serial) : base(serial)
        {
        }

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