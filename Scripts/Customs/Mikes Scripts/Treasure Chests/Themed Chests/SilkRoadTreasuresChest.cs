using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SilkRoadTreasuresChest : WoodenChest
    {
        [Constructable]
        public SilkRoadTreasuresChest()
        {
            Name = "Silk Road Treasures";
            Hue = Utility.Random(1, 1200);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.04);
            AddItem(CreateColoredItem<Sapphire>("Desert's Mirage Crystal", 1157), 0.28);
            AddItem(CreateNamedItem<MaxxiaScroll>("Trade Barter"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel4>("Spices of the Orient"), 0.24);
            AddItem(CreateNamedItem<GoldEarrings>("Caravan Trader's Earrings"), 0.35);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.14);
            AddItem(CreateRandomReagent(), 0.18);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateSandals(), 0.20);
            AddItem(CreateLeatherGorget(), 0.20);
            AddItem(CreateKryss(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "The riches of the West meet the treasures of the East.";
            note.TitleString = "Trader's Journal";
            return note;
        }

        private Item CreateRandomReagent()
        {
            Garlic reagent = new Garlic();
            reagent.Name = "Rare Oriental Herb";
            return reagent;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Kryss());
            weapon.Name = "Silk Road Defender's Blade";
            weapon.Hue = Utility.RandomList(1, 1200);
            return weapon;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Scribe's Enlightened Sandals";
            sandals.Hue = Utility.RandomMinMax(250, 1150);
            sandals.Attributes.BonusInt = 10;
            sandals.Attributes.RegenMana = 3;
            sandals.SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
            return sandals;
        }

        private Item CreateLeatherGorget()
        {
            LeatherGorget gorget = new LeatherGorget();
            gorget.Name = "String of Ears";
            gorget.Hue = Utility.RandomMinMax(150, 350);
            gorget.BaseArmorRating = Utility.RandomMinMax(25, 60);
            gorget.AbsorptionAttributes.EaterDamage = 10;
            gorget.ArmorAttributes.SelfRepair = 5;
            gorget.Attributes.LowerManaCost = 10;
            gorget.Attributes.WeaponDamage = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
            gorget.ColdBonus = 5;
            gorget.EnergyBonus = 10;
            gorget.FireBonus = 10;
            gorget.PhysicalBonus = 15;
            gorget.PoisonBonus = 10;
            return gorget;
        }

        private Item CreateKryss()
        {
            Kryss kryss = new Kryss();
            kryss.Name = "Musketeer's Rapier";
            kryss.Hue = Utility.RandomMinMax(400, 600);
            kryss.MinDamage = Utility.RandomMinMax(15, 55);
            kryss.MaxDamage = Utility.RandomMinMax(55, 90);
            kryss.Attributes.BonusInt = 5;
            kryss.Attributes.NightSight = 1;
            kryss.Slayer = SlayerName.Repond;
            kryss.WeaponAttributes.HitDispel = 25;
            kryss.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            kryss.SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
            return kryss;
        }

        public SilkRoadTreasuresChest(Serial serial) : base(serial)
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
