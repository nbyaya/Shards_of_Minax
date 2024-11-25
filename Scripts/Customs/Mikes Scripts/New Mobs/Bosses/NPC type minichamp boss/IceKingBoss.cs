using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Ice King Supreme")]
    public class IceKingBoss : IceKing
    {
        [Constructable]
        public IceKingBoss() : base()
        {
            // Update the name and title to reflect the boss version
            Name = "Ice King Supreme";
            Title = "the Frozen Overlord";

            // Enhance stats to be more powerful (matching or exceeding Barracoon's stats)
            SetStr(1200); // Increased strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Increased health for the boss
            SetDamage(30, 40); // Higher damage output

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 90); // Cold resistance increased significantly
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            // Increased skill levels
            SetSkill(SkillName.MagicResist, 150.0); // Maximum magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill

            Fame = 22500; // Increased fame for boss
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 80; // Higher armor for better defense

            // Attach the XmlRandomAbility to give the boss extra abilities
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
            
            // Add additional loot
            PackGem();
            PackGold(500, 1000); // Boss-level gold drop
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic for summons or other behaviors could go here
        }

        public IceKingBoss(Serial serial) : base(serial)
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
