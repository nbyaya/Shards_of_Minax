using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ForbiddenAlchemistsCache : WoodenChest
    {
        [Constructable]
        public ForbiddenAlchemistsCache()
        {
            Name = "Forbidden Alchemist's Cache";
            Hue = 1175;

            // Add items to the chest
            AddItem(CreateEmerald(), 0.18);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel3>("Alchemical Relic"), 0.14);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Potion of Ultimate Despair", 1225), 0.19);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.19);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Brew of Shadows", 1172), 0.10);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Darkness Draught", 2994), 0.15);
            AddItem(CreateRandomReagent("Forbidden Herb"), 0.14);
            AddItem(CreateNamedItem<Spyglass>("Obsidian Observer"), 0.11);
            AddItem(CreateRandomWand("Wand of Noxious Brews"), 0.13);
            AddItem(CreateRandomNecromancyReagent("Reagent of the Damned"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateSash(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
            AddItem(CreateBow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Emerald CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Emerald of Elixirs";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "In darkness, true alchemy is revealed.";
            note.TitleString = "Master Alchemist's Secret";
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

        private Item CreateRandomReagent(string name)
        {
            // Assumes RandomReagent is a subclass of Item; adjust if needed
            MandrakeRoot reagent = new MandrakeRoot();
            reagent.Name = name;
            return reagent;
        }

        private Item CreateRandomWand(string name)
        {
            // Assumes RandomWand is a subclass of Item; adjust if needed
            Shaft wand = new Shaft();
            wand.Name = name;
            return wand;
        }

        private Item CreateRandomNecromancyReagent(string name)
        {
            // Assumes RandomNecromancyReagent is a subclass of Item; adjust if needed
            MandrakeRoot reagent = new MandrakeRoot();
            reagent.Name = name;
            return reagent;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Blade of Alchemical Ruin";
            weapon.Hue = 1174;
            weapon.MaxDamage = Utility.Random(25, 60);
            return weapon;
        }

        private Item CreateSash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Cleric's Sacred Sash";
            sash.Hue = Utility.RandomMinMax(220, 1220);
            sash.Attributes.RegenMana = 3;
            sash.SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
            sash.SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
            return sash;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Fortune's Gorget";
            gorget.Hue = Utility.RandomMinMax(700, 950);
            gorget.BaseArmorRating = Utility.Random(25, 60);
            gorget.AbsorptionAttributes.EaterPoison = 10;
            gorget.ArmorAttributes.LowerStatReq = 15;
            gorget.Attributes.Luck = 100;
            gorget.SkillBonuses.SetValues(0, SkillName.TasteID, 10.0);
            gorget.ColdBonus = 5;
            gorget.EnergyBonus = 15;
            gorget.FireBonus = 5;
            gorget.PhysicalBonus = 10;
            gorget.PoisonBonus = 15;
            return gorget;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Genji Bow";
            bow.Hue = Utility.RandomMinMax(800, 900);
            bow.MinDamage = Utility.Random(25, 65);
            bow.MaxDamage = Utility.Random(65, 105);
            bow.Attributes.DefendChance = 10;
            bow.Attributes.RegenHits = 5;
            bow.Slayer = SlayerName.OrcSlaying;
            bow.WeaponAttributes.HitPoisonArea = 30;
            bow.WeaponAttributes.DurabilityBonus = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return bow;
        }

        public ForbiddenAlchemistsCache(Serial serial) : base(serial)
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
