using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the mighty fairy queen")]
    public class FairyQueenBoss : FairyQueen
    {
        [Constructable]
        public FairyQueenBoss() : base()
        {
            Name = "Mighty Fairy Queen";
            Title = "the Enchantress of the Wild";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Stronger than the original FairyQueen
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(10000); // Increased health for boss tier
            SetDamage(25, 40); // Enhanced damage range for the boss

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 80;

            // Attach the XmlRandomAbility to provide dynamic abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic could be added here, such as using more advanced spells, etc.
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

        public FairyQueenBoss(Serial serial) : base(serial)
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
