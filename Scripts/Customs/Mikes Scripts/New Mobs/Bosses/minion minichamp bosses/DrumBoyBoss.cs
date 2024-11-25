using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the drum overlord")]
    public class DrumBoyBoss : DrumBoy
    {
        [Constructable]
        public DrumBoyBoss() : base()
        {
            Name = "Drum Overlord";
            Title = "the Supreme Drummer";

            // Update stats to match or exceed the original DrumBoy's capabilities
            SetStr(700, 900);  // Enhanced strength
            SetDex(200, 250);  // Enhanced dexterity
            SetInt(400, 500);  // Enhanced intelligence

            SetHits(12000);  // Matching Barracoon-style health for a boss
            SetDamage(25, 40);  // Higher damage to match boss-tier difficulty

            SetDamageType(ResistanceType.Physical, 50);  // Keep the original damage types
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 80);  // Enhanced resistances
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);  // Enhanced skills for boss-level challenge
            SetSkill(SkillName.EvalInt, 100.1, 150.0);
            SetSkill(SkillName.Magery, 120.5, 150.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 150.5, 200.0);
            SetSkill(SkillName.Tactics, 100.1, 125.0);
            SetSkill(SkillName.Wrestling, 100.1, 125.0);

            Fame = 22500;  // High fame to reflect the boss's status
            Karma = -22500;  // Negative karma to match the enemy nature

            VirtualArmor = 70;  // Enhanced virtual armor

            // Attach a random ability to make the fight more dynamic
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new BushidoAugmentCrystal());
			PackItem(new EliteFoursVault());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // You could add other loot here if needed, e.g., rare items.
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic (e.g., ability usage or visual effects) could be added here.
        }

        public DrumBoyBoss(Serial serial) : base(serial)
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
