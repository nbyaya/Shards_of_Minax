using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the combat overlord")]
    public class CombatMedicBoss : CombatMedic
    {
        [Constructable]
        public CombatMedicBoss() : base()
        {
            Name = "Combat Overlord";
            Title = "the Supreme Medic";

            // Enhance the stats to match or exceed Barracoon-like standards
            SetStr(600); // More strength than the original
            SetDex(200); // Higher dexterity for a more agile boss
            SetInt(300); // More intelligence

            SetHits(12000); // More health for a boss-tier NPC
            SetDamage(20, 40); // Increased damage range

            // Enhanced resistances (tougher than the original)
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 50);

            // Enhanced skills (higher than the original)
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.Healing, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.MagicResist, 95.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 10000; // Higher fame
            Karma = -10000; // Higher negative karma for a boss-tier entity

            VirtualArmor = 50; // Increased virtual armor

            // Attach the XmlRandomAbility for additional random power
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new StatCapOrb());
			PackItem(new SkillOrb());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // You can add more boss-specific behavior here if needed
        }

        public CombatMedicBoss(Serial serial) : base(serial)
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
