using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MimeSilentChest : WoodenChest
    {
        [Constructable]
        public MimeSilentChest()
        {
            Name = "Mime's Silent Chest";
            Hue = 1155;

            // Add items to the chest
            AddItem(CreateNamedItem<Sapphire>("Mime's Blue Tear"), 0.05);
            AddItem(CreateSimpleNote(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel1>("Box of Silent Surprises"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Mime's Silent Bell Earring"), 0.15);
            AddItem(new Gold(Utility.Random(1, 3000)), 1.0);
            AddItem(CreateNamedItem<Apple>("Painted White Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Mime's Secret Drink"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Stealthy Mime", 1938), 0.12);
            AddItem(CreateNamedItem<Spyglass>("Performance Observer's Monocle"), 0.04);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateHammerlordCap(), 0.20);
            AddItem(CreateCrownOfTheAbyss(), 0.20);
            AddItem(CreateGriswoldEdge(), 0.20);
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
            note.NoteString = "Silence speaks volumes in the world of mimes.";
            note.TitleString = "Mime's Mute Note";
            return note;
        }

        private Item CreateLootItem(string name, Type type)
        {
            Item item = Loot.Construct(type);
            item.Name = name;
            return item;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Mime's Invisible Sword";
            weapon.Hue = 1158;
            weapon.MaxDamage = Utility.Random(20, 45);
            return weapon;
        }

        private Item CreateHammerlordCap()
        {
            Cap cap = new Cap();
            cap.Name = "Hammerlord's Cap";
            cap.Hue = Utility.RandomMinMax(400, 1400);
            cap.Attributes.BonusStr = 15;
            cap.Attributes.CastSpeed = 1;
            cap.SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
            return cap;
        }

        private Item CreateCrownOfTheAbyss()
        {
            CloseHelm helm = new CloseHelm();
            helm.Name = "Crown of the Abyss";
            helm.Hue = Utility.RandomMinMax(750, 1000);
            helm.BaseArmorRating = Utility.Random(25, 65);
            helm.Attributes.WeaponDamage = 40;
            helm.Attributes.ReflectPhysical = -30;
            helm.Attributes.BonusStr = 25;
            helm.SkillBonuses.SetValues(0, SkillName.Parry, -15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 10;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 10;
            return helm;
        }

        private Item CreateGriswoldEdge()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Griswold's Edge";
            axe.Hue = Utility.RandomMinMax(200, 400);
            axe.MinDamage = Utility.Random(30, 65);
            axe.MaxDamage = Utility.Random(65, 95);
            axe.Attributes.SpellChanneling = 1;
            axe.Attributes.LowerManaCost = 10;
            axe.WeaponAttributes.MageWeapon = 1;
            axe.WeaponAttributes.HitMagicArrow = 20;
            return axe;
        }

        public MimeSilentChest(Serial serial) : base(serial)
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
