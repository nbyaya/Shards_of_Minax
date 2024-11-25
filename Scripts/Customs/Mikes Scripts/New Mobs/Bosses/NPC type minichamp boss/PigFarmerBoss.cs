using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the pig overlord")]
    public class PigFarmerBoss : PigFarmer
    {
        [Constructable]
        public PigFarmerBoss() : base()
        {
            Name = "Pig Overlord";
            Title = "the Supreme Farmer";

            // Enhance the stats to match or exceed Barracoon-like values
            SetStr(1200); // Upper strength
            SetDex(255); // Upper dexterity
            SetInt(250); // Upper intelligence

            SetHits(12000); // Significantly higher health
            SetDamage(20, 30); // Slightly enhanced damage

            // Boost the resistances
            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 80, 95);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skill updates
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            
            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Increase virtual armor

            // Attach the XmlRandomAbility for enhanced behavior
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
            // Add any boss-specific thinking logic if needed here
        }

        public PigFarmerBoss(Serial serial) : base(serial)
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
