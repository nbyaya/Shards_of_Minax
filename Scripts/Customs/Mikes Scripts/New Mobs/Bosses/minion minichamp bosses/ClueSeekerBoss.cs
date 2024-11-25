using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the clue seeker overlord")]
    public class ClueSeekerBoss : ClueSeeker
    {
        [Constructable]
        public ClueSeekerBoss() : base()
        {
            Name = "Clue Seeker Overlord";
            Title = "the Supreme Seeker";

            // Update stats to match or exceed Barracoon's values
            SetStr(800); // Increased strength
            SetDex(255); // Increased dexterity
            SetInt(350); // Increased intelligence

            SetHits(12000); // Increased health to make it a boss-tier creature

            SetDamage(29, 38); // Increased damage range for boss difficulty

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resistance
            SetSkill(SkillName.Tactics, 120.0);     // Increased tactics for harder combat
            SetSkill(SkillName.Wrestling, 120.0);   // Increased wrestling skill for the boss
            SetSkill(SkillName.DetectHidden, 120.0); // Enhanced skill to detect hidden targets

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

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
			PackItem(new MythicDiamond());
			PackItem(new MelodicCirclet());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public ClueSeekerBoss(Serial serial) : base(serial)
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
