using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the crossbow overlord")]
    public class CrossbowMarksmanBoss : CrossbowMarksman
    {
        [Constructable]
        public CrossbowMarksmanBoss() : base()
        {
            Name = "Crossbow Overlord";
            Title = "the Deadly Marksman";

            // Update stats to match or exceed Barracoon-like values
            SetStr(800); // Increased strength for a boss-tier NPC
            SetDex(255); // Upper dexterity for better speed and damage
            SetInt(250); // High intelligence for mana and resistances

            SetHits(7000); // Increased hit points for the boss version
            SetDamage(30, 45); // Increased damage output

            SetResistance(ResistanceType.Physical, 70, 85); // Stronger resistances
            SetResistance(ResistanceType.Fire, 60, 75);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Archery, 120.0); // Boosted archery skill
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);

            Fame = 15000; // Increased fame for the boss-tier
            Karma = -15000; // Increased karma penalty for the boss-tier

            VirtualArmor = 75; // Increased virtual armor for the boss

            // Attach a random ability for added challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new AeonianBow());
			PackItem(new DaggerOfShadows());            
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

        public CrossbowMarksmanBoss(Serial serial) : base(serial)
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
