using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the enchanter lord")]
    public class EnchanterBoss : Enchanter
    {
        [Constructable]
        public EnchanterBoss() : base()
        {
            Name = "Enchanter Lord";
            Title = "the Grand Sorcerer";

            // Update stats to match or exceed Barracoon (use better stats where appropriate)
            SetStr(600, 800); // Higher strength than the original Enchanter
            SetDex(150); // Keep dexterity at the upper end
            SetInt(700, 900); // Higher intelligence than the original Enchanter

            SetHits(12000); // Boss-tier health
            SetDamage(20, 40); // Increase damage to make it more challenging

            // Update resistance to be more robust
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Keep or increase skill values
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 115.0, 125.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 85.0);
            SetSkill(SkillName.Wrestling, 60.0, 75.0);

            Fame = 22500; // Higher fame
            Karma = -22500; // Higher karma

            VirtualArmor = 60; // Boss-level virtual armor

            // Attach the XmlRandomAbility for extra random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new MysteryOrb());
			PackItem(new BavarianFestChest());
            // Drop 5 MaxxiaScrolls in addition to normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could go here if needed
        }

        public EnchanterBoss(Serial serial) : base(serial)
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
