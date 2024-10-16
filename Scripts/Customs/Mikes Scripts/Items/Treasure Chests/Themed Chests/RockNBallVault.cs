using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RockNBallVault : WoodenChest
    {
        [Constructable]
        public RockNBallVault()
        {
            Name = "Rock 'n' Roll Vault";
            Hue = Utility.Random(1, 1500);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateEmeraldItem("Guitar Pick of Legends"), 0.21);
            AddItem(CreateRandomInstrumentItem("Air Guitar"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel3>("Mixtape of the Ages"), 0.23);
            AddItem(CreateNamedItem<GoldEarrings>("Earrings of the Rockstar"), 0.36);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.15);
            AddItem(CreatePotion(), 0.19);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateDress(), 0.20);
            AddItem(CreateGloves(), 0.20);
            AddItem(CreateClub(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmeraldItem(string name)
        {
            Emerald emerald = new Emerald();
            emerald.Name = name;
            return emerald;
        }

        private Item CreateRandomInstrumentItem(string name)
        {
            Lute instrument = new Lute();
            instrument.Name = name;
            return instrument;
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
            note.NoteString = "The era of legendary rock.";
            note.TitleString = "Rockstar's Diary";
            return note;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Elixir of Heavy Metal";
            return potion;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Rock Anthem Armor";
            armor.Hue = Utility.RandomList(1, 1500);
            return armor;
        }

        private Item CreateDress()
        {
            PlainDress dress = new PlainDress();
            dress.Name = "Flower Child Sundress";
            dress.Hue = Utility.RandomMinMax(350, 1400);
            dress.ClothingAttributes.MageArmor = 1;
            dress.Attributes.RegenHits = 2;
            dress.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            dress.SkillBonuses.SetValues(1, SkillName.Healing, 20.0);
            return dress;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Courtesan's Whispering Gloves";
            gloves.Hue = Utility.RandomMinMax(100, 500);
            gloves.BaseArmorRating = Utility.Random(20, 50);
            gloves.Attributes.BonusDex = 10;
            gloves.Attributes.NightSight = 1;
            gloves.SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 10;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 5;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateClub()
        {
            Club club = new Club();
            club.Name = "Samson's Jawbone";
            club.Hue = Utility.RandomMinMax(250, 450);
            club.MinDamage = Utility.Random(30, 70);
            club.MaxDamage = Utility.Random(70, 110);
            club.Attributes.BonusStr = 15;
            club.Attributes.RegenStam = 5;
            club.Slayer = SlayerName.OrcSlaying;
            club.WeaponAttributes.HitPhysicalArea = 30;
            club.WeaponAttributes.BloodDrinker = 10;
            club.SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
            return club;
        }

        public RockNBallVault(Serial serial) : base(serial)
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
