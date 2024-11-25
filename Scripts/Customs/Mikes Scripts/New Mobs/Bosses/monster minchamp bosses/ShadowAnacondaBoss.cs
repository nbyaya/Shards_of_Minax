using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a shadow anaconda boss corpse")]
    public class ShadowAnacondaBoss : ShadowAnaconda
    {
        [Constructable]
        public ShadowAnacondaBoss() : base()
        {
            Name = "Shadow Anaconda, the Overlord";
            Title = "the Supreme Constrictor";
            
            // Update stats to match or exceed Barracoon's
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(150); // Matching Barracoon's upper dexterity
            SetInt(750); // Matching Barracoon's upper intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Same as Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75); // Improved resistance
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Boss fame level
            Karma = -22500; // Negative karma, like Barracoon

            VirtualArmor = 70; // Increased virtual armor for more defense

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

        public ShadowAnacondaBoss(Serial serial) : base(serial)
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
