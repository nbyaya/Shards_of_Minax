using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a flame overlord corpse")]
    public class FlameBearBoss : FlameBear
    {
        [Constructable]
        public FlameBearBoss() : base()
        {
            Name = "Flame Overlord";
            Title = "the Infernal Terror";

            // Update stats to match or exceed Barracoon's boss-tier stats
            SetStr(1200); // Increased strength
            SetDex(255); // Increased dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Increased health
            SetDamage(50, 70); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Increased skill for magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 120; // Increased virtual armor

            Tamable = false; // Boss-tier, not tamable
            ControlSlots = 3;
            MinTameSkill = 93.9;

            // Attach the XmlRandomAbility for random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public FlameBearBoss(Serial serial) : base(serial)
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
