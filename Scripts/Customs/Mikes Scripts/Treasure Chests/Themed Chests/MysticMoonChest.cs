using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MysticMoonChest : WoodenChest
    {
        [Constructable]
        public MysticMoonChest()
        {
            Name = "Mystic Moon Chest";
            Hue = 1128;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Clefairy's Moonstone", 1123), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(CreateNamedItem<TreasureLevel4>("Night Trainer's Moon Cache"), 0.13);
            AddItem(CreateColoredItem<GoldEarrings>("Umbreon's Night Earring", 1154), 0.10);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.19);
            AddItem(CreateNamedItem<Apple>("Gastly's Phantom Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Darkrai's Dream Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Night Seeker", 1119), 0.16);
            AddItem(CreateNamedItem<Spyglass>("Mystic Moon Explorer's Spyglass"), 0.11);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateKilt(), 0.20);
            AddItem(CreateChainChest(), 0.20);
            AddItem(CreateKatana(), 0.20);
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
            note.NoteString = "The moonlight reveals secrets of the Pokémon world.";
            note.TitleString = "Lunala's Message";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Katana();
            weapon.Name = "Blade of Scyther";
            weapon.Hue = Utility.RandomMinMax(1, 1120);
            weapon.MaxDamage = Utility.Random(25, 65);
            return weapon;
        }

        private Item CreateKilt()
        {
            Kilt kilt = new Kilt();
            kilt.Name = "Elixir Expert's Kilt";
            kilt.Hue = Utility.RandomMinMax(150, 1150);
            kilt.Attributes.LowerManaCost = 10;
            kilt.Attributes.RegenMana = 3;
            kilt.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            return kilt;
        }

        private Item CreateChainChest()
        {
            ChainChest chest = new ChainChest();
            chest.Name = "Serenade's Embrace";
            chest.Hue = Utility.RandomMinMax(250, 750);
            chest.BaseArmorRating = Utility.Random(35, 70);
            chest.AbsorptionAttributes.ResonanceKinetic = 10;
            chest.ArmorAttributes.SelfRepair = 5;
            chest.Attributes.BonusStr = 15;
            chest.Attributes.BonusInt = 10;
            chest.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            chest.SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 5;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 15;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Masamune Katana";
            katana.Hue = Utility.RandomMinMax(700, 900);
            katana.MinDamage = Utility.Random(35, 65);
            katana.MaxDamage = Utility.Random(65, 105);
            katana.Attributes.AttackChance = 15;
            katana.Attributes.BonusDex = 15;
            katana.Slayer = SlayerName.OrcSlaying;
            katana.Slayer2 = SlayerName.Terathan;
            katana.WeaponAttributes.HitMagicArrow = 30;
            katana.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
            return katana;
        }

        public MysticMoonChest(Serial serial) : base(serial)
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
