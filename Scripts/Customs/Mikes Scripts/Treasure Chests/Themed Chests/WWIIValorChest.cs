using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WWIIValorChest : WoodenChest
    {
        [Constructable]
        public WWIIValorChest()
        {
            Name = "WWII Valor Chest";
            Hue = Utility.Random(1, 1945);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<SilverNecklace>("Dog Tag of Bravery"), 0.25);
            AddItem(CreateNamedItem<MaxxiaScroll>("War Bond"), 0.20);
            AddItem(CreateNamedItem<TreasureLevel4>("Soldier's Rations"), 0.22);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("1940s Vintage Wine"), 0.13);
            AddItem(CreateRandomClothing(), 0.17);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreatePlateHelm(), 0.20);
            AddItem(CreateScimitar(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "We shall fight on the beaches...";
            note.TitleString = "Winston's Speeches";
            return note;
        }

        private Item CreateRandomClothing()
        {
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Army Uniform";
            clothing.Hue = Utility.RandomList(1, 1945);
            return clothing;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs());
            armor.Name = "Battlefield Protection";
            return armor;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Sneak's Silent Shoes";
            shoes.Hue = Utility.RandomMinMax(1000, 1800);
            shoes.Attributes.BonusDex = 10;
            shoes.Attributes.DefendChance = 10;
            shoes.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            shoes.SkillBonuses.SetValues(1, SkillName.Snooping, 15.0);
            return shoes;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Immortal King's Iron Crown";
            helm.Hue = Utility.RandomMinMax(600, 900);
            helm.BaseArmorRating = Utility.Random(35, 70);
            helm.AbsorptionAttributes.EaterFire = 15;
            helm.ArmorAttributes.SelfRepair = 5;
            helm.Attributes.BonusStr = 40;
            helm.Attributes.BonusStam = 20;
            helm.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            helm.FireBonus = 20;
            helm.PhysicalBonus = 20;
            return helm;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Barbarossa Scimitar";
            scimitar.Hue = Utility.RandomMinMax(250, 450);
            scimitar.MinDamage = Utility.Random(20, 70);
            scimitar.MaxDamage = Utility.Random(70, 110);
            scimitar.Attributes.BonusDex = 15;
            scimitar.Attributes.Luck = 100;
            scimitar.Slayer = SlayerName.ElementalHealth;
            scimitar.WeaponAttributes.BattleLust = 20;
            scimitar.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return scimitar;
        }

        public WWIIValorChest(Serial serial) : base(serial)
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
