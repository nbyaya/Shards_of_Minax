using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RoguesHiddenChest : WoodenChest
    {
        [Constructable]
        public RoguesHiddenChest()
        {
            Name = "Rogue's Hidden Chest";
            Hue = 1175;

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Thief's Prize", 1234), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel2>("Heist's Spoils"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Blackened Dagger Earring", 1195), 0.15);
            AddItem(new Gold(Utility.Random(1, 3500)), 0.19);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Swift Movement"), 0.15);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Silent Step", 1198), 0.14);
            AddItem(CreateRandomNecromancyReagent(), 0.12);
            AddItem(CreateRandomClothing(), 0.13);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateGloves(), 0.20);
            AddItem(CreateHelm(), 0.20);
            AddItem(CreateAxe(), 0.20);
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
            note.NoteString = "To he who finds this, be warned: shadows watch.";
            note.TitleString = "Rogue's Secret";
            return note;
        }

        private Item CreateRandomNecromancyReagent()
        {
            MandrakeRoot reagent = new MandrakeRoot();
            reagent.Name = "Assassin's Herb";
            return reagent;
        }

        private Item CreateRandomClothing()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Cloak of Shadows";
            return cloak;
        }

        private Item CreateWeapon()
        {
            Dagger weapon = new Dagger();
            weapon.Name = "Blade of Deception";
            weapon.Hue = Utility.RandomList(1, 1199);
            weapon.MaxDamage = Utility.Random(2, 4);
            return weapon;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Fletcher's Precision Gloves";
            gloves.Hue = Utility.RandomMinMax(300, 1300);
            gloves.Attributes.BonusDex = 20;
            gloves.Attributes.Luck = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Fletching, 25.0);
            gloves.PhysicalBonus = 10;
            gloves.ColdBonus = 5;
            return gloves;
        }

        private Item CreateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Pyre Plate Helm";
            helm.Hue = Utility.RandomMinMax(500, 600);
            helm.BaseArmorRating = Utility.Random(50, 80);
            helm.AbsorptionAttributes.CastingFocus = 10;
            helm.ArmorAttributes.DurabilityBonus = 20;
            return helm;
        }

        private Item CreateAxe()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Rune Axe";
            axe.Hue = Utility.RandomMinMax(300, 450);
            axe.MinDamage = Utility.Random(25, 60);
            axe.MaxDamage = Utility.Random(60, 90);
            axe.Attributes.SpellChanneling = 1;
            axe.Attributes.LowerManaCost = 15;
            axe.Slayer = SlayerName.OrcSlaying;
            axe.WeaponAttributes.MageWeapon = 1;
            axe.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return axe;
        }

        public RoguesHiddenChest(Serial serial) : base(serial)
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
