using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArcaneTreasureChest : WoodenChest
    {
        [Constructable]
        public ArcaneTreasureChest()
        {
            Name = "Arcane Treasure Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new Spellbook(), 0.1);
            AddItem(new Spellbook(), 0.05);
            AddItem(CreateSimpleNote(), 0.1);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Elixir of Arcane Power", 1487), 0.2);
            AddItem(CreateRandomScroll(), 0.05);
            AddItem(new Spellbook(), 0.15);
            AddItem(CreateColoredItem<Spellbook>("Ancient Grimoire", 1765), 0.12);
            AddItem(CreateJarmor(), 0.2);
            AddItem(CreateGoldItem("Mystical Coin"), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Arcane Crystal"), 0.05);
            AddItem(CreateRandomNecroScroll(), 0.05);
            AddItem(CreateFeatheredHat(), 0.2);
            AddItem(CreateBoneHelm(), 0.2);
            AddItem(CreateGnarledStaff(), 0.2);
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
            note.NoteString = "The runes inscribed here are too ancient and complex to decipher!";
            note.TitleString = "Ancient Mage's Journal";
            return note;
        }

        private Item CreateRandomScroll()
        {
            int scrollID = Utility.Random(1, 7);
            switch (scrollID)
            {
                case 1: return new ClumsyScroll();
                case 2: return new CreateFoodScroll();
                case 3: return new FeeblemindScroll();
                case 4: return new HealScroll();
                case 5: return new MagicArrowScroll();
                case 6: return new NightSightScroll();
                case 7: return new WeakenScroll();
                default: return new ClumsyScroll();
            }
        }

        private Item CreateJarmor()
        {
            int armorType = Utility.Random(2, 4);
            BaseArmor armor;
            switch (armorType)
            {
                case 2: armor = new PlateArms(); break;
                case 3: armor = new PlateChest(); break;
                case 4: armor = new PlateLegs(); break;
                default: armor = new PlateChest(); break;
            }
            armor.Name = "Mage's Robe";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(20, 50);
            return armor;
        }

        private Item CreateRandomNecroScroll()
        {
            int scrollID = Utility.Random(1, 10);
            switch (scrollID)
            {
                case 1: return new AnimateDeadScroll();
                case 2: return new BloodOathScroll();
                case 3: return new CorpseSkinScroll();
                case 4: return new CurseWeaponScroll();
                case 5: return new EvilOmenScroll();
                case 6: return new HorrificBeastScroll();
                case 7: return new LichFormScroll();
                case 8: return new MindRotScroll();
                case 9: return new PainSpikeScroll();
                case 10: return new PoisonStrikeScroll();
                default: return new AnimateDeadScroll();
            }
        }

        private Item CreateFeatheredHat()
        {
            FeatheredHat hat = new FeatheredHat();
            hat.Name = "Mystic's Feathered Hat";
            hat.Hue = Utility.RandomMinMax(1, 1000);
            hat.Attributes.BonusInt = 15;
            hat.Attributes.EnhancePotions = 10;
            hat.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            hat.SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);
            hat.Resistances.Energy = 20;
            hat.Resistances.Poison = 10;
            return hat;
        }

        private Item CreateBoneHelm()
        {
            BoneHelm helm = new BoneHelm();
            helm.Name = "Nature's Embrace Helm";
            helm.Hue = Utility.RandomMinMax(1, 1000);
            helm.BaseArmorRating = Utility.Random(20, 65);
            helm.AbsorptionAttributes.EaterFire = 20;
            helm.ArmorAttributes.SelfRepair = 5;
            helm.Attributes.RegenHits = 5;
            helm.Attributes.RegenStam = 5;
            helm.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            helm.SkillBonuses.SetValues(1, SkillName.Herding, 15.0);
            return helm;
        }

        private Item CreateGnarledStaff()
        {
            GnarledStaff staff = new GnarledStaff();
            staff.Name = "Geomancer's Staff";
            staff.Hue = Utility.RandomMinMax(300, 600);
            staff.MinDamage = Utility.Random(20, 80);
            staff.MaxDamage = Utility.Random(80, 140);
            staff.Attributes.SpellChanneling = 1;
            staff.Attributes.RegenStam = 5;
            staff.Slayer = SlayerName.EarthShatter;
            staff.WeaponAttributes.MageWeapon = 10;
            staff.WeaponAttributes.ResistPhysicalBonus = 20;
            staff.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            staff.SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
            return staff;
        }

        public ArcaneTreasureChest(Serial serial) : base(serial)
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
