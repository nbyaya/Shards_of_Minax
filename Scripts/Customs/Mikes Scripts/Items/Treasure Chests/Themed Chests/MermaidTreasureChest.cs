using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MermaidTreasureChest : WoodenChest
    {
        [Constructable]
        public MermaidTreasureChest()
        {
            Name = "Mermaid's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Diamond>("Pearl of the Ocean"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Mermaid's Kiss", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Mermaid's Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Seashell Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Sea", 1152), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Siren's Song", 1153), 0.08);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Sea", 1153), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Starfish Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<FishingPole>("Mermaid's Fishing Pole"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Water"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateFeatheredHat(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateDagger(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
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
            note.NoteString = "I found this chest in a sunken ship.";
            note.TitleString = "Mermaid's Note";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Mermaid's Grotto";
            map.Bounds = new Rectangle2D(1000, 1200, 200, 200);
            map.NewPin = new Point2D(1100, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Pitchfork weapon = new Pitchfork();
            weapon.Name = "Triton's Trident";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest());
            armor.Name = "Mermaid's Scale";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFeatheredHat()
        {
            FeatheredHat hat = new FeatheredHat();
            hat.Name = "Intriguer's Feathered Hat";
            hat.Hue = Utility.RandomMinMax(300, 1300);
            hat.Attributes.BonusDex = 15;
            hat.Attributes.NightSight = 1;
            hat.SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
            return hat;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Witch's Cursed Robe";
            chest.Hue = Utility.RandomMinMax(500, 750);
            chest.BaseArmorRating = Utility.Random(30, 60);
            chest.AbsorptionAttributes.EaterPoison = 20;
            chest.ArmorAttributes.DurabilityBonus = 20;
            chest.Attributes.LowerManaCost = 10;
            chest.Attributes.BonusMana = 20;
            chest.SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 15;
            chest.PoisonBonus = 20;
            return chest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Venom's Sting";
            dagger.Hue = Utility.RandomMinMax(300, 400);
            dagger.MinDamage = Utility.Random(10, 40);
            dagger.MaxDamage = Utility.Random(40, 70);
            dagger.Attributes.LowerManaCost = 10;
            dagger.Attributes.WeaponSpeed = 10;
            dagger.WeaponAttributes.HitPoisonArea = 30;
            dagger.WeaponAttributes.BattleLust = 15;
            dagger.SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            return dagger;
        }

        public MermaidTreasureChest(Serial serial) : base(serial)
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
