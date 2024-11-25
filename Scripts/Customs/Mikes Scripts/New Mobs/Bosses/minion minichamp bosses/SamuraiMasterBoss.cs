using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Samurai Overlord")]
    public class SamuraiMasterBoss : SamuraiMaster
    {
        [Constructable]
        public SamuraiMasterBoss() : base()
        {
            Name = "Samurai Overlord";
            Title = "the Supreme Samurai";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1300); // Matching the upper bound of Barracoon's strength
            SetDex(270); // Upper bound of Barracoon's dexterity
            SetInt(260); // Higher intelligence for the boss

            SetHits(12000); // Increased health for a boss-tier fight
            SetDamage(29, 38); // Match Barracoon's damage range (or set to be slightly higher if desired)

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Maxed resistance to poison for the boss
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Enhanced magic resistance for boss difficulty
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Maxed tactics
            SetSkill(SkillName.Wrestling, 120.0, 140.0); // Maxed wrestling
            SetSkill(SkillName.Bushido, 140.0, 160.0); // Boss-level Bushido skill
            SetSkill(SkillName.Parry, 140.0, 160.0); // Maxed parry skill for better defense

            Fame = 30000; // Increased fame for a boss-tier NPC
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 80; // Increased armor for better defense

            // Attach a random ability for dynamic combat
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
            // Additional boss behavior can be added here (for example, special abilities, etc.)
        }

        public SamuraiMasterBoss(Serial serial) : base(serial)
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
