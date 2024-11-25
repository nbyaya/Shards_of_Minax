using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a nyx'rith corpse")]
    public class NyxRithBoss : NyxRith
    {
        [Constructable]
        public NyxRithBoss() : base()
        {
            Name = "Nyx'Rith the Devourer";
            Title = "the Eternal Devourer";
            Hue = 1769; // Dark purple hue (same as original)

            // Update stats to match or exceed Barracoon (adjust as needed for balance)
            SetStr(1200, 1600); // Higher strength
            SetDex(255, 350); // Higher dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(12000, 16000); // Increased health for boss-tier challenge
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Higher resistance
            SetResistance(ResistanceType.Fire, 80, 90); 
            SetResistance(ResistanceType.Cold, 70, 80); 
            SetResistance(ResistanceType.Poison, 75, 85); 
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Stronger resist
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 120.0); // Stronger wrestling

            Fame = 30000; // Higher fame for boss-tier creature
            Karma = -30000; // Boss-tier negative karma

            VirtualArmor = 100; // Stronger armor

            // Attach a random ability (similar to what was done in Anvil Hurler boss)
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

        public NyxRithBoss(Serial serial) : base(serial)
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
