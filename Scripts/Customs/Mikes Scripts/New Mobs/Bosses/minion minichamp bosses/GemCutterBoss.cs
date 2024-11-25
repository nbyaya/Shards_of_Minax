using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the gem overlord")]
    public class GemCutterBoss : GemCutter
    {
        [Constructable]
        public GemCutterBoss() : base()
        {
            Name = "Gem Overlord";
            Title = "the Supreme Cutter";

            // Enhance stats to be comparable or superior to Barracoon
            SetStr(800, 1000); // Increased strength
            SetDex(200, 250); // Increased dexterity
            SetInt(150, 200); // Increased intelligence

            SetHits(12000); // Set higher health
            SetDamage(30, 40); // Increased damage

            // Set better resistances
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Archery, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);

            Fame = 12000; // Increased fame for a boss-tier entity
            Karma = -12000; // Negative karma for boss character

            VirtualArmor = 60;

            // Attach the XmlRandomAbility
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
            // You can add any additional behavior for the boss here, like special attacks
        }

        public GemCutterBoss(Serial serial) : base(serial)
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
