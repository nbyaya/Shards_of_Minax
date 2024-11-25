using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the mighty groovy mushroom spirit")]
    public class FunkFungiFamiliarBoss : FunkFungiFamiliar
    {
        [Constructable]
        public FunkFungiFamiliarBoss() : base()
        {
            Name = "Mighty Funk Fungi";
            Title = "the Spore Overlord";

            // Enhanced stats to match or exceed those of Barracoon or better
            SetStr(1200); // Upper bound for strength, stronger than original
            SetDex(255);  // Upper bound for dexterity
            SetInt(250);  // Upper bound for intelligence

            SetHits(12000); // High health to match the boss tier

            SetDamage(29, 38); // Boss damage range similar to Barracoon

            // Resistance improvements to make it a more formidable opponent
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach a random ability to enhance its combat tactics
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

            // Additional loot could be added here
        }

        public override void OnThink()
        {
            base.OnThink();
            // Boss-level behaviors could be expanded here if needed
        }

        public FunkFungiFamiliarBoss(Serial serial) : base(serial)
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
