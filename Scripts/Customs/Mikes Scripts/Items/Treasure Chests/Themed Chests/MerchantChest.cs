using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MerchantChest : WoodenChest
    {
        [Constructable]
        public MerchantChest()
        {
            Name = "Merchant’s Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateSapphire(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Merchant’s Ale", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Merchant’s Goods"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Scale Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Market", 1775), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fine Merchant’s Wine", 1486), 0.08);
            AddItem(CreateGoldItem("Golden Token"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Trader", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Coin Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Merchant’s Trusted Spyglass"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of Mysterious Brew"), 0.20);
            AddItem(CreateBodySash(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateScimitar(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSapphire()
        {
            Sapphire sapphire = new Sapphire();
            sapphire.Name = "Sapphire of the Trade";
            return sapphire;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
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
            note.NoteString = "I have made a fortune selling my wares.";
            note.TitleString = "Merchant’s Ledger";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to a Secret Warehouse";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateBodySash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Fisherman's Vest";
            sash.Hue = Utility.RandomMinMax(400, 1300);
            sash.Attributes.BonusInt = 8;
            sash.SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            sash.SkillBonuses.SetValues(1, SkillName.Cooking, 10.0);
            return sash;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Blade Dancer's Plate Arms";
            arms.Hue = Utility.RandomMinMax(600, 900);
            arms.BaseArmorRating = Utility.Random(40, 70);
            arms.AbsorptionAttributes.EaterKinetic = 10;
            arms.ArmorAttributes.LowerStatReq = 10;
            arms.Attributes.BonusHits = 15;
            arms.Attributes.DefendChance = 7;
            arms.SkillBonuses.SetValues(0, SkillName.Swords, 10.0);
            arms.SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
            arms.ColdBonus = 5;
            arms.EnergyBonus = 10;
            arms.FireBonus = 15;
            arms.PhysicalBonus = 20;
            arms.PoisonBonus = 5;
            return arms;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Goldbrand Scimitar";
            scimitar.Hue = Utility.RandomMinMax(450, 500);
            scimitar.MinDamage = Utility.Random(25, 60);
            scimitar.MaxDamage = Utility.Random(60, 90);
            scimitar.Attributes.BonusStr = 10;
            scimitar.Attributes.RegenStam = 3;
            scimitar.Slayer = SlayerName.FlameDousing;
            scimitar.WeaponAttributes.HitFireball = 50;
            scimitar.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            scimitar.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return scimitar;
        }

        public MerchantChest(Serial serial) : base(serial)
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
