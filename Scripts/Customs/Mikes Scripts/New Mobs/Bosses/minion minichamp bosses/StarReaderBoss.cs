using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the star overlord")]
    public class StarReaderBoss : StarReader
    {
        [Constructable]
        public StarReaderBoss() : base()
        {
            Name = "Star Overlord";
            Title = "the Supreme Star Reader";

            // Enhanced Stats to match or exceed Barracoon's values
            SetStr(425); // Enhanced Strength
            SetDex(200); // Enhanced Dexterity
            SetInt(750); // Enhanced Intelligence

            SetHits(12000); // Enhanced Hits, matching a boss-tier creature
            SetDamage(29, 38); // Damage range similar to Barracoon, adjust as needed

            // Enhanced Resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Enhanced Skills
            SetSkill(SkillName.EvalInt, 120.0); // High intelligence-related skills
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0); 
            SetSkill(SkillName.Wrestling, 100.0); // Higher skill in wrestling for defense

            Fame = 22500;
            Karma = -22500; // Boss-tier reputation

            VirtualArmor = 80; // Higher armor value

            // Attach a random ability from XmlSpawner2
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

            // Additional boss logic (e.g., casting buffs or dealing extra damage)
            // Further logic can be added as needed
        }

        public StarReaderBoss(Serial serial) : base(serial)
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
