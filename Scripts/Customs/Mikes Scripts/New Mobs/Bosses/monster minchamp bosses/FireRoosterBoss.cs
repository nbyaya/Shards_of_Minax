using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a fire rooster corpse")]
    public class FireRoosterBoss : FireRooster
    {
        private DateTime m_NextInfernoEgg;

        [Constructable]
        public FireRoosterBoss() : base()
        {
            Name = "Fire Rooster Overlord";
            Title = "the Inferno King";
            Hue = 1380; // Fiery red hue for distinction
            BaseSoundID = 0x6E; // Standard chicken sound

            // Enhanced Stats based on FireRooster
            SetStr(1200); // Boosted strength
            SetDex(255); // Boosted dexterity
            SetInt(250); // Boosted intelligence

            SetHits(12000); // High health, boss tier
            SetDamage(35, 50); // Increased damage for boss difficulty

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 35);
            SetDamageType(ResistanceType.Energy, 15);

            SetResistance(ResistanceType.Physical, 75, 90); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // High fire resistance
            SetResistance(ResistanceType.Cold, 60, 75); // Moderate cold resistance
            SetResistance(ResistanceType.Poison, 100); // Full poison resistance
            SetResistance(ResistanceType.Energy, 60, 75); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Strong magic resistance skill
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics for combat
            SetSkill(SkillName.Wrestling, 120.0); // Strong wrestling skill for combat
            SetSkill(SkillName.Magery, 100.0); // Magery skill for potential spellcasting

            Fame = 30000; // High fame value
            Karma = -30000; // Negative karma, fits a boss NPC

            VirtualArmor = 120; // Enhanced armor for durability

            // Attach random abilities via XML
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextInfernoEgg = DateTime.UtcNow;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Additional loot for a boss-tier NPC
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Optionally, you could add more advanced logic here for the boss's AI
        }
		
        public FireRoosterBoss(Serial serial)
            : base(serial)
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
