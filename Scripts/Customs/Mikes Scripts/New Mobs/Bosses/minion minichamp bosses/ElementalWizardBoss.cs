using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the elemental overlord")]
    public class ElementalWizardBoss : ElementalWizard
    {
        [Constructable]
        public ElementalWizardBoss() : base()
        {
            Name = "Elemental Overlord";
            Title = "the Supreme Wizard";

            // Enhanced stats for a boss version
            SetStr(800); // Matching the upper range of strength and higher than original
            SetDex(200); // Enhanced dexterity
            SetInt(300); // Keeping intelligence the same or better

            SetHits(12000); // Boss-tier health
            SetDamage(35, 50); // Increased damage range

            // Increased resistances to make it tougher
            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 70, 90);

            // Enhanced skills for a more dangerous boss fight
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // High negative karma for a boss villain

            VirtualArmor = 80; // Increased virtual armor for better defense

            // Attach a random ability to the boss version
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new RetroArcadeChest());
			PackItem(new WarriorsBelt());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional logic for the boss could be added here if needed (like special attacks)
        }

        public ElementalWizardBoss(Serial serial) : base(serial)
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
