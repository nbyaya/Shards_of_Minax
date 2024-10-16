using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FlowerPowerChest : WoodenChest
    {
        [Constructable]
        public FlowerPowerChest()
        {
            Name = "Flower Power";
            Hue = Utility.Random(1, 1800);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<Emerald>("Peace Sign Stone"), 0.28);
            AddItem(CreateNamedItem<Apple>("Magic Mushroom"), 0.29);
            AddItem(CreateNamedItem<TreasureLevel2>("Hippie Love Token"), 0.26);
            AddItem(CreateNamedItem<GoldEarrings>("Free Spirit Hoops"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.12);
            AddItem(CreateNamedItem<Garlic>("Herbal Essence"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Chill"), 0.20);
            AddItem(CreateJArmor(), 0.2);
            AddItem(CreateBoots(), 0.2);
            AddItem(CreateHelm(), 0.2);
            AddItem(CreateScimitar(), 0.2);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Peace, love, and happiness.";
            note.TitleString = "Hippie's Handbook";
            return note;
        }

        private Item CreateJArmor()
        {
            BaseClothing armor = Utility.RandomList<BaseClothing>(new Shirt(), new Kilt());
            armor.Name = "Hippie Threads";
            armor.Hue = Utility.RandomList(1, 1800);
            armor.Attributes.BonusDex = 20;
            armor.ClothingAttributes.LowerStatReq = 3;
            armor.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            return armor;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Go-Go Boots of Agility";
            boots.Hue = Utility.RandomMinMax(950, 980);
            boots.Attributes.BonusDex = 20;
            boots.Attributes.WeaponSpeed = 5;
            boots.ClothingAttributes.LowerStatReq = 3;
            boots.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            return boots;
        }

        private Item CreateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Courtier's Regal Circlet";
            helm.Hue = Utility.RandomMinMax(700, 950);
            helm.BaseArmorRating = Utility.RandomMinMax(35, 60);
            helm.AbsorptionAttributes.EaterCold = 10;
            helm.ArmorAttributes.SelfRepair = 3;
            helm.Attributes.BonusInt = 15;
            helm.Attributes.NightSight = 1;
            helm.SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 5;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Apep's Coiled Scimitar";
            scimitar.Hue = Utility.RandomMinMax(200, 300);
            scimitar.MinDamage = Utility.RandomMinMax(20, 60);
            scimitar.MaxDamage = Utility.RandomMinMax(60, 90);
            scimitar.Attributes.SpellChanneling = 1;
            scimitar.Slayer = SlayerName.ReptilianDeath;
            scimitar.Slayer2 = SlayerName.Ophidian;
            scimitar.WeaponAttributes.HitPoisonArea = 30;
            scimitar.WeaponAttributes.HitManaDrain = 15;
            scimitar.SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            return scimitar;
        }

        public FlowerPowerChest(Serial serial) : base(serial)
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
