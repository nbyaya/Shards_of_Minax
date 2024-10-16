using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class HomewardBoundChest : WoodenChest
    {
        [Constructable]
        public HomewardBoundChest()
        {
            Name = "Homeward Bound Chest";
            Hue = Utility.Random(1, 1944);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateEmerald(), 0.25);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel2>("Homecooked Meal"), 0.22);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4100)), 0.14);
            AddItem(CreateNamedItem<GoldEarrings>("Mother's Keepsake Earrings"), 0.17);
            AddItem(CreateRandomReagent(), 1.0); // Probability of 1.0, always added
            AddItem(CreateShoes(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateBow(), 0.20);
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
            emerald.Name = "Family Heirloom";
            return emerald;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Returning Soldier's Uniform";
            clothing.Hue = Utility.RandomList(1, 1944);
            return clothing;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Together again at last.";
            note.TitleString = "Homecoming Letters";
            return note;
        }

        private Item CreateRandomReagent()
        {
            Garlic reagent = new Garlic();
            reagent.Name = "Scent of Home";
            return reagent;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Urbanite's Sneakers";
            shoes.Hue = Utility.RandomMinMax(200, 700);
            shoes.Attributes.BonusDex = 10;
            shoes.Attributes.BonusStam = 5;
            shoes.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            return shoes;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest vestment = new LeatherChest();
            vestment.Name = "WhiteMage's Divine Vestment";
            vestment.Hue = Utility.RandomMinMax(900, 1000);
            vestment.BaseArmorRating = Utility.RandomMinMax(25, 60);
            vestment.AbsorptionAttributes.EaterPoison = 15;
            vestment.ArmorAttributes.MageArmor = 1;
            vestment.Attributes.RegenHits = 10;
            vestment.Attributes.SpellChanneling = 1;
            vestment.SkillBonuses.SetValues(0, SkillName.Healing, 25.0);
            vestment.ColdBonus = 10;
            vestment.EnergyBonus = 10;
            vestment.FireBonus = 10;
            vestment.PhysicalBonus = 20;
            vestment.PoisonBonus = 20;
            return vestment;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Yumi of Empress Jingu";
            bow.Hue = Utility.RandomMinMax(100, 150);
            bow.MinDamage = Utility.RandomMinMax(20, 60);
            bow.MaxDamage = Utility.RandomMinMax(60, 95);
            bow.Attributes.BonusInt = 15;
            bow.Attributes.LowerRegCost = 10;
            bow.Slayer = SlayerName.Repond;
            bow.WeaponAttributes.HitManaDrain = 20;
            bow.WeaponAttributes.MageWeapon = 1;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
            return bow;
        }

        public HomewardBoundChest(Serial serial) : base(serial)
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
