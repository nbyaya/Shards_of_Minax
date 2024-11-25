using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Maine Coon Titan boss corpse")]
    public class MaineCoonTitanBoss : MaineCoonTitan
    {
        [Constructable]
        public MaineCoonTitanBoss() : base()
        {
            Name = "Maine Coon Titan Overlord";
            Title = "the Primal Warden";

            // Update stats to match or exceed the original Maine Coon Titan
            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300); // Increased dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(15000); // Increased health
            SetDamage(40, 55); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            Fame = 32000;
            Karma = -32000;

            VirtualArmor = 120;

            // Attach a random ability
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

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public MaineCoonTitanBoss(Serial serial) : base(serial)
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
