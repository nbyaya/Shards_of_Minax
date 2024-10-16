using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PolishRoyalChest : WoodenChest
    {
        [Constructable]
        public PolishRoyalChest()
        {
            Name = "Polish Royal Chest";
            Hue = 2172;

            // Add items to the chest
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel4>("Royal Relic"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Jewel of the Vistula", 1174), 0.18);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Royal Feast Delight"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Kraków's Finest Ale"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Polish Court", 1194), 0.19);
            AddItem(CreateNamedItem<Spyglass>("Royal Seer's Spyglass"), 0.12);
            AddItem(CreateLoot<Shaft>("Wand of the Polish Magi"), 0.14);
            AddItem(CreateLoot<FancyDress>("Royal Polish Garb"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreatePlainDress(), 0.20);
            AddItem(CreateBoneHelm(), 0.20);
            AddItem(CreateCrossbow(), 0.20);
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
            emerald.Name = "Crown Jewel of Poland";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "To the glory of the Polish Crown.";
            note.TitleString = "Royal Decree";
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

        private Item CreateLoot<T>(string name) where T : Item, new()
        {
            T loot = new T();
            loot.Name = name;
            return loot;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Sword of Polish Royalty";
            weapon.Hue = 1156;
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreatePlainDress()
        {
            PlainDress dress = new PlainDress();
            dress.Name = "Courtesan's Graceful Kimono";
            dress.Hue = Utility.RandomMinMax(100, 1500);
            dress.Attributes.BonusDex = 10;
            dress.Attributes.BonusInt = 10;
            dress.SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
            dress.SkillBonuses.SetValues(1, SkillName.Musicianship, 20.0);
            return dress;
        }

        private Item CreateBoneHelm()
        {
            BoneHelm helm = new BoneHelm();
            helm.Name = "Necromancer's Hood";
            helm.Hue = Utility.RandomMinMax(10, 250);
            helm.BaseArmorRating = Utility.Random(30, 60);
            helm.AbsorptionAttributes.EaterPoison = 20;
            helm.ArmorAttributes.SelfRepair = 5;
            helm.Attributes.BonusInt = 30;
            helm.Attributes.SpellDamage = 10;
            helm.SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
            helm.ColdBonus = 15;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 20;
            return helm;
        }

        private Item CreateCrossbow()
        {
            Crossbow crossbow = new Crossbow();
            crossbow.Name = "Two-shot Crossbow";
            crossbow.Hue = Utility.RandomMinMax(250, 450);
            crossbow.MinDamage = Utility.Random(15, 55);
            crossbow.MaxDamage = Utility.Random(55, 95);
            crossbow.Attributes.BonusHits = 20;
            crossbow.Attributes.Luck = 100;
            crossbow.Slayer = SlayerName.DragonSlaying;
            crossbow.WeaponAttributes.HitFireball = 25;
            crossbow.WeaponAttributes.HitLightning = 15;
            crossbow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return crossbow;
        }

        public PolishRoyalChest(Serial serial) : base(serial)
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
