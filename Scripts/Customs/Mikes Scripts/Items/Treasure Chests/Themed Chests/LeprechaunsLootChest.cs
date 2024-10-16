using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class LeprechaunsLootChest : WoodenChest
    {
        [Constructable]
        public LeprechaunsLootChest()
        {
            Name = "Leprechaun's Loot Chest";
            Hue = 2320;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel4>("Pot of Gold"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Rainbow's End Earring", 1380), 0.15);
            AddItem(new Gold(Utility.Random(1, 7000)), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Leprechaun's Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Little Folk", 1388), 0.15);
            AddItem(CreateRandomLoot(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Shamrock Scope"), 0.13);
            AddItem(CreateRandomWand(), 0.14);
            AddItem(CreateArmor(), 0.2);
            AddItem(CreateSandals(), 0.2);
            AddItem(CreatePlateGloves(), 0.2);
            AddItem(CreateBattleAxe(), 0.2);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Lucky Charm";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Catch me if you can, but the gold is mine to keep!";
            note.TitleString = "Leprechaun's Taunt";
            return note;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateRandomLoot()
        {
            // Create a random item: Armor, Shield, Weapon, or Jewelry
            Type[] lootTypes = new Type[]
            {
                typeof(LeatherChest), typeof(PlateChest), typeof(Shield), typeof(GoldBracelet), typeof(GoldRing)
                // Add other types as necessary
            };
            Type randomType = lootTypes[Utility.Random(lootTypes.Length)];
            Item item = (Item)Activator.CreateInstance(randomType);
            item.Name = "Lucky Defender's Shield";
            return item;
        }

        private Item CreateRandomWand()
        {
            // Create a random wand
            Shaft wand = new Shaft();
            wand.Name = "Wand of the Endless Jig";
            return wand;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = new LeatherChest(); // Use an appropriate armor type
            armor.Name = "Irish Luck Armor";
            armor.Hue = Utility.Random(1, 1344);
            armor.BaseArmorRating = Utility.Random(20, 50);
            return armor;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Whisperer's Sandals";
            sandals.Hue = Utility.RandomMinMax(500, 1400);
            sandals.ClothingAttributes.MageArmor = 1;
            sandals.Attributes.BonusDex = 5;
            sandals.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            sandals.SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
            return sandals;
        }

        private Item CreatePlateGloves()
        {
            PlateGloves gloves = new PlateGloves();
            gloves.Name = "Stormforged Gauntlets";
            gloves.Hue = Utility.RandomMinMax(550, 850);
            gloves.BaseArmorRating = Utility.Random(40, 70);
            gloves.AbsorptionAttributes.EaterFire = 15;
            gloves.ArmorAttributes.LowerStatReq = 15;
            gloves.Attributes.WeaponSpeed = 10;
            gloves.Attributes.BonusDex = 15;
            gloves.SkillBonuses.SetValues(0, SkillName.ArmsLore, 10.0);
            gloves.EnergyBonus = 15;
            gloves.ColdBonus = 5;
            gloves.FireBonus = 15;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateBattleAxe()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Quasar Axe";
            axe.Hue = Utility.RandomMinMax(650, 850);
            axe.MinDamage = Utility.Random(40, 80);
            axe.MaxDamage = Utility.Random(80, 120);
            axe.Attributes.WeaponSpeed = 5;
            axe.Attributes.AttackChance = 10;
            axe.Slayer = SlayerName.ReptilianDeath;
            axe.WeaponAttributes.HitLeechStam = 30;
            axe.WeaponAttributes.BattleLust = 20;
            axe.SkillBonuses.SetValues(0, SkillName.ArmsLore, 15.0);
            return axe;
        }

        public LeprechaunsLootChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
