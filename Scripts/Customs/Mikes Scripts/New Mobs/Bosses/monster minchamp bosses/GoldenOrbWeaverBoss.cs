using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a golden orb weaver boss corpse")]
    public class GoldenOrbWeaverBoss : GoldenOrbWeaver
    {
        [Constructable]
        public GoldenOrbWeaverBoss() : base()
        {
            // Update name and title for boss
            Name = "Golden Orb Weaver Overlord";
            Title = "the Supreme Spider";

            // Enhanced stats to match or exceed the original stats
            SetStr(1200); // Higher strength
            SetDex(255); // Higher dexterity
            SetInt(250); // Higher intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Increased damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 70);

            // Improved skills
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            // Fame and Karma for boss
            Fame = 30000;
            Karma = -30000;

            // Enhanced virtual armor
            VirtualArmor = 120;

            // Attach the XmlRandomAbility for dynamic abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScroll items to loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Any additional boss logic could be added here if desired
        }

        public GoldenOrbWeaverBoss(Serial serial) : base(serial) { }

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
