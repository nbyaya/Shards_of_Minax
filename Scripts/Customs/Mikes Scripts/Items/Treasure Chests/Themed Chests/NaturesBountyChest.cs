using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NaturesBountyChest : WoodenChest
    {
        [Constructable]
        public NaturesBountyChest()
        {
            Name = "Nature's Bounty";
            Hue = Utility.Random(1, 1250);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Emerald>("Leaf of the Elders"), 0.22);
            AddItem(CreateNamedItem<Apple>("Golden Apple of Life"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel3>("Heart of the Forest"), 0.24);
            AddItem(CreateNamedItem<GoldEarrings>("Earrings of the Harvest Moon"), 0.35);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.12);
            AddItem(CreateNamedItem<Garlic>("Mysterious Herb"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Nature's Grace"), 0.18);
            AddItem(CreateJArmor(), 0.21);
            AddItem(CreateNamedItem<Drums>("Music of the Forest"), 0.15);
            AddItem(CreateNinjaTabi(), 0.20);
            AddItem(CreatePlateGorget(), 0.20);
            AddItem(CreateWarAxe(), 0.20);
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
            note.NoteString = "The woods whisper their secrets.";
            note.TitleString = "Forest Keeper's Memoir";
            return note;
        }

        private Item CreateJArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Guardian of the Woods";
            armor.Hue = Utility.RandomList(1, 1250);
            armor.BaseArmorRating = Utility.Random(30, 60);
            return armor;
        }

        private Item CreateNinjaTabi()
        {
            NinjaTabi tabi = new NinjaTabi();
            tabi.Name = "Shadow Walker's Tabi";
            tabi.Hue = Utility.RandomMinMax(500, 1500);
            tabi.ClothingAttributes.SelfRepair = 3;
            tabi.Attributes.BonusDex = 15;
            tabi.Attributes.NightSight = 1;
            tabi.SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
            tabi.SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
            return tabi;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Masked Avenger's Voice";
            gorget.Hue = Utility.RandomMinMax(900, 999);
            gorget.BaseArmorRating = Utility.Random(30, 60);
            gorget.Attributes.BonusInt = 10;
            gorget.Attributes.RegenStam = 5;
            gorget.SkillBonuses.SetValues(0, SkillName.Provocation, 10.0);
            gorget.ColdBonus = 5;
            gorget.EnergyBonus = 10;
            gorget.FireBonus = 5;
            gorget.PhysicalBonus = 15;
            gorget.PoisonBonus = 5;
            return gorget;
        }

        private Item CreateWarAxe()
        {
            WarAxe warAxe = new WarAxe();
            warAxe.Name = "Ebony WarAxe of Vampires";
            warAxe.Hue = Utility.RandomMinMax(800, 950);
            warAxe.MinDamage = Utility.Random(30, 65);
            warAxe.MaxDamage = Utility.Random(65, 100);
            warAxe.Attributes.BonusStr = 15;
            warAxe.Attributes.RegenHits = 5;
            warAxe.Slayer = SlayerName.BloodDrinking;
            warAxe.WeaponAttributes.HitLeechHits = 20;
            warAxe.WeaponAttributes.HitLeechMana = 10;
            warAxe.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            return warAxe;
        }

        public NaturesBountyChest(Serial serial) : base(serial)
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
