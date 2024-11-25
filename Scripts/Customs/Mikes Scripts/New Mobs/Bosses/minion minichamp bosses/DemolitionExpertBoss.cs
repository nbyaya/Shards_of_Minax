using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the demolition overlord")]
    public class DemolitionExpertBoss : DemolitionExpert
    {
        [Constructable]
        public DemolitionExpertBoss() : base()
        {
            Name = "Demolition Overlord";
            Title = "the Master of Destruction";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching Barracoon's upper strength or higher
            SetDex(255); // Matching the higher dexterity range
            SetInt(250); // Maximum intelligence for a stronger mage character

            SetHits(12000); // Boss-tier health
            SetDamage(29, 38); // Damage similar to Barracoon's boss-tier range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Keep full poison resistance
            SetResistance(ResistanceType.Energy, 70, 80);

            // Enhanced skill levels
            SetSkill(SkillName.MagicResist, 150.0); // Max MagicResist for a boss
            SetSkill(SkillName.EvalInt, 120.0); // Enhanced EvalInt for stronger spellcasting
            SetSkill(SkillName.Magery, 120.0); // Enhanced Magery for strong spellcasting
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.DetectHidden, 100.0); // Improved detection for traps
            SetSkill(SkillName.RemoveTrap, 100.0); // Increased ability to remove traps

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Enhanced Virtual Armor for added protection

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
			PackItem(new EtherealPlaneChest());
			PackItem(new CivilRightsStrongbox());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Enhanced loot
            PackGold(500, 700); // More gold for a boss-tier creature
            AddLoot(LootPack.Rich); // Rich loot pack for a boss

            // Specific drop messages
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say("This is just the beginning!"); break;
                case 1: this.Say("Your destruction was inevitable..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 30))); // Increased MandrakeRoot drop
        }

        public DemolitionExpertBoss(Serial serial) : base(serial)
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
