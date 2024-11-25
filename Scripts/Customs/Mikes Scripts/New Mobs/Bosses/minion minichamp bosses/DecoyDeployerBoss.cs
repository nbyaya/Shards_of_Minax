using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the decoy overlord")]
    public class DecoyDeployerBoss : DecoyDeployer
    {
        [Constructable]
        public DecoyDeployerBoss() : base()
        {
            Name = "Decoy Overlord";
            Title = "the Master Deployer";

            // Update stats to match or exceed Barracoon (for higher stats)
            SetStr(800); // Higher than the base
            SetDex(200); // Max dexterity
            SetInt(700); // Max intelligence

            SetHits(12000); // Higher than base
            SetDamage(25, 35); // Higher damage range

            SetResistance(ResistanceType.Physical, 75);  // Higher resistance
            SetResistance(ResistanceType.Fire, 75);      // Higher resistance
            SetResistance(ResistanceType.Cold, 60);      // Higher resistance
            SetResistance(ResistanceType.Poison, 100);  // Maxed Poison resistance
            SetResistance(ResistanceType.Energy, 75);   // Higher resistance

            SetSkill(SkillName.MagicResist, 150.0);  // Maxed Magic Resist
            SetSkill(SkillName.Tactics, 120.0);      // Maxed Tactics
            SetSkill(SkillName.Wrestling, 120.0);    // Maxed Wrestling

            Fame = 22500;  // Boss-level fame
            Karma = -22500; // Boss-level karma

            VirtualArmor = 80;  // Higher armor value

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new JestersTricksterBoots());
			PackItem(new DragonbornChestplate());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, e.g., special combat behavior
        }

        public DecoyDeployerBoss(Serial serial) : base(serial)
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
