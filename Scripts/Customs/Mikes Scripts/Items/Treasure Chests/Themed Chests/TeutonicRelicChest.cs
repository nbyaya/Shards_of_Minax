using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TeutonicRelicChest : WoodenChest
    {
        [Constructable]
        public TeutonicRelicChest()
        {
            Name = "Teutonic Relic Chest";
            Hue = 2101;

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateEmerald(), 0.18);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel3>("Teutonic Cross Relic"), 0.17);
            AddItem(CreateColoredItem<GoldEarrings>("Cross of the Order Earring", 2105), 0.17);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.18);
            AddItem(CreateNamedItem<Apple>("Black Forest Apple"), 0.11);
            AddItem(CreateNamedItem<GreaterHealPotion>("Meisters Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Crusader", 2104), 0.15);
            AddItem(CreateRandomWeapon(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Knight's Vigilance Spyglass"), 0.12);
            AddItem(CreateRandomShield(), 0.13);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateAxe(), 0.20);
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
            emerald.Name = "Green of Saxony";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Honor, bravery, and legacy.";
            note.TitleString = "Teutonic Code";
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

        private Item CreateRandomWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword(), new Broadsword(), new VikingSword()); // Example random weapons
            weapon.Name = "Sword of the Teutons";
            return weapon;
        }

        private Item CreateRandomShield()
        {
            BaseShield shield = Utility.RandomList<BaseShield>(new WoodenShield(), new MetalShield()); // Example random shields
            shield.Name = "Shield of the Teutonic Order";
            return shield;
        }

        private Item CreateRandomClothing()
        {
            BaseClothing clothing = Utility.RandomList<BaseClothing>(new Robe(), new Tunic(), new Doublet()); // Example random clothing
            clothing.Name = "Teutonic Knight's Robe";
            return clothing;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword();
            weapon.Name = "Blade of the Rhinelands";
            weapon.Hue = Utility.RandomList(2110);
            weapon.MaxDamage = Utility.Random(25, 65);
            return weapon;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Sorceress's Midnight Robe";
            robe.Hue = Utility.RandomMinMax(400, 1500);
            robe.ClothingAttributes.SelfRepair = 4;
            robe.Attributes.BonusInt = 20;
            robe.Attributes.CastSpeed = 1;
            robe.Attributes.SpellDamage = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            return robe;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Chef's Hat of Focus";
            cap.Hue = Utility.RandomMinMax(50, 550);
            cap.BaseArmorRating = Utility.Random(20, 50);
            cap.AbsorptionAttributes.CastingFocus = 15;
            cap.ArmorAttributes.SelfRepair = 3;
            cap.Attributes.BonusInt = 10;
            cap.Attributes.RegenMana = 3;
            cap.SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            cap.ColdBonus = 5;
            cap.EnergyBonus = 5;
            cap.FireBonus = 10;
            cap.PhysicalBonus = 5;
            cap.PoisonBonus = 5;
            return cap;
        }

        private Item CreateAxe()
        {
            TwoHandedAxe axe = new TwoHandedAxe();
            axe.Name = "Axe of the Juggernaut";
            axe.Hue = Utility.RandomMinMax(200, 400);
            axe.MinDamage = Utility.Random(30, 80);
            axe.MaxDamage = Utility.Random(80, 110);
            axe.Attributes.BonusHits = 20;
            axe.Attributes.DefendChance = 10;
            axe.Slayer = SlayerName.EarthShatter;
            axe.WeaponAttributes.HitPhysicalArea = 30;
            return axe;
        }

        public TeutonicRelicChest(Serial serial) : base(serial)
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
