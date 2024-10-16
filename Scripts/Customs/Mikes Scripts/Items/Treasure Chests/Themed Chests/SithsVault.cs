using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SithsVault : WoodenChest
    {
        [Constructable]
        public SithsVault()
        {
            Name = "Sith's Vault";
            Hue = Utility.Random(1, 1600);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateEmerald(), 0.27);
            AddItem(CreateRandomWand(), 0.32);
            AddItem(CreateNamedItem<TreasureLevel3>("Ancient Sith Holocron"), 0.25);
            AddItem(CreateNamedItem<SilverNecklace>("Dark Lord's Medallion"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5200)), 0.17);
            AddItem(CreateRandomPotion(), 0.19);
            AddItem(CreateWeapon(), 0.23);
            AddItem(CreateRandomReagent(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateHalberd(), 0.20);
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
            emerald.Name = "Dark Crystal of Power";
            return emerald;
        }

        private Item CreateRandomWand()
        {
            Longsword wand = new Longsword();
            wand.Name = "Red Lightsaber";
            wand.Hue = 1300;
            return wand;
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
            note.NoteString = "The Dark Side of the Force is a pathway to many abilities.";
            note.TitleString = "Sith's Codex";
            return note;
        }

        private Item CreateRandomPotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Sith Alchemical Potion";
            return potion;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Halberd());
            weapon.Name = "Darth's Wrath";
            return weapon;
        }

        private Item CreateRandomReagent()
        {
            Garlic reagent = new Garlic();
            reagent.Name = "Sith Artifact";
            return reagent;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Mistletoe Muffler";
            muffler.Hue = Utility.RandomMinMax(60, 70);
            muffler.Attributes.BonusDex = 8;
            muffler.SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
            return muffler;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Stormshield";
            plateChest.Hue = Utility.RandomMinMax(450, 850);
            plateChest.BaseArmorRating = Utility.RandomMinMax(50, 80);
            plateChest.AbsorptionAttributes.EaterEnergy = 20;
            plateChest.ArmorAttributes.DurabilityBonus = 20;
            plateChest.Attributes.BonusInt = 15;
            plateChest.Attributes.SpellDamage = 10;
            plateChest.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 25;
            plateChest.FireBonus = 5;
            plateChest.PhysicalBonus = 15;
            plateChest.PoisonBonus = 5;
            return plateChest;
        }

        private Item CreateHalberd()
        {
            Halberd halberd = new Halberd();
            halberd.Name = "Power Pole Halberd";
            halberd.Hue = Utility.RandomMinMax(150, 200);
            halberd.MinDamage = Utility.RandomMinMax(30, 70);
            halberd.MaxDamage = Utility.RandomMinMax(70, 100);
            halberd.Attributes.BonusStam = 20;
            halberd.Attributes.BonusStr = 10;
            halberd.Slayer = SlayerName.ReptilianDeath;
            halberd.WeaponAttributes.SelfRepair = 3;
            halberd.WeaponAttributes.HitHarm = 20;
            halberd.SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
            return halberd;
        }

        public SithsVault(Serial serial) : base(serial)
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
