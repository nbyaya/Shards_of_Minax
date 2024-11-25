using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the legendary explorer")]
    public class ExplorerBoss : Explorer
    {
        [Constructable]
        public ExplorerBoss() : base()
        {
            Name = "Legendary Explorer";
            Title = "the Master Adventurer";

            // Update stats to match or exceed Barracoon's stats (or better)
            SetStr(800); // Increased strength for boss-level difficulty
            SetDex(300); // Increased dexterity for more attack speed
            SetInt(250); // Increased intelligence

            SetHits(12000); // Boss-level health
            SetStam(300); // Increased stamina
            SetMana(750); // Increased mana

            SetDamage(20, 30); // Increased damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Archery, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Tracking, 120.0);
            SetSkill(SkillName.Meditation, 100.0); // For better mana regeneration

            Fame = 22500; // High fame for a boss
            Karma = -22500; // Boss-level negative karma

            VirtualArmor = 70; // Increased virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add boss loot such as richer items, higher gold, and a unique item drop
            PackGold(500, 1000); // More gold for a boss
            AddLoot(LootPack.Rich);
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new StormforgedGauntlets());
			PackItem(new VolendrungWarHammer());
            // Add specific phrases related to the boss fight
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "You won't survive this encounter!"); break;
                case 1: this.Say(true, "I have traveled the world, but this... will be my final journey!"); break;
            }

            // Add some special potions to the loot
            PackItem(new GreaterHealPotion());
            PackItem(new CurePotion());
        }

        public override void OnThink()
        {
            base.OnThink();
            
            // Boss-specific actions could be added here, like unique speech or abilities
        }

        public ExplorerBoss(Serial serial) : base(serial)
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
