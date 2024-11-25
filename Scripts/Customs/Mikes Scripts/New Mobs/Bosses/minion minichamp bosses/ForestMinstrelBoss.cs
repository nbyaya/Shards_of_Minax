using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the forest overlord")]
    public class ForestMinstrelBoss : ForestMinstrel
    {
        [Constructable]
        public ForestMinstrelBoss() : base()
        {
            Name = "Forest Overlord";
            Title = "the Supreme Minstrel";

            // Update stats to match or exceed Barracoon's stats (or as required for the boss)
            SetStr(500, 700); // Enhanced Strength
            SetDex(250, 300); // Enhanced Dexterity
            SetInt(400, 600); // Enhanced Intelligence

            SetHits(3500, 5000); // Much higher health

            SetDamage(20, 30); // Increased damage

            SetResistance(ResistanceType.Physical, 60, 80); // Increased resistance
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 80.1, 100.0); // Increased skill range
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.Meditation, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.1, 110.0);
            SetSkill(SkillName.Musicianship, 120.0); // Max musicianship
            SetSkill(SkillName.Provocation, 100.0, 120.0); // Max provocation
            SetSkill(SkillName.Discordance, 100.0, 120.0); // Max discordance
            SetSkill(SkillName.Peacemaking, 100.0, 120.0); // Max peacemaking

            Fame = 10000; // Higher Fame for a boss
            Karma = -10000; // Higher Karma for a boss

            VirtualArmor = 60; // Increased armor for a boss

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

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic can be placed here, such as unique speech, abilities, etc.
        }

        public ForestMinstrelBoss(Serial serial) : base(serial)
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
