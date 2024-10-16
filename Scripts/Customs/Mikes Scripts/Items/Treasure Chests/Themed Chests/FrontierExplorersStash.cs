using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FrontierExplorersStash : WoodenChest
    {
        [Constructable]
        public FrontierExplorersStash()
        {
            Name = "Frontier Explorer's Stash";
            Hue = Utility.Random(1, 1300);

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Pioneer's Jewel", 2610), 0.20);
            AddItem(CreateSimpleNote(), 0.31);
            AddItem(new Gold(Utility.Random(1, 4300)), 0.26);
            AddItem(CreateNamedItem<Spyglass>("Rancher's Scope"), 0.28);
            AddItem(CreateRandomClothing(), 0.25);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateSkullCap(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateCleaver(), 0.20);
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
            note.NoteString = "Wild lands and untamed frontiers.";
            note.TitleString = "Cowboy's Memoir";
            return note;
        }

        private Item CreateRandomClothing()
        {
            // Simulate adding random clothing item with "Western Outfit" name
            FancyShirt clothing = new FancyShirt(); // Assuming RandomClothing is a valid class
            clothing.Name = "Western Outfit";
            clothing.Hue = Utility.RandomList(1, 1300);
            return clothing;
        }

        private Item CreateWeapon()
        {
            Bow weapon = new Bow(); // Assuming JWEAPON is a valid class
            weapon.Name = "Gunslinger's Revolver";
            return weapon;
        }

        private Item CreateSkullCap()
        {
            SkullCap skullCap = new SkullCap();
            skullCap.Name = "Assassin's Masked Cap";
            skullCap.Hue = Utility.RandomList(1, 1000);
            skullCap.Attributes.NightSight = 1;
            skullCap.Attributes.BonusDex = 10;
            skullCap.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            skullCap.SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
            return skullCap;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Nottingham Stalker's Leggings";
            legs.Hue = Utility.RandomMinMax(250, 550);
            legs.BaseArmorRating = Utility.RandomMinMax(30, 50);
            legs.ArmorAttributes.MageArmor = 1;
            legs.Attributes.RegenStam = 5;
            legs.Attributes.NightSight = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            legs.PhysicalBonus = 15;
            legs.ColdBonus = 5;
            return legs;
        }

        private Item CreateCleaver()
        {
            Cleaver cleaver = new Cleaver();
            cleaver.Name = "Mehrune's Cleaver";
            cleaver.Hue = Utility.RandomMinMax(750, 900);
            cleaver.MinDamage = Utility.RandomMinMax(15, 45);
            cleaver.MaxDamage = Utility.RandomMinMax(45, 75);
            cleaver.Attributes.BonusDex = 10;
            cleaver.Attributes.LowerManaCost = 10;
            cleaver.Slayer = SlayerName.DaemonDismissal;
            cleaver.WeaponAttributes.HitLeechStam = 15;
            cleaver.WeaponAttributes.HitManaDrain = 10;
            cleaver.SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            return cleaver;
        }

        public FrontierExplorersStash(Serial serial) : base(serial)
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
