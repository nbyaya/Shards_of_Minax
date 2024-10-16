using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MondainsDarkSecretsChest : WoodenChest
    {
        [Constructable]
        public MondainsDarkSecretsChest()
        {
            Name = "Mondain's Dark Secrets Chest";
            Hue = 1175;

            // Add items to the chest
            AddItem(CreateEmerald(), 0.18);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel3>("Dark Sorcerer's Relic"), 0.14);
            AddItem(CreateColoredItem<GoldEarrings>("Necromancer's Sigil Earring", 1107), 0.05);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.18);
            AddItem(CreateNamedItem<Apple>("Cursed Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Shadows"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Forbidden", 1123), 0.15);
            AddItem(CreateRandomNecromancyReagent(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Seer of the Abyss"), 0.12);
            AddItem(CreateRandomWand(), 0.14);
            AddItem(CreateRandomClothing(), 0.18);
            AddItem(CreateArmor(), 0.2);
            AddItem(CreateBowcraftersCloak(), 0.2);
            AddItem(CreateFrostwardensPlateGloves(), 0.2);
            AddItem(CreateLongsword(), 0.2);
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
            emerald.Name = "Eye of Mondain";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Power comes at a cost, darkness binds its bearer.";
            note.TitleString = "Mondain's Prophecy";
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

        private Item CreateRandomNecromancyReagent()
        {
            // Assuming RandomNecromancyReagent is a class that handles random necromancy reagents
            MandrakeRoot reagent = new MandrakeRoot();
            reagent.Name = "Mondain's Herb";
            return reagent;
        }

        private Item CreateRandomWand()
        {
            // Assuming RandomWand is a class that handles random wands
            Shaft wand = new Shaft();
            wand.Name = "Wand of the Occult";
            return wand;
        }

        private Item CreateRandomClothing()
        {
            // Assuming RandomClothing is a class that handles random clothing
            FancyShirt clothing = new FancyShirt();
            clothing.Name = "Cloak of Darkness";
            return clothing;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of Despair";
            armor.Hue = Utility.RandomMinMax(1, 1176);
            armor.BaseArmorRating = Utility.Random(25, 55);
            return armor;
        }

        private Item CreateBowcraftersCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Bowcrafter's Protective Cloak";
            cloak.Hue = Utility.RandomMinMax(450, 1450);
            cloak.ClothingAttributes.LowerStatReq = 3;
            cloak.Attributes.DefendChance = 10;
            cloak.SkillBonuses.SetValues(0, SkillName.Fletching, 25.0);
            return cloak;
        }

        private Item CreateFrostwardensPlateGloves()
        {
            PlateGloves gloves = new PlateGloves();
            gloves.Name = "Frostwarden's Plate Gloves";
            gloves.Hue = Utility.RandomMinMax(600, 650);
            gloves.BaseArmorRating = Utility.Random(40, 70);
            gloves.AbsorptionAttributes.EaterCold = 15;
            gloves.ArmorAttributes.MageArmor = 1;
            gloves.Attributes.BonusHits = 10;
            gloves.Attributes.SpellDamage = 5;
            gloves.SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0);
            gloves.ColdBonus = 20;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 0;
            gloves.PhysicalBonus = 15;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Excalibur's Legacy";
            longsword.Hue = Utility.RandomMinMax(950, 960);
            longsword.MinDamage = Utility.Random(40, 60);
            longsword.MaxDamage = Utility.Random(90, 110);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.DefendChance = 10;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitLightning = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            longsword.SkillBonuses.SetValues(1, SkillName.Chivalry, 15.0);
            return longsword;
        }

        public MondainsDarkSecretsChest(Serial serial) : base(serial)
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
