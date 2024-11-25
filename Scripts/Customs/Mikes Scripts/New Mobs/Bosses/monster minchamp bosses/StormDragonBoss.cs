using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a storm overlord corpse")]
    public class StormDragonBoss : StormDragon
    {
        [Constructable]
        public StormDragonBoss() : base()
        {
            Name = "Storm Overlord";
            Title = "the Supreme Storm Dragon";
            Hue = 1481; // Keeps the storm theme

            // Upgrade stats to match or exceed Barracoon's stats
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(255); // Max dexterity from original StormDragon's max dexterity
            SetInt(750); // Upgraded to match Barracoon's upper intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Boss-level damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 75, 85);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90; // High virtual armor for a boss

            // Attach the XmlRandomAbility to provide random abilities
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
            // Additional boss logic can go here, such as using abilities more strategically
        }

        public StormDragonBoss(Serial serial) : base(serial)
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
