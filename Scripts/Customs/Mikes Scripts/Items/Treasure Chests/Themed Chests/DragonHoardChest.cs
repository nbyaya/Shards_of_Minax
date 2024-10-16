using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DragonHoardChest : WoodenChest
    {
        [Constructable]
        public DragonHoardChest()
        {
            Name = "Dragon's Hoard";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Fire Drake", 0), 0.22);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Draconian Wine", 1491), 0.18);
            AddItem(CreateNamedItem<TreasureLevel3>("Dragon's Prize"), 0.21);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Green Wyrm", 0), 0.25);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 7000)), 0.15);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Potion of Dragon's Breath"), 0.15);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateApron(), 0.2);
            AddItem(CreateCap(), 0.2);
            AddItem(CreateWarHammer(), 0.2);
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
            note.NoteString = "To he who dares to steal from the mighty dragon, peril awaits.";
            note.TitleString = "Dragon's Warning";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new VikingSword(), new WarAxe(), new Longsword());
            weapon.Name = "Dragon's Claw";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(40, 80);
            return weapon;
        }

        private Item CreateApron()
        {
            FullApron apron = new FullApron();
            apron.Name = "Chef's Gourmet Apron";
            apron.Hue = Utility.RandomMinMax(250, 750);
            apron.ClothingAttributes.SelfRepair = 4;
            apron.Attributes.BonusInt = 5;
            apron.Attributes.RegenStam = 2;
            apron.SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
            return apron;
        }

        private Item CreateCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Jester's Merry Cap";
            cap.Hue = Utility.RandomMinMax(500, 800);
            cap.BaseArmorRating = Utility.Random(15, 50);
            cap.AbsorptionAttributes.EaterEnergy = 15;
            cap.ArmorAttributes.LowerStatReq = 10;
            cap.Attributes.Luck = 70;
            cap.Attributes.ReflectPhysical = 15;
            cap.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
            cap.ColdBonus = 10;
            cap.EnergyBonus = 10;
            cap.FireBonus = 10;
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateWarHammer()
        {
            WarHammer warHammer = new WarHammer();
            warHammer.Name = "Blacksmith's WarHammer";
            warHammer.Hue = Utility.RandomMinMax(400, 700);
            warHammer.MinDamage = Utility.Random(30, 90);
            warHammer.MaxDamage = Utility.Random(90, 150);
            warHammer.Attributes.BonusHits = 20;
            warHammer.Attributes.EnhancePotions = 15;
            warHammer.WeaponAttributes.DurabilityBonus = 30;
            warHammer.WeaponAttributes.BloodDrinker = 25;
            warHammer.SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
            warHammer.SkillBonuses.SetValues(1, SkillName.Mining, 20.0);
            return warHammer;
        }

        public DragonHoardChest(Serial serial) : base(serial)
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
