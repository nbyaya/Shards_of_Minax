using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GoldRushBountyChest : WoodenChest
    {
        [Constructable]
        public GoldRushBountyChest()
        {
            Name = "Gold Rush Bounty";
            Hue = Utility.Random(1, 1600);

            // Add items to the chest
			AddItem(new MaxxiaScroll(), 0.7);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.27);
            AddItem(CreateNamedItem<Pickaxe>("Seeker's Tool"), 0.28);
            AddItem(CreateRandomGem(), 0.23);
            AddItem(CreateWeapon(), 0.2);
            AddItem(CreateShoes(), 0.2);
            AddItem(CreateLeatherChest(), 0.2);
            AddItem(CreateWarHammer(), 0.2);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Eureka! Gold in them hills!";
            note.TitleString = "Miner's Journal";
            return note;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateRandomGem()
        {
            // Replace with actual implementation to create a random gem
            Diamond gem = new Diamond();
            gem.Name = "Gold Nugget";
            return gem;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new WarHammer());
            weapon.Name = "Pioneer's Pick";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MinDamage = Utility.Random(35, 85);
            weapon.MaxDamage = Utility.Random(85, 125);
            return weapon;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Thief's Silent Shoes";
            shoes.Hue = Utility.RandomMinMax(900, 1700);
            shoes.ClothingAttributes.LowerStatReq = 2;
            shoes.Attributes.BonusDex = 10;
            shoes.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            shoes.SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
            return shoes;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Locksley Leather Chest";
            chest.Hue = Utility.RandomMinMax(250, 550);
            chest.BaseArmorRating = Utility.Random(30, 50);
            chest.ArmorAttributes.DurabilityBonus = 20;
            chest.Attributes.BonusStam = 20;
            chest.Attributes.DefendChance = 15;
            chest.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            chest.PhysicalBonus = 15;
            chest.PoisonBonus = 5;
            return chest;
        }

        private Item CreateWarHammer()
        {
            WarHammer hammer = new WarHammer();
            hammer.Name = "Volendrung WarHammer";
            hammer.Hue = Utility.RandomMinMax(150, 200);
            hammer.MinDamage = Utility.Random(35, 85);
            hammer.MaxDamage = Utility.Random(85, 125);
            hammer.Attributes.BonusHits = 20;
            hammer.Attributes.AttackChance = 10;
            hammer.WeaponAttributes.HitHarm = 30;
            hammer.SkillBonuses.SetValues(0, SkillName.Macing, 25.0);
            return hammer;
        }

        public GoldRushBountyChest(Serial serial) : base(serial)
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
