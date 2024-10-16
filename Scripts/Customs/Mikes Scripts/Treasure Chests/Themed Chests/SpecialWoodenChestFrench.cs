using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialWoodenChestFrench : WoodenChest
    {
        [Constructable]
        public SpecialWoodenChestFrench()
        {
            Name = "Coffre des Lumières";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Saphir de la Raison"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Champagne des Philosophes", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Trésor des Encyclopédistes"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Collier de Voltaire"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Émeraude du Progrès", 1775), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Absinthe des Artistes", 0), 0.08);
            AddItem(CreateGoldItem("Pièce Louis XV"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Bottes de Montesquieu", 1618), 0.19);
            AddItem(CreateColoredItem<GoldEarrings>("Boucles d’oreilles de Madame de Pompadour", 2213), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Lunette de Descartes"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Fiole du Siècle des Lumières"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreateLeatherArms(), 0.20);
            AddItem(CreateLongsword(), 0.20);
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

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Ecrasez l’infâme!";
            note.TitleString = "Lettre de Diderot";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Carte au Trésor de Rousseau";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Montaigne";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Balladeer's Muffler";
            muffler.Hue = Utility.RandomMinMax(250, 1250);
            muffler.ClothingAttributes.SelfRepair = 3;
            muffler.Attributes.BonusInt = 7;
            muffler.SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
            muffler.SkillBonuses.SetValues(1, SkillName.Discordance, 10.0);
            return muffler;
        }

        private Item CreateLeatherArms()
        {
            LeatherArms arms = new LeatherArms();
            arms.Name = "Shuriken Bracers";
            arms.Hue = Utility.RandomMinMax(500, 600);
            arms.BaseArmorRating = Utility.Random(30, 60);
            arms.AbsorptionAttributes.EaterFire = 10;
            arms.ArmorAttributes.DurabilityBonus = 10;
            arms.Attributes.AttackChance = 10;
            arms.Attributes.BonusStr = 5;
            arms.SkillBonuses.SetValues(0, SkillName.Archery, 10.0);
            arms.ColdBonus = 10;
            arms.EnergyBonus = 10;
            arms.FireBonus = 15;
            arms.PhysicalBonus = 10;
            arms.PoisonBonus = 10;
            return arms;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Blackrazor";
            longsword.Hue = Utility.RandomMinMax(800, 900);
            longsword.MinDamage = Utility.Random(25, 75);
            longsword.MaxDamage = Utility.Random(75, 105);
            longsword.Attributes.BonusHits = 20;
            longsword.Attributes.SpellChanneling = 1;
            longsword.Slayer = SlayerName.BloodDrinking;
            longsword.WeaponAttributes.HitLeechHits = 50;
            longsword.WeaponAttributes.HitManaDrain = 25;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public SpecialWoodenChestFrench(Serial serial) : base(serial)
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
