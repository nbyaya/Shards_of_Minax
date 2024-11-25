using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a vitrail the mosaic corpse")]
    public class VitrailTheMosaicBoss : VitrailTheMosaic
    {
        [Constructable]
        public VitrailTheMosaicBoss() : base()
        {
            Name = "Vitrail the Mosaic, Supreme";
            Title = "the Shattered";

            // Enhance stats to match or exceed Barracoon
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Enhanced dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(12000); // Enhanced health to boss-tier level
            SetDamage(35, 45); // Enhanced damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;

            Tamable = false; // Ensure the boss can't be tamed

            // Attach a random ability (XmlRandomAbility)
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

            // You can add any specific behavior for the boss here, if needed
        }

        public VitrailTheMosaicBoss(Serial serial) : base(serial)
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
