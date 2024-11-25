using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the terrain overlord")]
    public class TerrainScoutBoss : TerrainScout
    {
        [Constructable]
        public TerrainScoutBoss() : base()
        {
            Name = "Terrain Overlord";
            Title = "the Supreme Scout";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(800); // Increase Strength significantly
            SetDex(200); // Max out Dexterity for high agility
            SetInt(400); // Max out Intelligence for better spellcasting

            SetHits(12000); // Boss health
            SetDamage(25, 40); // Increased damage range

            // Enhanced resistances for a boss-tier challenge
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Update skills to make it tougher
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Increased armor for better defense

            // Attach a random ability to enhance the fight dynamically
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
            // Additional boss-specific behavior could go here
        }

        public TerrainScoutBoss(Serial serial) : base(serial)
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
