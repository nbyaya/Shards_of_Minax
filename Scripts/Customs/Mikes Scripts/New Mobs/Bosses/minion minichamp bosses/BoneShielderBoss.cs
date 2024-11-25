using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the bone overlord")]
    public class BoneShielderBoss : BoneShielder
    {
        [Constructable]
        public BoneShielderBoss() : base()
        {
            Name = "Bone Overlord";
            Title = "the Bone Shielder";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(150);  // Enhanced Dexterity
            SetInt(750);  // Enhanced Intelligence

            SetHits(12000); // Increased health to make it more challenging

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 75, 85);

            SetSkill(SkillName.MagicResist, 100.0);  // Higher Magic Resist
            SetSkill(SkillName.Tactics, 120.0);      // Higher Tactics
            SetSkill(SkillName.Wrestling, 120.0);    // Higher Wrestling
            SetSkill(SkillName.Magery, 110.0);      // Enhanced Magery

            Fame = 22500;  // Increased Fame for a boss
            Karma = -22500; // Karma for a villainous NPC

            VirtualArmor = 100; // Enhanced Armor

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
			PackItem(new HealersBlessedSandals());
			PackItem(new ForbiddenAlchemistsCache());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public BoneShielderBoss(Serial serial) : base(serial)
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
