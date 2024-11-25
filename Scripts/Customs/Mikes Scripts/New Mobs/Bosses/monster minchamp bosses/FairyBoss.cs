using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Fairy boss corpse")]
    public class FairyBoss : Fairy
    {
        [Constructable]
        public FairyBoss() : base()
        {
            Name = "Fairy Queen";
            Title = "the Supreme Fairy";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Higher strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // Max intelligence

            SetHits(12000); // High health for a boss

            SetDamage(35, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 80);  // Improved resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;  // Higher virtual armor

            // Attach the XmlRandomAbility for dynamic abilities
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
            // Additional boss behavior can be added here if needed
        }

        public FairyBoss(Serial serial) : base(serial)
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
