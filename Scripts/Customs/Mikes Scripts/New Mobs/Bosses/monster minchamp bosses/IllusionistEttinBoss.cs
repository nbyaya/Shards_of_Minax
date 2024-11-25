using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an illusionist ettin's corpse")]
    public class IllusionistEttinBoss : IllusionistEttin
    {
        [Constructable]
        public IllusionistEttinBoss() : base()
        {
            Name = "Illusionist Ettin";
            Title = "the Supreme Illusionist";

            // Enhanced stats for the boss version
            SetStr(1200); // Stronger than the original
            SetDex(255); // Maxed dexterity
            SetInt(250); // Maxed intelligence

            SetHits(12000); // Increased health to match a boss-tier enemy
            SetDamage(35, 50); // Increased damage output

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistances
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Magery, 120.0, 140.0); // Increased skill levels
            SetSkill(SkillName.MagicResist, 130.0, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // Higher negative karma

            VirtualArmor = 100; // Boss-tier armor

            Tamable = false; // Not tamable for the boss version
            ControlSlots = 0;

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

        public IllusionistEttinBoss(Serial serial) : base(serial)
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
