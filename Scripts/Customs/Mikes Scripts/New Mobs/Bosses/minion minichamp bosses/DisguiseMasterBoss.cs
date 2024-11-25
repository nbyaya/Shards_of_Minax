using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the disguise overlord")]
    public class DisguiseMasterBoss : DisguiseMaster
    {
        [Constructable]
        public DisguiseMasterBoss() : base()
        {
            Name = "Disguise Overlord";
            Title = "the Supreme Master of Disguise";

            // Enhance stats to make it boss-tier, similar to Barracoon
            SetStr(900, 1200); // Increased strength
            SetDex(300, 400); // Increased dexterity
            SetInt(500, 700); // Increased intelligence

            SetHits(12000); // Set boss health
            SetDamage(29, 38); // Match Barracoon's damage range

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 75); // Increased resistance
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Increased skill range
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 75;

            // Attach the XmlRandomAbility for extra randomness
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new TreasureChestOfTheThreeKingdoms());
			PackItem(new BloodSwarmGem());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic or behaviors can be added here
        }

        public DisguiseMasterBoss(Serial serial) : base(serial)
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
