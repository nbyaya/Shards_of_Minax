using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ChocolatierTreasureChest : WoodenChest
    {
        [Constructable]
        public ChocolatierTreasureChest()
        {
            Name = "Chocolatier's Treasure";
            Hue = 1176;

            // Add items to the chest
            AddItem(CreateNamedItem<Sapphire>("Chocolate Heart Gem"), 0.05);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel3>("Box of Chocolate Coins"), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Cocoa Bean Earrings"), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 1.0);
            AddItem(CreateNamedItem<Apple>("Chocolate Dipped Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Chocolate Liqueur"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Cocoa Explorer", 1174), 0.10);
            AddItem(CreateRandomGem("Chocolate Diamond"), 0.17);
            AddItem(CreateNamedItem<Spyglass>("Cocoa Seer's Spyglass"), 0.04);
            AddItem(CreateRandomNecromancyReagent("Chocolate Truffle"), 0.12);
            AddItem(CreateRandomWand("Wand of Cocoa Magic"), 0.14);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateBoots(), 1.0);
            AddItem(CreateGloves(), 0.20);
            AddItem(CreateMaul(), 0.20);
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
            note.NoteString = "Richness beyond gold lies in cocoa.";
            note.TitleString = "Chocolatier's Note";
            return note;
        }

        private Item CreateRandomGem(string name)
        {
            Item gem = Loot.RandomGem();
            gem.Name = name;
            return gem;
        }

        private Item CreateRandomNecromancyReagent(string name)
        {
            Item reagent = Loot.RandomNecromancyReagent();
            reagent.Name = name;
            return reagent;
        }

        private Item CreateRandomWand(string name)
        {
            BaseWand wand = Loot.RandomWand();
            wand.Name = name;
            return wand;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Loot.RandomWand();
            weapon.Name = "Chocolate Coated Blade";
            weapon.Hue = 1175;
            weapon.MaxDamage = Utility.Random(20, 60);
            return weapon;
        }

        private Item CreateBoots()
        {
            Boots boots = new Boots();
            boots.Name = "Boots of the Deep Caverns";
            boots.Hue = Utility.RandomMinMax(500, 1500);
            boots.ClothingAttributes.LowerStatReq = 3;
            boots.Attributes.BonusDex = 7;
            boots.Attributes.RegenStam = 2;
            boots.SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
            return boots;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Hexweaver's Mystical Gloves";
            gloves.Hue = Utility.RandomMinMax(1, 1000);
            gloves.BaseArmorRating = Utility.Random(15, 55);
            gloves.AbsorptionAttributes.EaterCold = 20;
            gloves.Attributes.CastSpeed = 1;
            gloves.Attributes.SpellChanneling = 1;
            gloves.SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
            gloves.ColdBonus = 10;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 10;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private Item CreateMaul()
        {
            Maul maul = new Maul();
            maul.Name = "Kaom's Maul";
            maul.Hue = Utility.RandomMinMax(500, 700);
            maul.MinDamage = Utility.Random(30, 90);
            maul.MaxDamage = Utility.Random(90, 130);
            maul.Attributes.BonusHits = 25;
            maul.Attributes.LowerRegCost = 15;
            maul.Slayer = SlayerName.DragonSlaying;
            maul.WeaponAttributes.HitFireArea = 40;
            maul.WeaponAttributes.BattleLust = 20;
            maul.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
            return maul;
        }

        public ChocolatierTreasureChest(Serial serial) : base(serial)
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
