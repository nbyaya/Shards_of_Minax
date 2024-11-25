using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a volcanic titan overlord corpse")]
    public class VolcanicTitanBoss : VolcanicTitan
    {
        [Constructable]
        public VolcanicTitanBoss()
            : base()
        {
            // Boss specific changes
            Name = "Volcanic Titan Overlord";
            Title = "the Infernal Fury";

            // Update stats to match or exceed the original Volcanic Titan
            SetStr(1200); // Increased strength
            SetDex(255); // Increased dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // High health for boss-tier
            SetDamage(35, 45); // Increased damage

            // Increased resistances for a more durable boss
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhanced skills for a boss
            SetSkill(SkillName.Anatomy, 50.0, 80.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Higher fame for a boss-tier entity
            Karma = -30000; // Negative karma for a villainous boss

            VirtualArmor = 110; // Higher virtual armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Tamable, but this boss shouldn't be tamed
            Tamable = false;

            // Initializing loot, with 5 MaxxiaScroll items
            this.PackItem(new SulfurousAsh(10)); // Increased Sulfurous Ash as part of loot
        }

        public override void GenerateLoot()
        {
            // In addition to normal loot, drop 5 MaxxiaScrolls
            base.GenerateLoot();

            // Add 5 MaxxiaScroll items
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public VolcanicTitanBoss(Serial serial)
            : base(serial)
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
