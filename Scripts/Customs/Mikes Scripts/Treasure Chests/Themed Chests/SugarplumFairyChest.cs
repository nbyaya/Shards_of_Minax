using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SugarplumFairyChest : WoodenChest
    {
        [Constructable]
        public SugarplumFairyChest()
        {
            Name = "Sugarplum Fairy's Chest";
            Hue = Utility.Random(1, 1165);

            // Add items to the chest
            AddItem(CreateNamedItem<Emerald>("Enchanted Sugar Gem"), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel2>("Fairy's Candy Stash"), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Gumdrop Earrings"), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 1.0);
            AddItem(CreateNamedItem<Apple>("Candied Apple"), 0.08);
            AddItem(CreateNamedItem<GreaterHealPotion>("Pixie's Fizzy Drink"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Candy Dancer", 1161), 0.19);
            AddItem(CreateNamedItem<Emerald>("Candy Jewel"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Sugar Sight Spyglass"), 0.12);
            AddItem(CreateNamedItem<Harp>("Sweet Melody Harp"), 0.13);
            AddItem(CreateArmor(), 1.0);
            AddItem(CreateShoes(), 0.2);
            AddItem(CreateBoneHelm(), 0.2);
            AddItem(CreateBow(), 0.2);
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

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The sweetest dreams are made of these candies.";
            note.TitleString = "Fairy's Sweet Whisper";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Candy Armor";
            armor.Hue = Utility.RandomList(1, 1166);
            armor.BaseArmorRating = Utility.Random(25, 55);
            return armor;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Gloves of Stonemasonry";
            shoes.Hue = Utility.RandomMinMax(600, 1600);
            shoes.ClothingAttributes.DurabilityBonus = 4;
            shoes.Attributes.LowerRegCost = 5;
            shoes.SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
            return shoes;
        }

        private Item CreateBoneHelm()
        {
            BoneHelm helm = new BoneHelm();
            helm.Name = "Coven's Shadowed Hood";
            helm.Hue = Utility.RandomMinMax(1, 1000);
            helm.BaseArmorRating = Utility.Random(20, 65);
            helm.AbsorptionAttributes.EaterPoison = 25;
            helm.ArmorAttributes.SelfRepair = 5;
            helm.Attributes.NightSight = 1;
            helm.SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            helm.SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 10;
            return helm;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Doomfletch's Prism";
            bow.Hue = Utility.RandomMinMax(700, 900);
            bow.MinDamage = Utility.Random(20, 60);
            bow.MaxDamage = Utility.Random(60, 90);
            bow.Attributes.LowerManaCost = 15;
            bow.Attributes.AttackChance = 10;
            bow.Slayer = SlayerName.ArachnidDoom;
            bow.WeaponAttributes.HitFireball = 25;
            bow.WeaponAttributes.HitColdArea = 25;
            bow.WeaponAttributes.HitLightning = 25;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return bow;
        }

        public SugarplumFairyChest(Serial serial) : base(serial)
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
