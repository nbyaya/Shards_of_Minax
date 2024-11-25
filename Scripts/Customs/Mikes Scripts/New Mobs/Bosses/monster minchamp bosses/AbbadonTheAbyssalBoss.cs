using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an abyssal corpse")]
    public class AbbadonTheAbyssalBoss : AbbadonTheAbyssal
    {
        [Constructable]
        public AbbadonTheAbyssalBoss() : base()
        {
            Name = "Abbadon the Abyssal Overlord";
            Title = "the Supreme of the Abyss";

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Upper dexterity
            SetInt(750); // Upper intelligence

            SetHits(12000); // Boss health
            SetDamage(35, 50); // Enhanced damage

            // Update resistances to make it tougher
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90; // Increased virtual armor for better defense

            // Attach the random ability XML
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

        public AbbadonTheAbyssalBoss(Serial serial) : base(serial)
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
