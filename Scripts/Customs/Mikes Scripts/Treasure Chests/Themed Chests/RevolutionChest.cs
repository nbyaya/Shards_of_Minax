using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RevolutionChest : WoodenChest
    {
        [Constructable]
        public RevolutionChest()
        {
            Name = "Coffre de la Révolution";
            Hue = Utility.RandomList(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Saphir de la Liberté"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Vin de la Bastille", 1645), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Trésor des Sans-Culottes"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Collier de la Marianne"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Émeraude de l’Égalité", 1775), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Cognac de l’Empereur", 0), 0.08);
            AddItem(CreateGoldItem("Pièce Napoléon"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Bottes du Général", 1618), 0.19);
            AddItem(CreateColoredItem<GoldEarrings>("Boucles d’oreilles de la Reine", 2213), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Lunette de Voltaire"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Fiole de la Guillotine"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "Vive la République!";
            note.TitleString = "Journal de Robespierre";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Carte au Trésor de Lafayette";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "La Fayette";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "La Marseillaise";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Harmonist's Soft Shoes";
            shoes.Hue = Utility.RandomMinMax(400, 1400);
            shoes.ClothingAttributes.LowerStatReq = 2;
            shoes.Attributes.BonusStam = 5;
            shoes.SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
            shoes.SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
            return shoes;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Silent Step Tabi";
            legs.Hue = Utility.RandomMinMax(500, 600);
            legs.BaseArmorRating = Utility.Random(25, 55);
            legs.AbsorptionAttributes.ResonanceKinetic = 10;
            legs.Attributes.BonusStam = 10;
            legs.Attributes.NightSight = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            legs.ColdBonus = 5;
            legs.EnergyBonus = 10;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 15;
            legs.PoisonBonus = 5;
            return legs;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Serpent's Venom Dagger";
            dagger.Hue = Utility.RandomMinMax(500, 600);
            dagger.MinDamage = Utility.Random(10, 50);
            dagger.MaxDamage = Utility.Random(50, 90);
            dagger.Attributes.NightSight = 1;
            dagger.Attributes.BonusDex = 15;
            dagger.Slayer = SlayerName.SnakesBane;
            dagger.WeaponAttributes.HitPoisonArea = 40;
            dagger.SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
            return dagger;
        }

        public RevolutionChest(Serial serial) : base(serial)
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
