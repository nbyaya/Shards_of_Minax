using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss iron golem corpse")]
    public class BossIronGolem : IronGolem
    {
        [Constructable]
        public BossIronGolem() : base()
        {
            // Update to boss-level stats
            Name = "Iron Golem";
            Title = "the Colossus";
            Hue = 1935; // Unique hue for Boss Iron Golem
            BaseSoundID = 357;

            // Enhanced Stats
            SetStr(1500, 1800);
            SetDex(300, 350);
            SetInt(300, 450);
            SetHits(15000, 20000);
            SetDamage(45, 60);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 85, 95);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 105.5, 120.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 150.5, 200.0);
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 100;

            // Additional features
            Tamable = false;
            ControlSlots = 5; // Increased control slots for bosses
            MinTameSkill = 120.0;

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());


        }

        public override void GenerateLoot()
        {
            // Add additional loot for boss
            base.GenerateLoot();
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 10);

            // Boss-specific loot: MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original abilities
        }

        public BossIronGolem(Serial serial) : base(serial)
        {
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
        }
    }
}
