using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the escape master")]
    public class EscapeArtistBoss : EscapeArtist
    {
        [Constructable]
        public EscapeArtistBoss() : base()
        {
            Name = "Escape Master";
            Title = "the Ultimate Escape Artist";

            // Update stats to match or exceed Barracoon's stats
            SetStr(425); // Strength to match Barracoon's upper limit
            SetDex(400); // Dexterity enhanced for a boss-tier difficulty
            SetInt(750); // Intelligence maxed for enhanced magic resistance and abilities

            SetHits(12000); // Health to match Barracoon's value
            SetStam(300); // High stamina for better combat endurance
            SetMana(750); // Mana to match Barracoon's upper value

            SetDamage(29, 38); // Damage range to match Barracoon's value

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach the XmlRandomAbility for extra abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new BladeDancersPlateLegs());
			PackItem(new WorkersRevolutionChest());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic or custom speech could be added here
        }

        public EscapeArtistBoss(Serial serial) : base(serial)
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
